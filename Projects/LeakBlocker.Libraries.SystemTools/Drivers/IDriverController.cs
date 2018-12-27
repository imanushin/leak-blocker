using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;

namespace LeakBlocker.Libraries.SystemTools.Drivers
{
    /// <summary>
    /// File access configuration type.
    /// </summary>
    public enum FileAccessType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None = 0,

        /// <summary>
        /// Allow any access.
        /// </summary>
        Allow = DriverStructures.ACTION_ALLOW,

        /// <summary>
        /// Allow reaad access and deny write access.
        /// </summary>
        ReadOnly = DriverStructures.ACTION_READONLY,

        /// <summary>
        /// Deny any access.
        /// </summary>
        Block = DriverStructures.ACTION_BLOCK
    }

    /// <summary>
    /// Action that was applied to the file.
    /// </summary>
    public enum FileActionType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None = 0,

        /// <summary>
        /// Unknown action.
        /// </summary>
        Unknown,

        /// <summary>
        /// Read was allowed.
        /// </summary>
        ReadAllowed,

        /// <summary>
        /// Write was allowed.
        /// </summary>
        WriteAllowed,

        /// <summary>
        /// Both read and write were allowed.
        /// </summary>
        ReadWriteAllowed,

        /// <summary>
        /// Read was blocked.
        /// </summary>
        ReadBlocked,

        /// <summary>
        /// Write was blocked.
        /// </summary>
        WriteBlocked,

        /// <summary>
        /// Both read and write were blocked.
        /// </summary>
        ReadWriteBlocked
    }

    /// <summary>
    /// Volume attached message.
    /// </summary>
    public interface IVolumeAttachMessage
    {
        /// <summary>
        /// Volume identifier.
        /// </summary>
        long InstanceIdentifier
        {
            get;
        }

        /// <summary>
        /// Volume name.
        /// </summary>
        VolumeName Name
        {
            get;
        }
    }

    /// <summary>
    /// Volume detached event data.
    /// </summary>
    public interface IVolumeDetachMessage
    {
        /// <summary>
        /// Volume identifier. This value is no longer valid and all its usages should be removed.
        /// </summary>
        long InstanceIdentifier
        {
            get;
        }
    }

    /// <summary>
    /// File access event data.
    /// </summary>
    public interface IFileNotificationMessage
    {
        /// <summary>
        /// File name.
        /// </summary>
        string FileName
        {
            get;
        }
        
        /// <summary>
        /// Volume name.
        /// </summary>
        VolumeName Volume
        {
            get;
        }

        /// <summary>
        /// User that accessed the file.
        /// </summary>
        AccountSecurityIdentifier UserIdentifier
        {
            get;
        }

        /// <summary>
        /// True if operation is read operation.
        /// </summary>
        bool Read
        {
            get;
        }

        /// <summary>
        /// True if operation is write operation.
        /// </summary>
        bool Write
        {
            get;
        }

        /// <summary>
        /// True if operation is delete operation.
        /// </summary>
        bool Delete
        {
            get;
        }

        /// <summary>
        /// True if target object is directory.
        /// </summary>
        bool Directory
        {
            get;
        }

        /// <summary>
        /// Time when the file was accessed.
        /// </summary>
        Time SystemTime
        {
            get;
        }

        /// <summary>
        /// Action that should be applied to the current operation.
        /// </summary>
        FileAccessType AppliedAction
        {
            get;
        }

        /// <summary>
        /// Process that aaccessed the file.
        /// </summary>
        string ProcessName
        {
            get;
        }

        /// <summary>
        /// Action that actually was applied to the current operation.
        /// </summary>
        FileActionType ResultAction
        {
            get;
        }
    }

    /// <summary>
    /// Driver event handler.
    /// </summary>
    public interface IDriverControllerHandler
    {
        /// <summary>
        /// Driver was attached to a new volume.
        /// </summary>
        /// <param name="message">Event data.</param>
        void VolumeAttachMessageReceived(IVolumeAttachMessage message);

        /// <summary>
        /// Driver was removed from the volume.
        /// </summary>
        /// <param name="message">Event data.</param>
        void VolumeDetachMessageReceived(IVolumeDetachMessage message);

        /// <summary>
        /// File access event.
        /// </summary>
        /// <param name="message">Event data.</param>
        void FileNotificationMessageReceived(IFileNotificationMessage message);

        /// <summary>
        /// VolumeList update event.
        /// </summary>
        void VolumeListUpdateMessageReceived();
    }

    /// <summary>
    /// Class for controlling filesystem driver.
    /// </summary>
    public interface IDriverController : IDisposable
    {
        /// <summary>
        /// Checks if the driver is running and can be controlled.
        /// </summary>
        bool Available
        {
            get;
        }

        /// <summary>
        /// Attaches the driver to new volumes and detaches from volumes that are not present in the specified list.
        /// </summary>
        /// <param name="volumes">NT volume names.</param>
        void SetManagedVolumes(IReadOnlyCollection<VolumeName> volumes);

        /// <summary>
        /// Updated driver instance configuration.
        /// </summary>
        /// <param name="instanceIdentifier">Instance identifier that was earlier received from the driver.</param>
        /// <param name="rules">Configuration.</param>
        void SetInstanceConfiguration(long instanceIdentifier, ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules);

        /// <summary>
        /// Installs the driver.
        /// </summary>
        void Install();

        /// <summary>
        /// Removes the driver.
        /// </summary>
        void Remove();
    }
}
