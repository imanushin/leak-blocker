#include "Common.h"

#define ENDS_WITH_SLASH(UnicodeString) ((UnicodeString.Length) >= 2 && (UnicodeString.Buffer[UnicodeString.Length / 2 - 1] == L'\\'))

typedef struct
{
    PFLT_CALLBACK_DATA Data;
    PCFLT_RELATED_OBJECTS FltObjects;
    VOID (*ProcessingCallback)(PFILE_NAME fileName, PVOID parameter);
    PVOID Parameter;
    
    UNICODE_STRING              VolumeNameString;
    UNICODE_STRING              RelatedFileName;
    UNICODE_STRING              TargetFileName;
} AUXILIARY_FILENAME_QUERY_PARAMETERS, *PAUXILIARY_FILENAME_QUERY_PARAMETERS;

typedef struct
{
    PFLT_FILE_NAME_INFORMATION FileNameInformation;
    VOID (*ProcessingCallback)(PFILE_NAME fileName, PVOID parameter);
    PVOID Parameter;
    BOOLEAN Result;
} MAIN_FILENAME_QUERY_PARAMETERS, *PMAIN_FILENAME_QUERY_PARAMETERS;

typedef struct
{
    PFLT_VOLUME Volume;
    VOID (*ProcessingCallback)(PVOLUME_NAME fileName, PVOID parameter);
    PVOID Parameter;
    LONG Size;
} VOLUME_QUERY_PARAMETERS, *PVOLUME_QUERY_PARAMETERS;

VOID AuxiliaryFileNameQuery(PVOLUME_NAME volumeName, PVOID parameter);
VOID MainFileNameQuery(PVOID memory, LONG size, PVOID parameter);
VOID InitializeVolumeName(PVOID memory, LONG size, PVOID parameter);
VOID InitializeFileNameAuxiliary(PVOID memory, LONG size, PVOID parameter);

LONG IsDirectory(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects)
{
    NTSTATUS    status;
    BOOLEAN     flag;

    if(data->Iopb->Parameters.Create.Options & FILE_DIRECTORY_FILE)
        return 1;
    else if(data->Iopb->Parameters.Create.Options & FILE_NON_DIRECTORY_FILE)
        return 0;
    else
    {
        status = FltIsDirectory(relatedObjects->FileObject, relatedObjects->Instance, &flag);
        if(!NT_SUCCESS(status))
            return 0;
        else
            return flag ? 1 : 0;
    }
}

VOID GetCurrentUserIdentifier(PUSER_IDENTIFIER user)
{
    NTSTATUS                        status;
    PACCESS_TOKEN                   primaryToken = NULL;    
    PTOKEN_USER                     tokenUserStruct = NULL;
    BOOLEAN                         copyOnOpen;
    BOOLEAN                         effectiveOnly;
    SECURITY_IMPERSONATION_LEVEL    impersonationLevel;
    UNICODE_STRING                  string;

    RtlZeroMemory(user, sizeof(USER_IDENTIFIER));
    string.Buffer = user->Value;
    string.Length = 0;
    string.MaximumLength = sizeof(user->Value);

    primaryToken = PsReferenceImpersonationToken(PsGetCurrentThread(), &copyOnOpen, &effectiveOnly, &impersonationLevel); 
    if(primaryToken == NULL)
        primaryToken = PsReferencePrimaryToken(PsGetCurrentProcess()); 

    status = SeQueryInformationToken(primaryToken, TokenUser, (PVOID*)(&tokenUserStruct));    
    if(status != STATUS_SUCCESS) 
    {
        DebugMessage("SeQueryInformationToken failed with error %x.", status);
        DebugBreak();

        PsDereferencePrimaryToken(primaryToken);
        return;
    }
    
    status = RtlConvertSidToUnicodeString(&string, tokenUserStruct->User.Sid, FALSE);
    if(status != STATUS_SUCCESS)      
    {
        DebugMessage("RtlConvertSidToUnicodeString failed with error %x.", status);
        DebugBreak();
    }

    ExFreePool(tokenUserStruct);
    PsDereferencePrimaryToken(primaryToken);
}

