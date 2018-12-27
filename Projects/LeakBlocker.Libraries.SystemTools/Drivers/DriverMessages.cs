using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Drivers
{
    internal interface IOutgoingMessage
    {
        byte[] GetBinaryData();
    }

    internal sealed class ConfigurationMessage : IOutgoingMessage
    {
        private readonly ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules;
        private readonly long instanceIdentifier;

        internal ConfigurationMessage(long instanceIdentifier, ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules)
        {
            Check.ObjectIsNotNull(rules, "rules");

            this.instanceIdentifier = instanceIdentifier;
            this.rules = rules.ToReadOnlyDictionary();
        }

        public byte[] GetBinaryData()
        {
            var size = (int)(UnmanagedStructure<DriverStructures.CONFIGURATION_MESSAGE>.GetSize() +
                rules.Count * UnmanagedStructure<DriverStructures.CONFIGURATION_ENTRY>.GetSize());

            using (var memory = new UnmanagedMemory(size))
            using (var structure = new UnmanagedStructure<DriverStructures.CONFIGURATION_MESSAGE>(+memory))
            using (var entriesArray = new UnmanagedArray<DriverStructures.CONFIGURATION_ENTRY>(+memory + structure.SSize, rules.Count))
            {
                structure.Value = new DriverStructures.CONFIGURATION_MESSAGE
                {
                    InstanceIdentifier = instanceIdentifier,
                    EntriesCount = rules.Count,
                    Header = new DriverStructures.MESSAGE_HEADER
                    {
                        Protocol = DriverStructures.PROTOCOL_VERSION,
                        TotalSize = memory.SSize,
                        Type = DriverStructures.CONFIGURATION_MESSAGE_TYPE
                    }
                };

                entriesArray.Value = rules.Select(delegate(KeyValuePair<AccountSecurityIdentifier, FileAccessType> rule)
                {
                    var userIdentifier = new char[DriverStructures.USER_IDENTIFIER_LENGTH];
                    var identifier = rule.Key.Value;
                    identifier.CopyTo(0, userIdentifier, 0, identifier.Length);

                    return new DriverStructures.CONFIGURATION_ENTRY
                    {
                        Action = (int)rule.Value,
                        User = new DriverStructures.USER_IDENTIFIER
                        {
                            Value = userIdentifier
                        }
                    };
                }).ToArray();

                return memory.BinaryData;
            }
        }
    }

    internal sealed class VolumeAttachMessage : IVolumeAttachMessage
    {
        public long InstanceIdentifier
        {
            get;
            private set;
        }

        public VolumeName Name
        {
            get;
            private set;
        }

        internal VolumeAttachMessage(UnmanagedMemory memory)
        {
            Check.ObjectIsNotNull(memory, "memory");

            if (memory.SSize <= UnmanagedStructure<DriverStructures.VOLUME_IDENTIFIER_NOTIFICATION>.GetSize())
                Exceptions.Throw(ErrorMessage.InsufficientBuffer);

            using (var structure = new UnmanagedStructure<DriverStructures.VOLUME_IDENTIFIER_NOTIFICATION>(+memory))
            {
                Name = new VolumeName(structure);
                InstanceIdentifier = structure.Value.InstanceIdentifier;
            }
        }
    }
    
    internal sealed class VolumeDetachMessage : IVolumeDetachMessage
    {
        public long InstanceIdentifier
        {
            get;
            private set;
        }

        internal VolumeDetachMessage(UnmanagedMemory memory)
        {
            Check.ObjectIsNotNull(memory, "memory");

            if (memory.SSize <= UnmanagedStructure<DriverStructures.VOLUME_DETACH_NOTIFICATION>.GetSize())
                Exceptions.Throw(ErrorMessage.InsufficientBuffer);

            using (var structure = new UnmanagedStructure<DriverStructures.VOLUME_DETACH_NOTIFICATION>(+memory))
            {
                InstanceIdentifier = structure.Value.InstanceIdentifier;
            }
        }
    }

    internal sealed class FileNotificationMessage : IFileNotificationMessage
    {
        public string FileName
        {
            get;
            private set;
        }

        public VolumeName Volume
        {
            get;
            private set;
        }

        public AccountSecurityIdentifier UserIdentifier
        {
            get;
            private set;
        }

        public bool Read
        {
            get;
            private set;
        }

        public bool Write
        {
            get;
            private set;
        }

        public bool Delete
        {
            get;
            private set;
        }

        public bool Directory
        {
            get;
            private set;
        }

        public Time SystemTime
        {
            get;
            private set;
        }

        public FileAccessType AppliedAction
        {
            get;
            private set;
        }

        public string ProcessName
        {
            get;
            private set;
        }

        public FileActionType ResultAction
        {
            get;
            private set;
        }

        internal FileNotificationMessage(UnmanagedMemory memory)
        {
            Check.ObjectIsNotNull(memory, "memory");

            if (memory.SSize <= UnmanagedStructure<DriverStructures.FILE_ACCESS_NOTIFICATION>.GetSize())
                Exceptions.Throw(ErrorMessage.InsufficientBuffer);

            using (var structure = new UnmanagedStructure<DriverStructures.FILE_ACCESS_NOTIFICATION>(+memory))
            using (var nameString = new UnmanagedUnicodeString(+structure + structure.SSize, structure.Value.FileName.StringLength))
            {
                FileName = nameString.Value;
                Delete = (structure.Value.Delete != 0);
                Read = (structure.Value.Read != 0);
                Write = (structure.Value.Write != 0);
                Directory = (structure.Value.Directory != 0);
                UserIdentifier =new AccountSecurityIdentifier( new string(structure.Value.User.Value).Trim());
                SystemTime = new Time(structure.Value.SystemTime);
                AppliedAction = (FileAccessType)structure.Value.AppliedAction;
                Volume = new VolumeName(structure);

                try
                {
                    ProcessName = Process.GetProcessById(structure.Value.ProcessIdentifier).ProcessName;
                }
                catch (Exception exception)
                {
                    ProcessName = string.Empty;
                    Log.Write(exception);
                }
            }

            InitializeResultAction();
        }

        private void InitializeResultAction()
        {
            bool read = Read;
            bool write = Write || Delete;

            bool readBlocked = (AppliedAction == FileAccessType.Block);
            bool writeBlocked = (AppliedAction == FileAccessType.Block) || (AppliedAction == FileAccessType.ReadOnly);

            if (read && !write && readBlocked)
                ResultAction = FileActionType.ReadBlocked;
            else if (read && !write)
                ResultAction = FileActionType.ReadAllowed;
            else if (!read && write && writeBlocked)
                ResultAction = FileActionType.WriteBlocked;
            else if (!read && write)
                ResultAction = FileActionType.WriteAllowed;
            else if (read && writeBlocked && readBlocked)
                ResultAction = FileActionType.ReadWriteBlocked;
            else if (read && !writeBlocked && !readBlocked)
                ResultAction = FileActionType.ReadWriteAllowed;
            else if (read && writeBlocked)
                ResultAction = FileActionType.WriteBlocked;
            else if (read)
                ResultAction = FileActionType.ReadBlocked;
            else
                ResultAction = FileActionType.Unknown;
        }
    }
}
