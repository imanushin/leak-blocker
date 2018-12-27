#include "Common.h"

#define IS_READ(AccessMask) (((AccessMask & (GENERIC_READ | FILE_READ_DATA)) != 0) ? 1 : 0)
#define IS_WRITE(AccessMask) (((AccessMask & (GENERIC_WRITE | FILE_WRITE_DATA)) != 0) ? 1 : 0)
#define IS_DELETE(Options) (((Options & FILE_DELETE_ON_CLOSE) != 0) ? 1 : 0)

typedef struct
{
    PFLT_CALLBACK_DATA      Data;
    PCFLT_RELATED_OBJECTS   FltObjects;
    PFILE_NAME              FileName;
    BOOLEAN                 Cancel;
    LONG                    Action;
} FILE_NOTIFICATION_PARAMETERS, *PFILE_NOTIFICATION_PARAMETERS;

typedef struct
{
    PCFLT_RELATED_OBJECTS FltObjects;
    PVOLUME_NAME VolumeName;
} VOLUME_NOTIFICATION_PARAMETERS, *PVOLUME_NOTIFICATION_PARAMETERS;

VOID FileNameQueryHandler(PFILE_NAME fileName, PVOID parameter);
VOID SendFileNotification(PVOID memory, LONG size, PVOID parameter);
VOID GetDataTransferActionType(LONG action, PVOID parameter);
VOID VolumeNameQueryHandler(PVOLUME_NAME volumeName, PVOID parameter);
VOID GetRequiredAction(PCONFIGURATION_MESSAGE instanceConfiguration, PVOID parameter);
VOID SendVolumeNotification(PVOID memory, LONG size, PVOID parameter);

KMUTEX          activeInstancesMutex;
PFLT_INSTANCE   activeInstances[4096];
LONG            activeInstancesCount;

VOID InitializeProcessing()
{
    KeInitializeMutex(&activeInstancesMutex, 0); 
}

VOID AddActiveInstance(PFLT_INSTANCE instance)
{
    KeWaitForMutexObject(&activeInstancesMutex, Executive, KernelMode, FALSE, NULL);  
    
    if(activeInstancesCount == 4096)
        DebugMessage("Instance limit was reached.");
    else
    {
        activeInstances[activeInstancesCount] = instance;
        activeInstancesCount++;
    }

    KeReleaseMutex(&activeInstancesMutex, FALSE);   
}

VOID RemoveActiveInstance(PFLT_INSTANCE instance)
{
    LONG i;
    LONG index = -1;

    KeWaitForMutexObject(&activeInstancesMutex, Executive, KernelMode, FALSE, NULL);  
    
    for(i = 0; i < activeInstancesCount; i++)
    {
        if(activeInstances[i] == instance)
        {
            index = i;
            break;
        }
    }

    if(index == -1)
        DebugMessage("Instance %p was not found.", instance);
    else
    {
        for(i = (index + 1); i < activeInstancesCount; i++)
            activeInstances[i - 1] = activeInstances[i];
        activeInstancesCount--;
    }
    
    KeReleaseMutex(&activeInstancesMutex, FALSE);   
}

BOOLEAN CheckActiveInstance(PFLT_INSTANCE instance)
{
    LONG i;
    BOOLEAN result = FALSE;

    KeWaitForMutexObject(&activeInstancesMutex, Executive, KernelMode, FALSE, NULL);  
                 
    for(i = 0; i < activeInstancesCount; i++)
    {
        if(activeInstances[i] == instance)
        {
            result = TRUE;
            break;
        }
    }

    KeReleaseMutex(&activeInstancesMutex, FALSE);    

    return result;
}

VOID Attaching(PCFLT_RELATED_OBJECTS relatedObjects)
{
    VOLUME_NOTIFICATION_PARAMETERS  parameters = {0};
    CONFIGURATION_MESSAGE           emptyConfiguration;
    
    DebugMessage("Attaching new instance. Instance identifier: %p", relatedObjects->Instance);
    
    parameters.FltObjects = relatedObjects;
    QueryVolumeName(relatedObjects->Volume, VolumeNameQueryHandler, &parameters);

    emptyConfiguration.Header.Protocol = PROTOCOL_VERSION;
    emptyConfiguration.Header.TotalSize = sizeof(emptyConfiguration);
    emptyConfiguration.Header.Type = CONFIGURATION_MESSAGE_TYPE;
    emptyConfiguration.InstanceIdentifier = (LONGLONG)relatedObjects->Instance;
    emptyConfiguration.EntriesCount = 0;

    SetInstanceContext(relatedObjects->Filter, relatedObjects->Instance, &emptyConfiguration);

    AddActiveInstance(relatedObjects->Instance);
}

