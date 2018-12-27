using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Libraries.Common.Entities
{
    /// <summary>
    /// Device class.
    /// </summary>
    [EnumerationDescriptionProvider(typeof(DeviceCategoryStrings))]
    public enum DeviceCategory
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None,

        /// <summary>
        /// Modems, network adapters, bluetooth devices.
        /// </summary>
        Network,

        /// <summary>
        /// CD-ROMs.
        /// </summary>
        OpticalDrive,

        /// <summary>
        /// Disk drives, floppy drives e.t.c.
        /// </summary>
        Storage,

        /// <summary>
        /// Printers.
        /// </summary>
        Printer,
        
        /// <summary>
        /// Smartphones, tablet computers and other multifunctional computing devices.
        /// </summary>
        Mobile,

        /// <summary>
        /// Scanners, cameras e.t.c.
        /// </summary>
        ImageAcquisition,
        
        /// <summary>
        /// Mice, keyboards e.t.c.
        /// </summary>
        UserInput,

        /// <summary>
        /// Generic USB devices.
        /// </summary>
        UniversalSerialBus,

        /// <summary>
        /// Generic IEEE1394 devices.
        /// </summary>
        FireWire,

        /// <summary>
        /// Device cannot be used for stealing data and hence is not supported by Leak Blocker.
        /// </summary>
        Unsupported,

        /// <summary>
        /// Device type is unknown. 
        /// </summary>
        Unknown,

        /// <summary>
        /// Device just an interface between endpoint device and system.
        /// </summary>
        Controller
    }
}