VOID QueryVolumeName(PFLT_VOLUME volume, VOID (*processingCallback)(PVOLUME_NAME volumeName, PVOID parameter), PVOID parameter)
{
    NTSTATUS                status;
    VOLUME_QUERY_PARAMETERS parameters;

    if((volume == NULL) || (processingCallback == NULL))
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    status = FltGetVolumeName(volume, NULL, (PULONG)&parameters.Size);
    if(status != STATUS_BUFFER_TOO_SMALL)
    {
        DebugMessage("FltGetVolumeName failed with error %x.", status);
        DebugBreak();
        return;
    }   
    
    parameters.Parameter = parameter;
    parameters.ProcessingCallback = processingCallback;
    parameters.Volume = volume;

    UseMemory(sizeof(VOLUME_NAME) + parameters.Size + 2, InitializeVolumeName, &parameters);
}

VOID InitializeVolumeName(PVOID memory, LONG size, PVOID parameter)
{
    NTSTATUS                    status;
    PVOLUME_QUERY_PARAMETERS    parameters = (PVOLUME_QUERY_PARAMETERS)parameter;
    UNICODE_STRING              string;
    PVOLUME_NAME                result = (PVOLUME_NAME)memory;

    string.Length = 0;
    string.MaximumLength = (USHORT)parameters->Size;
    string.Buffer = (PWCHAR)((PUCHAR)memory + sizeof(VOLUME_NAME));

    status = FltGetVolumeName(parameters->Volume, &string, NULL);
    if(!NT_SUCCESS(status))
    {
        DebugMessage("FltGetVolumeName failed with error %x.", status);
        DebugBreak();
        return;
    }

    result->StringLength = string.Length / sizeof(WCHAR);
    result->TotalSize = size;
    
    parameters->ProcessingCallback(result, parameters->Parameter);
}

VOID QueryFileName(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, VOID (*processingCallback)(PFILE_NAME fileName, PVOID parameter), PVOID parameter)
{    
    AUXILIARY_FILENAME_QUERY_PARAMETERS     auxiliaryParameters = {0};   
    MAIN_FILENAME_QUERY_PARAMETERS          mainParameters = {0};
    PFLT_FILE_NAME_INFORMATION              currentFilenameInfo = NULL;
    NTSTATUS                                status;

    if((data == NULL) || (relatedObjects == NULL) || (processingCallback == NULL))
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    if(relatedObjects->FileObject->FileName.Length == 0)
        return;

    RtlZeroMemory(&mainParameters, sizeof(mainParameters));
    RtlZeroMemory(&auxiliaryParameters, sizeof(auxiliaryParameters));

    status = FltGetFileNameInformation(data, /*FLT_FILE_NAME_OPENED*/ FLT_FILE_NAME_NORMALIZED | FLT_FILE_NAME_QUERY_DEFAULT, &currentFilenameInfo);
    if(NT_SUCCESS(status)) 
    {
        FltParseFileNameInformation(currentFilenameInfo);
        
        mainParameters.FileNameInformation = currentFilenameInfo;
        mainParameters.ProcessingCallback = processingCallback;
        mainParameters.Parameter = parameter;
        mainParameters.Result = FALSE;

        UseMemory(sizeof(FILE_NAME) + currentFilenameInfo->Name.Length + 2, MainFileNameQuery, &mainParameters);

        FltReleaseFileNameInformation(currentFilenameInfo);
    }

    if(!mainParameters.Result)
    {    
        DebugMessage("Default filename query was unsuccessful. Trying alternative query.");

        auxiliaryParameters.Data = data;
        auxiliaryParameters.FltObjects = relatedObjects;
        auxiliaryParameters.ProcessingCallback = processingCallback;
        auxiliaryParameters.Parameter = parameter;

        QueryVolumeName(relatedObjects->Volume, AuxiliaryFileNameQuery, &auxiliaryParameters);
    }
}

