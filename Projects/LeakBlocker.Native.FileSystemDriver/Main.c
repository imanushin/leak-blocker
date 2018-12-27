#include "Common.h"

DRIVER_INITIALIZE   DriverEntry;
PFLT_FILTER         filterHandle;
PFLT_PORT           communicationServerPort;
PFLT_PORT           communicationUserModePort;
PEPROCESS           communicationUserProcess;
KMUTEX              debugMessageMutex;

FLT_PREOP_CALLBACK_STATUS   BeforeCreateRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext);
FLT_PREOP_CALLBACK_STATUS   BeforeReadRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext);
FLT_PREOP_CALLBACK_STATUS   BeforeWriteRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext);
VOID                        ContextCleanup(__in PFLT_CONTEXT Context, __in FLT_CONTEXT_TYPE ContextType);
NTSTATUS                    DriverEntry(__in PDRIVER_OBJECT DriverObject, __in PUNICODE_STRING RegistryPath);
NTSTATUS                    Unload(__in FLT_FILTER_UNLOAD_FLAGS Flags);
NTSTATUS                    InstanceSetup(__in PCFLT_RELATED_OBJECTS FltObjects, __in FLT_INSTANCE_SETUP_FLAGS Flags, __in DEVICE_TYPE VolumeDeviceType, __in FLT_FILESYSTEM_TYPE VolumeFilesystemType);
NTSTATUS                    InstanceQueryTeardown(__in PCFLT_RELATED_OBJECTS FltObjects, __in FLT_INSTANCE_QUERY_TEARDOWN_FLAGS Flags);
NTSTATUS                    UserModeConnect(__in PFLT_PORT ClientPort, __in_opt PVOID ServerPortCookie, __in_bcount_opt(SizeOfContext) PVOID ConnectionContext, __in ULONG SizeOfContext, __deref_out_opt PVOID *ConnectionCookie);
VOID                        UserModeDisconnect(__in_opt PVOID ConnectionCookie);
NTSTATUS                    ReceiveUserModeMessage(IN PVOID PortCookie, IN PVOID InputBuffer OPTIONAL, IN ULONG InputBufferLength, OUT PVOID OutputBuffer OPTIONAL, IN ULONG OutputBufferLength, OUT PULONG ReturnOutputBufferLength);
FLT_POSTOP_CALLBACK_STATUS  AfterCreateRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __in_opt PVOID CompletionContext, __in FLT_POST_OPERATION_FLAGS Flags);

BOOLEAN                     IsProcessingSafe(PFLT_CALLBACK_DATA Data, FLT_POST_OPERATION_FLAGS PostOperationFlags);
FLT_PREOP_CALLBACK_STATUS   CallOperationHandler(BOOLEAN (*handler)(PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS FltObjects), PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS FltObjects);

#define UserModeConnectCallback (PFLT_CONNECT_NOTIFY)UserModeConnect
#define UserModeDisconnectCallback (PFLT_DISCONNECT_NOTIFY)UserModeDisconnect
#define ReceiveUserModeMessageCallback (PFLT_MESSAGE_NOTIFY)ReceiveUserModeMessage
#define UnloadCallback (PFLT_FILTER_UNLOAD_CALLBACK)Unload
#define BeforeCreateCallback (PFLT_PRE_OPERATION_CALLBACK)BeforeCreateRequest
#define BeforeReadCallback (PFLT_PRE_OPERATION_CALLBACK)BeforeReadRequest
#define BeforeWriteCallback (PFLT_PRE_OPERATION_CALLBACK)BeforeWriteRequest
#define ContextCleanupCallback (PFLT_CONTEXT_CLEANUP_CALLBACK)ContextCleanup
#define InstanceSetupCallback (PFLT_INSTANCE_SETUP_CALLBACK)InstanceSetup
#define InstanceQueryTeardownCallback (PFLT_INSTANCE_QUERY_TEARDOWN_CALLBACK)InstanceQueryTeardown
#define AfterCreateCallback (PFLT_POST_OPERATION_CALLBACK)AfterCreateRequest