VOID VolumeNameQueryHandler(PVOLUME_NAME volumeName, PVOID parameter)
{
    ((PVOLUME_NOTIFICATION_PARAMETERS)parameter)->VolumeName = volumeName;
    UseMemory(sizeof(VOLUME_IDENTIFIER_NOTIFICATION) - sizeof(VOLUME_NAME) + volumeName->TotalSize, SendVolumeNotification, parameter);
}

VOID SendVolumeNotification(PVOID memory, LONG size, PVOID parameter)
{
    PVOLUME_NOTIFICATION_PARAMETERS parameters = (PVOLUME_NOTIFICATION_PARAMETERS)parameter;
    PVOLUME_IDENTIFIER_NOTIFICATION notification = (PVOLUME_IDENTIFIER_NOTIFICATION)memory;
    
    notification->Header.Protocol = PROTOCOL_VERSION;
    notification->Header.TotalSize = size;
    notification->Header.Type = VOLUME_IDENTIFIER_NOTIFICATION_TYPE;

    notification->InstanceIdentifier = (LONGLONG)parameters->FltObjects->Instance;
    RtlCopyMemory(&notification->VolumeName, parameters->VolumeName, parameters->VolumeName->TotalSize);

    DebugMessage("Volume name: %ws, identifier: %llu", (PWCHAR)((PUCHAR)&notification->VolumeName + sizeof(VOLUME_NAME)), notification->InstanceIdentifier);
    
    SendMessage(&notification->Header);
}

VOID Detaching(PCFLT_RELATED_OBJECTS relatedObjects)
{
    VOLUME_DETACH_NOTIFICATION notification;

    DebugMessage("Detaching instance. Instance identifier: %p", relatedObjects->Instance);
    
    RemoveActiveInstance(relatedObjects->Instance);

    notification.Header.Protocol = PROTOCOL_VERSION;
    notification.Header.TotalSize = sizeof(notification);
    notification.Header.Type = VOLUME_DETACH_NOTIFICATION_TYPE;
    notification.InstanceIdentifier = (LONGLONG)relatedObjects->Instance;
}

BOOLEAN BeforeCreate(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, PVOID *context)
{
    FILE_NOTIFICATION_PARAMETERS parameters = {0};
    BOOLEAN read = IS_READ(data->Iopb->Parameters.Create.SecurityContext->AccessState->OriginalDesiredAccess);
    BOOLEAN write = IS_DELETE(data->Iopb->Parameters.Create.Options) || IS_WRITE(data->Iopb->Parameters.Create.SecurityContext->AccessState->OriginalDesiredAccess);

    DebugMessage("BeforeCreate");

    parameters.Data = data;
    parameters.FltObjects = relatedObjects;

    QueryFileName(data, relatedObjects, FileNameQueryHandler, &parameters);

    *context = (PVOID)parameters.Action;
     
    return parameters.Cancel && !((parameters.Action == ACTION_READONLY) && read && write);
}

VOID AfterCreate(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, PVOID context)
{
    DebugMessage("AfterCreate");

    SetStreamContext(relatedObjects->Filter, relatedObjects->Instance, relatedObjects->FileObject, (LONG)context);
}

VOID FileNameQueryHandler(PFILE_NAME fileName, PVOID parameter)
{
    ((PFILE_NOTIFICATION_PARAMETERS)parameter)->FileName = fileName;
    UseMemory(sizeof(FILE_ACCESS_NOTIFICATION) - sizeof(FILE_NAME) + fileName->TotalSize, SendFileNotification, parameter);
}

