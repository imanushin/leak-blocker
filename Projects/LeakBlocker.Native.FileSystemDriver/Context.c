#include "Common.h"

VOID SetContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, FLT_CONTEXT_TYPE type, LONG size, VOID (*initializer)(PUCHAR context, PVOID parameter), PVOID parameter);
VOID QueryContext(PFLT_INSTANCE instance, PFILE_OBJECT file, FLT_CONTEXT_TYPE type, PVOID processingCallback, PVOID parameter);



VOID InstanceContextInitializer(PUCHAR context, PVOID parameter)
{
    RtlCopyMemory(context, parameter, ((PCONFIGURATION_MESSAGE)parameter)->Header.TotalSize);
}

VOID CallInstanceContextHandler(PUCHAR context, VOID (*processingCallback)(INSTANCE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    processingCallback((PCONFIGURATION_MESSAGE)context, parameter);
}

VOID SetInstanceContext(PFLT_FILTER filter, PFLT_INSTANCE instance, INSTANCE_CONTEXT_PARAMETERS)
{
    SetContext(filter, instance, NULL, FLT_INSTANCE_CONTEXT, instanceConfiguration->Header.TotalSize, InstanceContextInitializer, instanceConfiguration);
}

VOID QueryInstanceContext(PFLT_INSTANCE instance, VOID (*processingCallback)(INSTANCE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    QueryContext(instance, NULL, FLT_INSTANCE_CONTEXT, processingCallback, parameter);
}



VOID StreamHandleContextInitializer(PUCHAR context, PVOID parameter)
{
    RtlCopyMemory(context, parameter, ((PFILE_NAME)parameter)->TotalSize);
}

VOID CallStreamHandleContextHandler(PUCHAR context, VOID (*processingCallback)(STREAM_HANDLE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    processingCallback((PFILE_NAME)context, parameter);
}

VOID SetStreamHandleContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, STREAM_HANDLE_CONTEXT_PARAMETERS)
{
    SetContext(filter, instance, file, FLT_STREAMHANDLE_CONTEXT, fileName->TotalSize, StreamHandleContextInitializer, fileName);
}

VOID QueryStreamHandleContext(PFLT_INSTANCE instance, PFILE_OBJECT file, VOID (*processingCallback)(STREAM_HANDLE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    QueryContext(instance, file, FLT_STREAMHANDLE_CONTEXT, processingCallback, parameter);
}



VOID StreamContextInitializer(PUCHAR context, PVOID parameter)
{
    RtlCopyMemory(context, parameter, sizeof(LONG));
}

VOID CallStreamContextHandler(PUCHAR context, VOID (*processingCallback)(STREAM_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    processingCallback(*(PLONG)context, parameter);
}

VOID SetStreamContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, STREAM_CONTEXT_PARAMETERS)
{
    SetContext(filter, instance, file, FLT_STREAM_CONTEXT, sizeof(action), StreamContextInitializer, &action);
}

VOID QueryStreamContext(PFLT_INSTANCE instance, PFILE_OBJECT file, VOID (*processingCallback)(STREAM_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter)
{
    QueryContext(instance, file, FLT_STREAM_CONTEXT, processingCallback, parameter);
}



VOID SetContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, FLT_CONTEXT_TYPE type, LONG size, VOID (*initializer)(PUCHAR context, PVOID parameter), PVOID parameter)
{
    PFLT_CONTEXT    Context = NULL;
    NTSTATUS        Status;
    BASE_CONTEXT    BaseContext;
    PUCHAR          ContextData;
    
    if((filter == NULL) || (instance == NULL) || ((file == NULL) && (type != FLT_INSTANCE_CONTEXT)) || (size <= 0) || (initializer == NULL) || (parameter == NULL))
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    Status = FltAllocateContext(filter, type, size + sizeof(BASE_CONTEXT), NonPagedPool, &Context);
    if (!NT_SUCCESS(Status)) 
    {
        DebugMessage("FltAllocateContext failed with error %x.", Status);
        DebugBreak();
        return;
    }
    
    RtlZeroMemory(&BaseContext, sizeof(BASE_CONTEXT));
    RtlZeroMemory(Context, size + sizeof(BASE_CONTEXT));

    BaseContext.CleanupCallback = NULL;

    RtlCopyMemory(Context, &BaseContext, sizeof(BASE_CONTEXT));

    ContextData = (PUCHAR)Context + sizeof(BASE_CONTEXT);

    initializer(ContextData, parameter);

    switch(type)
    {
    case FLT_INSTANCE_CONTEXT:
        Status = FltSetInstanceContext(instance, FLT_SET_CONTEXT_REPLACE_IF_EXISTS, Context, NULL);
        if(!NT_SUCCESS(Status))    
        {
            DebugMessage("FltSetInstanceContext failed with error %x.", Status);
            DebugBreak();
        }
        else
            DebugMessage("Instance context was set.");
        break;

    case FLT_STREAMHANDLE_CONTEXT:
        Status = FltSetStreamHandleContext(instance, file, FLT_SET_CONTEXT_REPLACE_IF_EXISTS, Context, NULL);
        if(!NT_SUCCESS(Status))          
        {
            DebugMessage("FltSetStreamHandleContext failed with error %x.", Status);
            if(Status != STATUS_NOT_SUPPORTED)
                DebugBreak();
        }
        else
            DebugMessage("Stream handle context was set.");
        break;

    case FLT_STREAM_CONTEXT:
        Status = FltSetStreamContext(instance, file, FLT_SET_CONTEXT_KEEP_IF_EXISTS, Context, NULL);
        if(!NT_SUCCESS(Status) && (Status != STATUS_FLT_CONTEXT_ALREADY_DEFINED)) 
        {
            DebugMessage("FltSetStreamContext failed with error %x.", Status);
            if(Status != STATUS_NOT_SUPPORTED)
                DebugBreak();
        }
        else
            DebugMessage("Stream context was set.");
        break;
    }

    FltReleaseContext(Context);
}

VOID QueryContext(PFLT_INSTANCE instance, PFILE_OBJECT file, FLT_CONTEXT_TYPE type, PVOID processingCallback, PVOID parameter)
{
    NTSTATUS                        Status;
    PFLT_CONTEXT                    Context = NULL;
    PUCHAR                          ContextData = NULL;

    if((instance == NULL) || ((file == NULL) && (type != FLT_INSTANCE_CONTEXT)) || (processingCallback == NULL))
    {
        DebugMessage("Wrong parameters.");
        DebugBreak();
        return;
    }

    switch(type)
    {
    case FLT_INSTANCE_CONTEXT:
        Status = FltGetInstanceContext(instance, &Context);
        if(!NT_SUCCESS(Status))
        {
            DebugMessage("FltGetInstanceContext failed with error %x.", Status);
            DebugBreak();
            return;
        }
        break;

    case FLT_STREAMHANDLE_CONTEXT:
        Status = FltGetStreamHandleContext(instance, file, &Context);
        if(!NT_SUCCESS(Status))
        {
            DebugMessage("FltGetStreamHandleContext failed with error %x.", Status);
            DebugBreak();
            return;
        }
        break;

    case FLT_STREAM_CONTEXT:
        Status = FltGetStreamContext(instance, file, &Context);
        if(!NT_SUCCESS(Status))
        {
            DebugMessage("FltGetStreamContext failed with error %x.", Status);
            DebugBreak();
            return;
        }
        break;

    default:
        DebugMessage("Unknown context type.");
        return;
    }

    if(Context == NULL)
        return;
    
    ContextData = (PUCHAR)Context + sizeof(BASE_CONTEXT);
    
    switch(type)
    {
    case FLT_INSTANCE_CONTEXT:
        CallInstanceContextHandler(ContextData, (VOID (*)(INSTANCE_CONTEXT_PARAMETERS, PVOID parameter))processingCallback, parameter);
        break;
    case FLT_STREAM_CONTEXT:
        CallStreamContextHandler(ContextData, (VOID (*)(STREAM_CONTEXT_PARAMETERS, PVOID parameter))processingCallback, parameter);
        break;
    case FLT_STREAMHANDLE_CONTEXT:
        CallStreamHandleContextHandler(ContextData, (VOID (*)(STREAM_HANDLE_CONTEXT_PARAMETERS, PVOID parameter))processingCallback, parameter);
        break;
    }

    FltReleaseContext(Context);
}