CONST FLT_CONTEXT_REGISTRATION ContextRegistration[] =
{
    { FLT_STREAM_CONTEXT, 0, ContextCleanupCallback, FLT_VARIABLE_SIZED_CONTEXTS, 'Ctx1' },
    { FLT_STREAMHANDLE_CONTEXT, 0, ContextCleanupCallback, FLT_VARIABLE_SIZED_CONTEXTS, 'Ctx2' },
    { FLT_INSTANCE_CONTEXT, 0, ContextCleanupCallback, FLT_VARIABLE_SIZED_CONTEXTS, 'Ctx3' },
    { FLT_CONTEXT_END }
};

CONST FLT_OPERATION_REGISTRATION ProcessingCallbacks[] = 
{
    { IRP_MJ_CREATE, 0, BeforeCreateCallback, AfterCreateCallback },
    { IRP_MJ_READ, 0, BeforeReadCallback, NULL },
    { IRP_MJ_WRITE, 0, BeforeWriteCallback, NULL },    
    { IRP_MJ_OPERATION_END }
};

CONST FLT_REGISTRATION FilterRegistration = { sizeof(FLT_REGISTRATION), FLT_REGISTRATION_VERSION, 0, ContextRegistration, 
    ProcessingCallbacks, UnloadCallback, InstanceSetupCallback, InstanceQueryTeardownCallback, NULL, NULL, NULL, NULL, NULL };

BOOLEAN IsProcessingSafe(PFLT_CALLBACK_DATA Data, FLT_POST_OPERATION_FLAGS PostOperationFlags)
{
    if((PostOperationFlags & FLTFL_POST_OPERATION_DRAINING) != 0) 
        return FALSE;
    if(IoGetTopLevelIrp() != NULL)
        return FALSE;
    if(KeGetCurrentIrql() != PASSIVE_LEVEL)
        return FALSE;
    if((Data->Iopb->IrpFlags & IRP_PAGING_IO) != 0)
        return FALSE;
    if((communicationUserModePort != NULL) && (PsGetCurrentProcess() == communicationUserProcess))
        return FALSE;
    return TRUE;
}

NTSTATUS DriverEntry(__in PDRIVER_OBJECT DriverObject, __in PUNICODE_STRING RegistryPath)
{
    NTSTATUS                status;
    PSECURITY_DESCRIPTOR    securityDescriptor = NULL;
    OBJECT_ATTRIBUTES       objectAttributes = {0};
    UNICODE_STRING          portName = {0};
    
    UNREFERENCED_PARAMETER(RegistryPath);
    
    DebugBreak();

    KeInitializeMutex(&debugMessageMutex, 0);  
    
    DebugMessage("Initializing driver.");
    
    InitializeProcessing();

    status = FltRegisterFilter(DriverObject, &FilterRegistration, &filterHandle);
    if(!NT_SUCCESS(status))
    {
        DebugMessage("FltRegisterFilter failed with error: %x.", status);
        DebugBreak();
        return status;
    }

    status = FltBuildDefaultSecurityDescriptor(&securityDescriptor, FLT_PORT_ALL_ACCESS);      
    if(!NT_SUCCESS(status))
    {
        FltUnregisterFilter(filterHandle);
        DebugMessage("FltBuildDefaultSecurityDescriptor failed with error: %x.", status);
        DebugBreak();
        return status;
    }    
    
    RtlInitUnicodeString(&portName, COMMUNICATION_PORT_NAME); 
    InitializeObjectAttributes(&objectAttributes, &portName, OBJ_CASE_INSENSITIVE | OBJ_KERNEL_HANDLE, NULL, securityDescriptor);

    status = FltCreateCommunicationPort(filterHandle, &communicationServerPort, &objectAttributes, NULL, UserModeConnectCallback, 
        UserModeDisconnectCallback, ReceiveUserModeMessageCallback, ALLOWED_USERMODE_CONNECTIONS);
    
    FltFreeSecurityDescriptor(securityDescriptor); 
    
    if(!NT_SUCCESS(status))
    {
        FltUnregisterFilter(filterHandle);
        DebugMessage("FltBuildDefaultSecurityDescriptor failed with error: %x.", status);
        DebugBreak();
        return status;
    }

    status = FltStartFiltering(filterHandle);
    if(!NT_SUCCESS(status))
    {    
        DebugMessage("FltStartFiltering failed with error: %x.", status);
        DebugBreak();
        Unload(0);
        return status;
    }
        
    DebugMessage("Driver initialized.");
    return STATUS_SUCCESS;
}

