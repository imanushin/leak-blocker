#include <fltkernel.h>
#include "../../CommonProperties/version.h"

#pragma once

#pragma pack(push, 1)

typedef struct
{
    LONG TotalSize;

    LONG VolumeNameLength;
    LONG StringLength;
} FILE_NAME, *PFILE_NAME;

typedef struct
{
    LONG TotalSize;

    LONG StringLength;
} VOLUME_NAME, *PVOLUME_NAME;

typedef struct
{
    VOID (*CleanupCallback)(PVOID context);
} BASE_CONTEXT, *PBASE_CONTEXT;

#define USER_IDENTIFIER_LENGTH (184 + 1)

typedef struct
{
    WCHAR Value[USER_IDENTIFIER_LENGTH];
} USER_IDENTIFIER, *PUSER_IDENTIFIER;

#define ACTION_BLOCK    3
#define ACTION_ALLOW    0
#define ACTION_READONLY 1 

#define PROTOCOL_VERSION    2

typedef struct 
{
    LONG Protocol;
    LONG TotalSize;
    LONG Type;
} MESSAGE_HEADER, *PMESSAGE_HEADER;

#define FILE_ACCESS_NOTIFICATION_TYPE 64327

typedef struct
{
    MESSAGE_HEADER Header;
        
    LONG Directory;
    LONG Delete;
    LONG Read;
    LONG Write;
    
    LONG AppliedAction;
    LONG ProcessIdentifier;

    LONGLONG SystemTime;  

    USER_IDENTIFIER User;
    FILE_NAME       FileName;
} FILE_ACCESS_NOTIFICATION, *PFILE_ACCESS_NOTIFICATION;

#define VOLUME_IDENTIFIER_NOTIFICATION_TYPE 9987412

typedef struct
{
    MESSAGE_HEADER Header;
       
    LONGLONG       InstanceIdentifier;
    VOLUME_NAME    VolumeName;
} VOLUME_IDENTIFIER_NOTIFICATION, *PVOLUME_IDENTIFIER_NOTIFICATION;

#define VOLUME_DETACH_NOTIFICATION_TYPE 685396

typedef struct
{
    MESSAGE_HEADER Header;
} VOLUME_LIST_UPDATE_NOTIFICATION, *PVOLUME_LIST_UPDATE_NOTIFICATION;

#define VOLUME_LIST_UPDATE_NOTIFICATION_TYPE 879286

typedef struct
{
    MESSAGE_HEADER Header;
       
    LONGLONG       InstanceIdentifier;
} VOLUME_DETACH_NOTIFICATION, *PVOLUME_DETACH_NOTIFICATION;

#define CONFIGURATION_MESSAGE_TYPE 752271

typedef struct 
{
    LONG            Action;
    USER_IDENTIFIER User;
} CONFIGURATION_ENTRY, *PCONFIGURATION_ENTRY;

typedef struct 
{
    MESSAGE_HEADER Header;

    LONGLONG    InstanceIdentifier;
    LONG        EntriesCount;
} CONFIGURATION_MESSAGE, *PCONFIGURATION_MESSAGE;

#pragma pack(pop)

VOID SendMessage(PMESSAGE_HEADER message);
VOID UseMemory(LONG size, VOID (*processingCallback)(PVOID memory, LONG size, PVOID parameter), PVOID parameter);

VOID    InitializeProcessing();
VOID    Attaching(PCFLT_RELATED_OBJECTS relatedObjects);
VOID    Detaching(PCFLT_RELATED_OBJECTS relatedObjects);
BOOLEAN BeforeRead(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects);
BOOLEAN BeforeWrite(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects);
BOOLEAN BeforeCreate(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, PVOID *context);
VOID    MessageReceived(PFLT_FILTER filter, PMESSAGE_HEADER message);
VOID    AfterCreate(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, PVOID context);

#define LPCSTR2LPCWSTR(x) L ## x
#define AS_UNICODE(x) LPCSTR2LPCWSTR(x)

#define COMMUNICATION_PORT_NAME         L"\\LeakBlockerFsDrv" AS_UNICODE(FILENAME_VERSION_PART)
#define ALLOWED_USERMODE_CONNECTIONS    1
#define PROCESSING_TIMEOUT              (-1000 * 1000 * 10)

#ifdef _DEBUG
#define DBG_OUTPUT
#endif

#ifdef DBG_OUTPUT

extern KMUTEX debugMessageMutex;

#define DebugBreak() KdBreakPoint()

#define DebugMessage(Format, ...)                                                                                           \
    do                                                                                                                      \
    {                                                                                                                       \
        KeWaitForMutexObject(&debugMessageMutex, Executive, KernelMode, FALSE, NULL);                                       \
        DbgPrint(">>>>> %p %u %s:%u %s >>> ", KeGetCurrentThread(), KeGetCurrentIrql(), __FILE__, __LINE__, __FUNCTION__);  \
        DbgPrint(Format, __VA_ARGS__);                                                                                      \
        DbgPrint("\n");                                                                                                     \
        KeReleaseMutex(&debugMessageMutex, FALSE);                                                                          \
    }                                                                                                                       \
    while(0)

#else
#define DebugBreak()
#define DebugMessage(Format, ...)
#endif

VOID QueryVolumeName(PFLT_VOLUME volume, VOID (*processingCallback)(PVOLUME_NAME volumeName, PVOID parameter), PVOID parameter);
VOID QueryFileName(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects, VOID (*processingCallback)(PFILE_NAME fileName, PVOID parameter), PVOID parameter);
LONG IsDirectory(PFLT_CALLBACK_DATA data, PCFLT_RELATED_OBJECTS relatedObjects);
VOID GetCurrentUserIdentifier(PUSER_IDENTIFIER user);

#define INSTANCE_CONTEXT_PARAMETERS         PCONFIGURATION_MESSAGE instanceConfiguration
#define STREAM_CONTEXT_PARAMETERS           LONG action
#define STREAM_HANDLE_CONTEXT_PARAMETERS    PFILE_NAME fileName

VOID SetInstanceContext(PFLT_FILTER filter, PFLT_INSTANCE instance, INSTANCE_CONTEXT_PARAMETERS);
VOID QueryInstanceContext(PFLT_INSTANCE instance, VOID (*processingCallback)(INSTANCE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter);

VOID SetStreamHandleContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, STREAM_HANDLE_CONTEXT_PARAMETERS);
VOID QueryStreamHandleContext(PFLT_INSTANCE instance, PFILE_OBJECT file, VOID (*processingCallback)(STREAM_HANDLE_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter);

VOID SetStreamContext(PFLT_FILTER filter, PFLT_INSTANCE instance, PFILE_OBJECT file, STREAM_CONTEXT_PARAMETERS);
VOID QueryStreamContext(PFLT_INSTANCE instance, PFILE_OBJECT file, VOID (*processingCallback)(STREAM_CONTEXT_PARAMETERS, PVOID parameter), PVOID parameter);