VOID SendFileNotification(PVOID memory, LONG size, PVOID parameter)
{
    PFILE_NOTIFICATION_PARAMETERS   parameters = (PFILE_NOTIFICATION_PARAMETERS)parameter;
    PFILE_ACCESS_NOTIFICATION       notification = (PFILE_ACCESS_NOTIFICATION)memory;

    notification->Header.Protocol = PROTOCOL_VERSION;
    notification->Header.TotalSize = size;
    notification->Header.Type = FILE_ACCESS_NOTIFICATION_TYPE;
    
    notification->Directory = IsDirectory(parameters->Data, parameters->FltObjects);
    notification->Delete = IS_DELETE(parameters->Data->Iopb->Parameters.Create.Options);
    notification->Read = IS_READ(parameters->Data->Iopb->Parameters.Create.SecurityContext->AccessState->OriginalDesiredAccess);
    notification->Write = IS_WRITE(parameters->Data->Iopb->Parameters.Create.SecurityContext->AccessState->OriginalDesiredAccess);    
    KeQuerySystemTime((PLARGE_INTEGER)&notification->SystemTime);
    GetCurrentUserIdentifier(&notification->User);
    RtlCopyMemory(&notification->FileName, parameters->FileName, parameters->FileName->TotalSize);
    notification->ProcessIdentifier = FltGetRequestorProcessId(parameters->Data);

    if((notification->Directory != 0) && (notification->Write == 0))
        notification->AppliedAction = ACTION_ALLOW;
    else
        QueryInstanceContext(parameters->FltObjects->Instance, GetRequiredAction, notification);

    parameters->Action = notification->AppliedAction;
    parameters->Cancel = (parameters->Action != ACTION_ALLOW) && !((notification->Write == 0) && (parameters->Action == ACTION_READONLY));

    DebugMessage("File name: %ws, action: %u, user: %ws", (PWCHAR)((PUCHAR)&notification->FileName + sizeof(FILE_NAME)), notification->AppliedAction, notification->User.Value);
    
    SendMessage(&notification->Header);
}

VOID GetRequiredAction(PCONFIGURATION_MESSAGE instanceConfiguration, PVOID parameter)
{
    PFILE_ACCESS_NOTIFICATION   notification = (PFILE_ACCESS_NOTIFICATION)parameter;
    LONG                        i;
    PCONFIGURATION_ENTRY        entries;
    UNICODE_STRING              currentUserIdentifierString;
    UNICODE_STRING              configurationUserIdentifierString;

    entries = (PCONFIGURATION_ENTRY)((PUCHAR)instanceConfiguration + sizeof(CONFIGURATION_MESSAGE));

    for(i = 0; i < instanceConfiguration->EntriesCount; i++)
    {
        RtlInitUnicodeString(&currentUserIdentifierString, notification->User.Value);
        RtlInitUnicodeString(&configurationUserIdentifierString, entries[i].User.Value);

        if(RtlCompareUnicodeString(&currentUserIdentifierString, &configurationUserIdentifierString, TRUE) == 0)
        {
            notification->AppliedAction = entries[i].Action;
            return;
        }
    }

    DebugMessage("User was not found in the configuration.");
}

BOOLEAN BeforeRead(PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS relatedObjects)
{
    LONG action;

    QueryStreamContext(relatedObjects->Instance, relatedObjects->FileObject, GetDataTransferActionType, &action);

    return ((action != ACTION_ALLOW) && (action != ACTION_READONLY));
}

BOOLEAN BeforeWrite(PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS relatedObjects)
{
    LONG action;

    QueryStreamContext(relatedObjects->Instance, relatedObjects->FileObject, GetDataTransferActionType, &action);
    
    return (action != ACTION_ALLOW);
}

VOID GetDataTransferActionType(LONG action, PVOID parameter)
{
    *(PLONG)parameter = action;
}

VOID MessageReceived(PFLT_FILTER filter, PMESSAGE_HEADER message)
{
    PCONFIGURATION_MESSAGE configurationMessage = (PCONFIGURATION_MESSAGE)message;

    DebugMessage("MessageReceived %u", message->TotalSize);
    DebugBreak();

    switch(message->Type)
    {
    case CONFIGURATION_MESSAGE_TYPE:
        if(CheckActiveInstance((PFLT_INSTANCE)configurationMessage->InstanceIdentifier))
            SetInstanceContext(filter, (PFLT_INSTANCE)configurationMessage->InstanceIdentifier, configurationMessage);
        break;
    }
}