NTSTATUS Unload(__in FLT_FILTER_UNLOAD_FLAGS Flags)
{
    UNREFERENCED_PARAMETER(Flags);

    DebugMessage("Unloading started.");
        
    FltCloseCommunicationPort(communicationServerPort); 
    communicationServerPort = NULL;

    FltUnregisterFilter(filterHandle); 
    
    DebugMessage("Unloading completed.");

    return STATUS_SUCCESS;
}

VOID ContextCleanup(__in PFLT_CONTEXT Context, __in FLT_CONTEXT_TYPE ContextType)
{
    PBASE_CONTEXT baseContext = (PBASE_CONTEXT)Context;

    switch(ContextType) 
    {
    case FLT_STREAMHANDLE_CONTEXT:
    case FLT_STREAM_CONTEXT:
    case FLT_INSTANCE_CONTEXT:
        DebugMessage("Context cleanup. Type: %u.", ContextType);

        if(baseContext->CleanupCallback != NULL)
            baseContext->CleanupCallback(baseContext);
        break;
    }
}

NTSTATUS InstanceSetup(__in PCFLT_RELATED_OBJECTS FltObjects, __in FLT_INSTANCE_SETUP_FLAGS Flags, __in DEVICE_TYPE VolumeDeviceType, __in FLT_FILESYSTEM_TYPE VolumeFilesystemType)
{
    VOLUME_LIST_UPDATE_NOTIFICATION notification;

    UNREFERENCED_PARAMETER(VolumeFilesystemType); 
    
    notification.Header.Protocol = PROTOCOL_VERSION;
    notification.Header.TotalSize = sizeof(notification);
    notification.Header.Type = VOLUME_LIST_UPDATE_NOTIFICATION_TYPE;

    SendMessage(&notification.Header);

    if((Flags & FLTFL_INSTANCE_SETUP_MANUAL_ATTACHMENT) == 0)
        return STATUS_FLT_DO_NOT_ATTACH;

    Attaching(FltObjects);

    return STATUS_SUCCESS;
}

NTSTATUS InstanceQueryTeardown(__in PCFLT_RELATED_OBJECTS FltObjects, __in FLT_INSTANCE_QUERY_TEARDOWN_FLAGS Flags)
{
    Detaching(FltObjects);
 
    return STATUS_SUCCESS;
}

FLT_PREOP_CALLBACK_STATUS CallOperationHandler(BOOLEAN (*handler)(PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS FltObjects), PFLT_CALLBACK_DATA Data, PCFLT_RELATED_OBJECTS FltObjects)
{
    if(!IsProcessingSafe(Data, 0))
        return FLT_PREOP_SUCCESS_NO_CALLBACK;

    if(handler(Data, FltObjects))
    {
        Data->IoStatus.Status = STATUS_ACCESS_DENIED;
        Data->IoStatus.Information = 0;  
        return FLT_PREOP_COMPLETE;
    }

    return FLT_PREOP_SUCCESS_NO_CALLBACK;
}

FLT_PREOP_CALLBACK_STATUS BeforeCreateRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext)
{
    UNREFERENCED_PARAMETER(CompletionContext);

    if(!IsProcessingSafe(Data, 0))
        return FLT_PREOP_SUCCESS_NO_CALLBACK;

    if(BeforeCreate(Data, FltObjects, CompletionContext))
    {
        Data->IoStatus.Status = STATUS_ACCESS_DENIED;
        Data->IoStatus.Information = 0;  
        return FLT_PREOP_COMPLETE;
    }

    return FLT_PREOP_SUCCESS_WITH_CALLBACK;
}

FLT_POSTOP_CALLBACK_STATUS AfterCreateRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __in_opt PVOID CompletionContext, __in FLT_POST_OPERATION_FLAGS Flags)
{    
    UNREFERENCED_PARAMETER(FltObjects);
    UNREFERENCED_PARAMETER(CompletionContext);
    UNREFERENCED_PARAMETER(Flags);
    
    if(!IsProcessingSafe(Data, Flags))
        return FLT_POSTOP_FINISHED_PROCESSING;
    
    AfterCreate(Data, FltObjects, CompletionContext);

    return FLT_POSTOP_FINISHED_PROCESSING;
}

FLT_PREOP_CALLBACK_STATUS BeforeReadRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext)
{
    UNREFERENCED_PARAMETER(CompletionContext);
    
    if(!IsProcessingSafe(Data, 0))
        return FLT_PREOP_SUCCESS_NO_CALLBACK;

    return CallOperationHandler(BeforeRead, Data, FltObjects);
}