VOID MainFileNameQuery(PVOID memory, LONG size, PVOID parameter)
{
    PFILE_NAME                      result = (PFILE_NAME)memory;
    PMAIN_FILENAME_QUERY_PARAMETERS parameters = (PMAIN_FILENAME_QUERY_PARAMETERS)parameter;

    RtlCopyMemory((PWCHAR)((PUCHAR)result + sizeof(FILE_NAME)), parameters->FileNameInformation->Name.Buffer, parameters->FileNameInformation->Name.Length);
    result->StringLength = parameters->FileNameInformation->Name.Length / sizeof(WCHAR);
    result->VolumeNameLength = parameters->FileNameInformation->Volume.Length / sizeof(WCHAR);
    result->TotalSize = size;

    parameters->ProcessingCallback(result, parameters->Parameter);
    parameters->Result = TRUE;
}

VOID AuxiliaryFileNameQuery(PVOLUME_NAME volumeName, PVOID parameter)
{
    PAUXILIARY_FILENAME_QUERY_PARAMETERS  parameters = (PAUXILIARY_FILENAME_QUERY_PARAMETERS)parameter;
    
    parameters->VolumeNameString.Buffer = (PWCHAR)((PUCHAR)volumeName + sizeof(VOLUME_NAME));
    parameters->VolumeNameString.Length = (USHORT)(volumeName->StringLength * sizeof(WCHAR));
    parameters->VolumeNameString.MaximumLength = parameters->VolumeNameString.Length;

    if(ENDS_WITH_SLASH(parameters->VolumeNameString))
        parameters->VolumeNameString.Length -= 2;

    if(parameters->FltObjects->FileObject->RelatedFileObject != NULL)
    {
        RtlCopyMemory(&parameters->RelatedFileName, &parameters->FltObjects->FileObject->RelatedFileObject->FileName, sizeof(UNICODE_STRING));
        if(ENDS_WITH_SLASH(parameters->RelatedFileName))
            parameters->RelatedFileName.Length -= 2;
    }

    RtlCopyMemory(&parameters->TargetFileName, &parameters->FltObjects->FileObject->FileName, sizeof(UNICODE_STRING));
    
    UseMemory(sizeof(FILE_NAME) + parameters->VolumeNameString.Length + parameters->RelatedFileName.Length + parameters->TargetFileName.Length + 2, InitializeFileNameAuxiliary, parameters);
}

VOID InitializeFileNameAuxiliary(PVOID memory, LONG size, PVOID parameter)
{
    PFILE_NAME                              result = (PFILE_NAME)memory;
    PAUXILIARY_FILENAME_QUERY_PARAMETERS    parameters = (PAUXILIARY_FILENAME_QUERY_PARAMETERS)parameter;

    RtlCopyMemory((PWCHAR)((PUCHAR)result + sizeof(FILE_NAME)), parameters->VolumeNameString.Buffer, parameters->VolumeNameString.Length);
    if(parameters->RelatedFileName.Length > 0)
        RtlCopyMemory((PUCHAR)result + sizeof(FILE_NAME) + parameters->VolumeNameString.Length, parameters->RelatedFileName.Buffer, parameters->RelatedFileName.Length);
    RtlCopyMemory((PUCHAR)result + sizeof(FILE_NAME) + parameters->VolumeNameString.Length + parameters->RelatedFileName.Length, parameters->TargetFileName.Buffer, parameters->TargetFileName.Length);
    
    result->StringLength = (parameters->VolumeNameString.Length + parameters->RelatedFileName.Length + parameters->TargetFileName.Length) / sizeof(WCHAR);
    result->VolumeNameLength = parameters->VolumeNameString.Length / sizeof(WCHAR);
    result->TotalSize = size;

    parameters->ProcessingCallback(result, parameters->Parameter);    
}
