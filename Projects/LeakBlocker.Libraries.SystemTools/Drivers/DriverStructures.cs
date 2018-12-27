using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeakBlocker.Libraries.Common;

// ReSharper disable InconsistentNaming
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace LeakBlocker.Libraries.SystemTools.Drivers
{
    internal static class DriverStructures
    {
        internal const int VOLUME_IDENTIFIER_NOTIFICATION_TYPE = 9987412;
        
        internal const int VOLUME_DETACH_NOTIFICATION_TYPE = 685396;
        
        internal const int CONFIGURATION_MESSAGE_TYPE = 752271;

        internal const int USER_IDENTIFIER_LENGTH = (184 + 1);

        internal const int FILE_ACCESS_NOTIFICATION_TYPE = 64327;
        
        internal const int VOLUME_LIST_UPDATE_NOTIFICATION_TYPE = 879286;

        internal const int PROTOCOL_VERSION = 2;

        internal const int ACTION_BLOCK = 3;
        internal const int ACTION_ALLOW = 0;
        internal const int ACTION_READONLY = 1;

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct MESSAGE_HEADER
        {
            internal int Protocol;
            internal int TotalSize;
            internal int Type;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct FILE_NAME
        {
            internal int TotalSize;

            internal int VolumeNameLength;
            internal int StringLength;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct VOLUME_NAME
        {
            internal int TotalSize;

            internal int StringLength;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct USER_IDENTIFIER
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = USER_IDENTIFIER_LENGTH)]
            internal char[] Value;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct FILE_ACCESS_NOTIFICATION
        {
            internal MESSAGE_HEADER Header;

            internal int Directory;
            internal int Delete;
            public int Read;
            public int Write;

            internal int AppliedAction;
            internal int ProcessIdentifier;

            internal long SystemTime;

            internal USER_IDENTIFIER User;
            internal FILE_NAME FileName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct VOLUME_IDENTIFIER_NOTIFICATION
        {
            internal MESSAGE_HEADER Header;

            internal long InstanceIdentifier;
            internal VOLUME_NAME VolumeName;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct VOLUME_DETACH_NOTIFICATION
        {
            internal MESSAGE_HEADER Header;

            internal long InstanceIdentifier;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct CONFIGURATION_ENTRY
        {
            internal int Action;
            internal USER_IDENTIFIER User;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct CONFIGURATION_MESSAGE
        {
            internal MESSAGE_HEADER Header;

            internal long InstanceIdentifier;
            internal int EntriesCount;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
        internal struct VOLUME_LIST_UPDATE_NOTIFICATION
        {
            internal MESSAGE_HEADER Header;
        }
    }
}

// ReSharper restore InconsistentNaming
// ReSharper restore FieldCanBeMadeReadOnly.Global
// ReSharper restore MemberCanBePrivate.Global