FLT_PREOP_CALLBACK_STATUS BeforeWriteRequest(__inout PFLT_CALLBACK_DATA Data, __in PCFLT_RELATED_OBJECTS FltObjects, __deref_out_opt PVOID *CompletionContext)
{
    UNREFERENCED_PARAMETER(CompletionContext);
    
    return CallOperationHandler(BeforeWrite, Data, FltObjects);
}

NTSTATUS UserModeConnect(__in PFLT_PORT ClientPort, __in_opt PVOID ServerPortCookie, __in_bcount_opt(SizeOfContext) PVOID ConnectionContext, __in ULONG SizeOfContext, __deref_out_opt PVOID *ConnectionCookie)
{
    UNREFERENCED_PARAMETER(ServerPortCookie);
    UNREFERENCED_PARAMETER(ConnectionContext);
    UNREFERENCED_PARAMETER(SizeOfContext);
    UNREFERENCED_PARAMETER(ConnectionCookie);

    DebugMessage("User mode application connected.");

    communicationUserModePort = ClientPort;
    communicationUserProcess = PsGetCurrentProcess();
    return STATUS_SUCCESS;
}

VOID UserModeDisconnect(__in_opt PVOID ConnectionCookie)
{
    UNREFERENCED_PARAMETER(ConnectionCookie);
    
    DebugMessage("User mode application disconnected.");

    FltCloseClientPort(filterHandle, &communicationUserModePort);
    communicationUserModePort = NULL;
    communicationUserProcess = NULL;
}

NTSTATUS ReceiveUserModeMessage(IN PVOID PortCookie, IN PVOID InputBuffer OPTIONAL, IN ULONG InputBufferLength, OUT PVOID OutputBuffer OPTIONAL, IN ULONG OutputBufferLength, OUT PULONG ReturnOutputBufferLength)
{
    PMESSAGE_HEADER header = (PMESSAGE_HEADER)InputBuffer;
    
    DebugMessage("Received user mode message. Size = %u.", InputBufferLength);

    if(InputBufferLength < sizeof(MESSAGE_HEADER))
    {
        DebugMessage("Too short message.");
        DebugBreak();
        return STATUS_SUCCESS;
    }

    DebugMessage("User mode message type = %u.", header->Type);

    if(InputBufferLength != header->TotalSize)
    {
        DebugMessage("Incorrect size.");
        DebugBreak();
        return STATUS_SUCCESS;
    }
    
    if(header->Protocol != PROTOCOL_VERSION)
    {
        DebugMessage("Incorrect version.");
        DebugBreak();
        return STATUS_SUCCESS;
    }

    MessageReceived(filterHandle, header);
    
    DebugMessage("User mode message was successfully handled.");
    return STATUS_SUCCESS;
}

VOID SendMessage(PMESSAGE_HEADER message)
{
    LARGE_INTEGER   timeout;    
    NTSTATUS        status;
    
    if(message == NULL)
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    timeout.QuadPart = PROCESSING_TIMEOUT;
    
    DebugMessage("Sending message. Type = %u, Size = %u.", message->Type, message->TotalSize);

    status = FltSendMessage(filterHandle, &communicationUserModePort, message, message->TotalSize, NULL, NULL, &timeout);
    if(!NT_SUCCESS(status))
    {
        DebugMessage("FltSendMessage failed with error %x.", status);
        if(status != STATUS_PORT_DISCONNECTED)
            DebugBreak();
    }
    else if(status == STATUS_TIMEOUT)
    {
        DebugMessage("FltSendMessage timed out.");
        DebugBreak();
    }
    else
        DebugMessage("Message was successfully sent.");
}

VOID UseMemory(LONG size, VOID (*processingCallback)(PVOID memory, LONG size, PVOID parameter), PVOID parameter)
{
    PVOID memory;

    if((size == 0) || (processingCallback == NULL))
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    memory = ExAllocatePoolWithTag(NonPagedPool, size, 'Fbld');
    if(memory == NULL)
    {
        DebugMessage("Memory allocation error.");
        DebugBreak();
    }
    else
    {
        RtlZeroMemory(memory, size);
        processingCallback(memory, size, parameter);    
        ExFreePoolWithTag(memory, 'Fbld');
    }
}


