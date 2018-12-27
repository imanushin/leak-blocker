using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.Devices
{
    #region DeviceOptions

    /// <summary>
    /// Device capabilities and flags.
    /// </summary>
    [Flags]
    public enum DeviceOptions : long
    {
        /// <summary>
        /// Device has no additional options.
        /// </summary>
        None = 0,

        /// <summary>
        /// ???
        /// </summary>
        RootEnumerated = (long)NativeMethods.DN_ROOT_ENUMERATED,

        /// <summary>
        /// Driver is currently loaded. 
        /// </summary>
        DriverLoaded = (long)NativeMethods.DN_DRIVER_LOADED,

        /// <summary>
        /// ???
        /// </summary>
        EnumLoaded = (long)NativeMethods.DN_ENUM_LOADED,

        /// <summary>
        /// Device is active.
        /// </summary>
        Started = (long)NativeMethods.DN_STARTED,

        /// <summary>
        /// ???
        /// </summary>
        Manual = (long)NativeMethods.DN_MANUAL,

        /// <summary>
        /// ???
        /// </summary>
        NeedToEnum = (long)NativeMethods.DN_NEED_TO_ENUM,

        /// <summary>
        /// ???
        /// </summary>
        NotFirstTime = (long)NativeMethods.DN_NOT_FIRST_TIME,

        /// <summary>
        /// ???
        /// </summary>
        HardwareEnum = (long)NativeMethods.DN_HARDWARE_ENUM,

        /// <summary>
        /// ???
        /// </summary>
        Liar = (long)NativeMethods.DN_LIAR,

        /// <summary>
        /// ???
        /// </summary>
        HasMark = (long)NativeMethods.DN_HAS_MARK,

        /// <summary>
        /// Device is not working. Check error code for details. 
        /// </summary>
        HasProblem = (long)NativeMethods.DN_HAS_PROBLEM,

        /// <summary>
        /// ???
        /// </summary>
        Filtered = (long)NativeMethods.DN_FILTERED,

        /// <summary>
        /// ???
        /// </summary>
        Moved = (long)NativeMethods.DN_MOVED,

        /// <summary>
        /// Device can be disabled.
        /// </summary>
        CanBeDisabled = (long)NativeMethods.DN_DISABLEABLE,

        /// <summary>
        /// Device is removable.
        /// </summary>
        CanBeRemoved = (long)NativeMethods.DN_REMOVABLE,

        /// <summary>
        /// ???
        /// </summary>
        PrivateProblem = (long)NativeMethods.DN_PRIVATE_PROBLEM,

        /// <summary>
        /// ???
        /// </summary>
        MultifunctionParent = (long)NativeMethods.DN_MF_PARENT,

        /// <summary>
        /// ???
        /// </summary>
        MultifunctionChild = (long)NativeMethods.DN_MF_CHILD,

        /// <summary>
        /// ???
        /// </summary>
        WillBeRemoved = (long)NativeMethods.DN_WILL_BE_REMOVED,

        /// <summary>
        /// ???
        /// </summary>
        NotFirstTimeE = (long)NativeMethods.DN_NOT_FIRST_TIMEE,

        /// <summary>
        /// ???
        /// </summary>
        StopFreeRes = (long)NativeMethods.DN_STOP_FREE_RES,

        /// <summary>
        /// ???
        /// </summary>
        RebalanceCandidate = (long)NativeMethods.DN_REBAL_CANDIDATE,

        /// <summary>
        /// ???
        /// </summary>
        BadPartial = (long)NativeMethods.DN_BAD_PARTIAL,

        /// <summary>
        /// ???
        /// </summary>
        NtEnumerator = (long)NativeMethods.DN_NT_ENUMERATOR,

        /// <summary>
        /// ???
        /// </summary>
        NtDriver = (long)NativeMethods.DN_NT_DRIVER,

        /// <summary>
        /// ???
        /// </summary>
        NeedsLocking = (long)NativeMethods.DN_NEEDS_LOCKING,

        /// <summary>
        /// ???
        /// </summary>
        ArmWakeup = (long)NativeMethods.DN_ARM_WAKEUP,

        /// <summary>
        /// ???
        /// </summary>
        AdvancedPowerManagementEnumerator = (long)NativeMethods.DN_APM_ENUMERATOR,

        /// <summary>
        /// ???
        /// </summary>
        AdvancedPowerManagementDriver = (long)NativeMethods.DN_APM_DRIVER,

        /// <summary>
        /// ???
        /// </summary>
        SilentInstall = (long)NativeMethods.DN_SILENT_INSTALL,

        /// <summary>
        /// Device is hidden from device manager.
        /// </summary>
        NoShowInDeviceManager = (long)NativeMethods.DN_NO_SHOW_IN_DM,

        /// <summary>
        /// ???
        /// </summary>
        BootLogProblem = (long)NativeMethods.DN_BOOT_LOG_PROB,

        /// <summary>
        /// ???
        /// </summary>
        DriverBlocked = (long)NativeMethods.DN_NOT_FIRST_TIME,

        /// <summary>
        /// ???
        /// </summary>
        LegacyDriver = (long)NativeMethods.DN_MOVED,

        /// <summary>
        /// ???
        /// </summary>
        ChildWithInvalidIdentifier = (long)NativeMethods.DN_HAS_MARK,

        /// <summary>
        /// System reboot is required.
        /// </summary>
        NeedRestart = (long)NativeMethods.DN_NEED_RESTART,

        /// <summary>
        /// Device is removable.
        /// </summary>
        Removable = (long)NativeMethods.CM_DEVCAP_REMOVABLE << 32,

        /// <summary>
        /// Device has serial number.
        /// </summary>
        UniqueId = (long)NativeMethods.CM_DEVCAP_UNIQUEID << 32,

        /// <summary>
        /// ???
        /// </summary>
        LockSupported = (long)NativeMethods.CM_DEVCAP_LOCKSUPPORTED << 32,

        /// <summary>
        /// Device supports safe removal.
        /// </summary>
        EjectSupported = (long)NativeMethods.CM_DEVCAP_EJECTSUPPORTED << 32,

        /// <summary>
        /// ???
        /// </summary>
        DockDevice = (long)NativeMethods.CM_DEVCAP_DOCKDEVICE << 32,

        /// <summary>
        /// ???
        /// </summary>
        SilentInstall1 = (long)NativeMethods.CM_DEVCAP_SILENTINSTALL << 32,

        /// <summary>
        /// ???
        /// </summary>
        RawDevice = (long)NativeMethods.CM_DEVCAP_RAWDEVICEOK << 32,

        /// <summary>
        /// Device can be normally removed without using safe removal.
        /// </summary>
        SurpriseRemoval = (long)NativeMethods.CM_DEVCAP_SURPRISEREMOVALOK << 32,

        /// <summary>
        /// ???
        /// </summary>
        HardwareDisabled = (long)NativeMethods.CM_DEVCAP_HARDWAREDISABLED << 32,

        /// <summary>
        /// ???
        /// </summary>
        NonDynamic = (long)NativeMethods.CM_DEVCAP_NONDYNAMIC << 32
    }

    #endregion

    #region DeviceError

    /// <summary>
    /// Device status codes.
    /// </summary>
    public enum DeviceError
    {
        /// <summary>
        /// Device has no error.
        /// </summary>
        None = 0,
        
        /// <summary>
        /// ???
        /// </summary>
        NotConfigured = (int)NativeMethods.CM_PROB_NOT_CONFIGURED,

        /// <summary>
        /// ???
        /// </summary>
        DevLoaderFailed = (int)NativeMethods.CM_PROB_DEVLOADER_FAILED,

        /// <summary>
        /// Out of memory.
        /// </summary>
        OutOfMemory = (int)NativeMethods.CM_PROB_OUT_OF_MEMORY,

        /// <summary>
        /// ???
        /// </summary>
        EntryIsWrongType = (int)NativeMethods.CM_PROB_ENTRY_IS_WRONG_TYPE,

        /// <summary>
        /// ???
        /// </summary>
        LackedArbitrator = (int)NativeMethods.CM_PROB_LACKED_ARBITRATOR,

        /// <summary>
        /// ???
        /// </summary>
        BootConfigConflict = (int)NativeMethods.CM_PROB_BOOT_CONFIG_CONFLICT,

        /// <summary>
        /// ???
        /// </summary>
        FailedFilter = (int)NativeMethods.CM_PROB_FAILED_FILTER,

        /// <summary>
        /// ???
        /// </summary>
        DevLoaderNotFound = (int)NativeMethods.CM_PROB_DEVLOADER_NOT_FOUND,

        /// <summary>
        /// ???
        /// </summary>
        InvalidData = (int)NativeMethods.CM_PROB_INVALID_DATA,

        /// <summary>
        /// ???
        /// </summary>
        FailedStart = (int)NativeMethods.CM_PROB_FAILED_START,

        /// <summary>
        /// ???
        /// </summary>
        Liar = (int)NativeMethods.CM_PROB_LIAR,

        /// <summary>
        /// ???
        /// </summary>
        Conflict = (int)NativeMethods.CM_PROB_NORMAL_CONFLICT,

        /// <summary>
        /// ???
        /// </summary>
        NotVerified = (int)NativeMethods.CM_PROB_NOT_VERIFIED,

        /// <summary>
        /// Reboot is required.
        /// </summary>
        NeedRestart = (int)NativeMethods.CM_PROB_NEED_RESTART,

        /// <summary>
        /// ???
        /// </summary>
        Reenumeration = (int)NativeMethods.CM_PROB_REENUMERATION,

        /// <summary>
        /// ???
        /// </summary>
        PartialLogConfiguration = (int)NativeMethods.CM_PROB_PARTIAL_LOG_CONF,

        /// <summary>
        /// ???
        /// </summary>
        UnknownResource = (int)NativeMethods.CM_PROB_UNKNOWN_RESOURCE,

        /// <summary>
        /// ???
        /// </summary>
        Reinstall = (int)NativeMethods.CM_PROB_REINSTALL,

        /// <summary>
        /// ???
        /// </summary>
        Registry = (int)NativeMethods.CM_PROB_REGISTRY,

        /// <summary>
        /// ???
        /// </summary>
        WillBeRemoved = (int)NativeMethods.CM_PROB_WILL_BE_REMOVED,

        /// <summary>
        /// Device is disabled.
        /// </summary>
        Disabled = (int)NativeMethods.CM_PROB_DISABLED,

        /// <summary>
        /// ???
        /// </summary>
        DevLoaderNotReady = (int)NativeMethods.CM_PROB_DEVLOADER_NOT_READY,

        /// <summary>
        /// ???
        /// </summary>
        DeviceNotThere = (int)NativeMethods.CM_PROB_DEVICE_NOT_THERE,

        /// <summary>
        /// ???
        /// </summary>
        Moved = (int)NativeMethods.CM_PROB_MOVED,

        /// <summary>
        /// ???
        /// </summary>
        TooEarly = (int)NativeMethods.CM_PROB_TOO_EARLY,

        /// <summary>
        /// ???
        /// </summary>
        NoValidLogConfiguration = (int)NativeMethods.CM_PROB_NO_VALID_LOG_CONF,

        /// <summary>
        /// ???
        /// </summary>
        FailedInstall = (int)NativeMethods.CM_PROB_FAILED_INSTALL,

        /// <summary>
        /// ???
        /// </summary>
        HardwareDisabled = (int)NativeMethods.CM_PROB_HARDWARE_DISABLED,

        /// <summary>
        /// ???
        /// </summary>
        CantShareIrq = (int)NativeMethods.CM_PROB_CANT_SHARE_IRQ,

        /// <summary>
        /// ???
        /// </summary>
        FailedAdd = (int)NativeMethods.CM_PROB_FAILED_ADD,

        /// <summary>
        /// ???
        /// </summary>
        DisabledService = (int)NativeMethods.CM_PROB_DISABLED_SERVICE,

        /// <summary>
        /// ???
        /// </summary>
        TranslationFailed = (int)NativeMethods.CM_PROB_TRANSLATION_FAILED,

        /// <summary>
        /// ???
        /// </summary>
        NoSoftConfig = (int)NativeMethods.CM_PROB_NO_SOFTCONFIG,

        /// <summary>
        /// ???
        /// </summary>
        BiosTable = (int)NativeMethods.CM_PROB_BIOS_TABLE,

        /// <summary>
        /// ???
        /// </summary>
        IrqTranslationFailed = (int)NativeMethods.CM_PROB_IRQ_TRANSLATION_FAILED,

        /// <summary>
        /// ???
        /// </summary>
        FailedDriverEntry = (int)NativeMethods.CM_PROB_FAILED_DRIVER_ENTRY,

        /// <summary>
        /// ???
        /// </summary>
        FailedPriorUnload = (int)NativeMethods.CM_PROB_DRIVER_FAILED_PRIOR_UNLOAD,

        /// <summary>
        /// Driver cannot be loaded.
        /// </summary>
        FailedLoad = (int)NativeMethods.CM_PROB_DRIVER_FAILED_LOAD,

        /// <summary>
        /// ???
        /// </summary>
        ServiceKeyInvalid = (int)NativeMethods.CM_PROB_DRIVER_SERVICE_KEY_INVALID,

        /// <summary>
        /// ???
        /// </summary>
        LegacyServiceNoDevices = (int)NativeMethods.CM_PROB_LEGACY_SERVICE_NO_DEVICES,

        /// <summary>
        /// ???
        /// </summary>
        DuplicateDevice = (int)NativeMethods.CM_PROB_DUPLICATE_DEVICE,

        /// <summary>
        /// ???
        /// </summary>
        FailedPostStart = (int)NativeMethods.CM_PROB_FAILED_POST_START,

        /// <summary>
        /// ???
        /// </summary>
        Halted = (int)NativeMethods.CM_PROB_HALTED,

        /// <summary>
        /// ???
        /// </summary>
        Phantom = (int)NativeMethods.CM_PROB_PHANTOM,

        /// <summary>
        /// ???
        /// </summary>
        SystemShutdown = (int)NativeMethods.CM_PROB_SYSTEM_SHUTDOWN,

        /// <summary>
        /// ???
        /// </summary>
        HeldForEject = (int)NativeMethods.CM_PROB_HELD_FOR_EJECT,

        /// <summary>
        /// ???
        /// </summary>
        DriverBlocked = (int)NativeMethods.CM_PROB_DRIVER_BLOCKED,

        /// <summary>
        /// ???
        /// </summary>
        RegistryTooLarge = (int)NativeMethods.CM_PROB_REGISTRY_TOO_LARGE,

        /// <summary>
        /// ???
        /// </summary>
        SetPropertiesFailed = (int)NativeMethods.CM_PROB_SETPROPERTIES_FAILED,

        /// <summary>
        /// ???
        /// </summary>
        WaitingOnDependency = (int)NativeMethods.CM_PROB_WAITING_ON_DEPENDENCY,

        /// <summary>
        /// Device has unsigned driver.
        /// </summary>
        UnsignedDriver = (int)NativeMethods.CM_PROB_UNSIGNED_DRIVER,
    }

    #endregion

    #region SystemDeviceType

    /// <summary>
    /// Device class.
    /// </summary>
    public enum SystemDeviceType
    {
        /// <summary>
        /// Incorrect value.
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None,

        /// <summary>
        /// Class is not recognized by the current module.
        /// </summary>
        Unsupported,

        /// <summary>
        /// This class includes all Bluetooth devices.
        /// </summary>
        Bluetooth,

        /// <summary>
        /// This class includes CD-ROM drives, including SCSI CD-ROM drives. By default, the system's CD-ROM class installer also installs a system-supplied CD audio driver and CD-ROM changer driver as Plug and Play filters. 
        /// </summary>
        OpticalDrive,

        /// <summary>
        /// This class includes hard disk drives.   
        /// </summary>
        DiskDrive,

        /// <summary>
        /// This class includes floppy disk drives.
        /// </summary>
        FloppyDisk,

        /// <summary>
        /// This class includes devices that control the operation of multifunction IEEE 1284.4 peripheral devices. 
        /// </summary>
        Dot4,

        /// <summary>
        /// This class includes Dot4 print functions. A Dot4 print function is a function on a Dot4 device and 
        /// has a single child device, which is a member of the Printer device setup class. 
        /// </summary>
        Dot4Print,

        /// <summary>
        /// This class includes infrared devices. Drivers for this class include Serial-IR and Fast-IR NDIS miniports.
        /// </summary>
        Infrared,

        /// <summary>
        /// This class includes modem devices. An INF file for a device of this class specifies the features and configuration 
        /// of the device and stores this information in the registry. An INF file for a device of this class can also 
        /// be used to install device drivers for a controllerless modem or a software modem. These devices split the 
        /// functionality between the modem device and the device driver.
        /// </summary>
        Modem,

        /// <summary>
        /// This class includes NDIS miniport drivers excluding Fast-IR miniport drivers, NDIS intermediate drivers (of virtual adapters), and CoNDIS MCM miniport drivers. 
        /// </summary>
        Net,

        /// <summary>
        /// This class includes serial and parallel port devices. 
        /// </summary>
        Ports,

        /// <summary>
        /// This class includes printers.
        /// </summary>
        Printer,

        /// <summary>
        /// This class includes SCSI/1394-enumerated printers. Drivers for this class provide printer communication for a specific bus.
        /// </summary>
        PlugAndPlayPrinters,

        /// <summary>
        /// This class includes tape drives, including all tape miniclass drivers. 
        /// </summary>
        TapeDrive,

        /// <summary>
        /// This class includes USB host controllers and USB hubs, but not USB peripherals. Drivers for this class are system-supplied.
        /// </summary>
        UniversalSerialBus,

        /// <summary>
        /// USBDevice includes all USB devices that do not belong to another class. This class is not used for USB host controllers and hubs. 
        /// </summary>
        UniversalSerialBusDevice,

        /// <summary>
        /// The WCEUSBS setup class supports communication between a personal computer and a device that is compatible with the Windows CE ActiveSync driver (generally, PocketPC devices) over USB. 
        /// </summary>
        WindowsCeActiveSync,

        /// <summary>
        /// This class includes WPD devices. 
        /// </summary>
        WindowsPortableDevice,

        /// <summary>
        /// This class includes still-image capture devices, digital cameras, and scanners. 
        /// </summary>
        Image,

        /// <summary>
        /// This class includes Audio and DVD multimedia devices, joystick ports, and full-motion video capture devices.
        /// </summary>
        Media,

        /// <summary>
        /// This class includes IEEE 1394 devices that support the IEC-61883 protocol device class. The 61883 component includes
        /// the 61883.sys protocol driver that transmits various audio and video data streams over the 1394 bus. These currently
        /// include standard/high/low quality DV, MPEG2, DSS, and Audio. These data streams are defined by the IEC-61883 specifications.
        /// </summary>
        Iec61883,

        /// <summary>
        /// This class includes IEEE 1394 devices that support the AVC protocol device class.
        /// </summary>
        AdvancedVideoCoding,

        /// <summary>
        /// This class includes IEEE 1394 devices that support the SBP2 protocol device class.
        /// </summary>
        SerialBusProtocol2,

        /// <summary>
        /// This class includes combo cards, such as a PCMCIA modem and netcard adapter. The driver for such a Plug and Play multifunction 
        /// device is installed under this class and enumerates the modem and netcard separately as its child devices.
        /// </summary>
        Multifunction,

        /// <summary>
        /// This class includes battery devices and UPS devices. 
        /// </summary>
        Battery,

        /// <summary>
        /// This class includes all biometric-based personal identification devices.
        /// </summary>
        Biometric,

        /// <summary>
        /// This class includes video adapters. Drivers for this class include display drivers and video miniport drivers.
        /// </summary>
        Display,

        /// <summary>
        /// This class includes floppy disk drive controllers. 
        /// </summary>
        FloppyDiskController,

        /// <summary>
        /// This class includes hard disk controllers, including ATA/ATAPI controllers but not SCSI and RAID disk controllers. 
        /// </summary>
        HardDiskController,

        /// <summary>
        /// This class includes interactive input devices that are operated by the system-supplied HID class driver. 
        /// This includes USB devices that comply with the USB HID Standard and non-USB devices that use a HID minidriver.
        /// </summary>
        HidClass,

        /// <summary>
        /// This class includes 1394 host controllers connected on a PCI bus, but not 1394 peripherals. Drivers for this class are system-supplied.
        /// </summary>
        Ieee1394,

        /// <summary>
        /// This class includes all keyboards. That is, it must also be specified in the (secondary) INF for an enumerated child HID keyboard device.
        /// </summary>
        Keyboard,

        /// <summary>
        /// This class includes SCSI media changer devices.
        /// </summary>
        MediumChanger,

        /// <summary>
        /// This class includes memory devices, such as flash memory cards.
        /// </summary>
        MemoryTechnologyDevice,

        /// <summary>
        /// This class includes display monitors. An INF for a device of this class installs no device driver(s), but instead specifies the features of a particular 
        /// monitor to be stored in the registry for use by drivers of video adapters. (Monitors are enumerated as the child devices of display adapters.)
        /// </summary>
        Monitor,

        /// <summary>
        /// This class includes all mouse devices and other kinds of pointing devices, such as trackballs. That is, this class must also be specified in the (secondary) INF for an enumerated child HID mouse device.
        /// </summary>
        Mouse,

        /// <summary>
        /// This class includes intelligent multiport serial cards, but not peripheral devices that connect to its ports. It does not include unintelligent (16550-type) multiport serial controllers or single-port serial controllers.
        /// </summary>
        MultiportSerial,

        /// <summary>
        /// This class includes network and/or print providers.
        /// </summary>
        NetworkClient,

        /// <summary>
        /// This class includes network services, such as redirectors and servers. 
        /// </summary>
        NetworkService,

        /// <summary>
        /// This class includes NDIS protocols CoNDIS stand-alone call managers, and CoNDIS clients, in addition to higher level drivers in transport stacks.
        /// </summary>
        NetworkTransport,

        /// <summary>
        /// This class includes devices that accelerate secure socket layer (SSL) cryptographic processing.
        /// </summary>
        SecurityAccelerator,

        /// <summary>
        /// This class includes PCMCIA and CardBus host controllers, but not PCMCIA or CardBus peripherals. Drivers for this class are system-supplied.
        /// </summary>
        Pcmcia,

        /// <summary>
        /// This class includes processor types. 
        /// </summary>
        Processor,

        /// <summary>
        /// This class includes SCSI HBAs (Host Bus Adapters) and disk-array controllers. 
        /// </summary>
        ScsiAdapter,

        /// <summary>
        /// This class includes sensor and location devices, such as GPS devices. 
        /// </summary>
        Sensor,

        /// <summary>
        /// This class includes smart card readers.
        /// </summary>
        SmartcardReader,

        /// <summary>
        /// This class includes storage volumes as defined by the system-supplied logical volume manager and class drivers that create device objects to represent storage volumes, such as the system disk class driver. 
        /// </summary>
        Volume,

        /// <summary>
        /// This class includes HALs, system buses, system bridges, the system ACPI driver, and the system volume manager driver. 
        /// </summary>
        System,

        /// <summary>
        /// This class includes all devices that are compatible with Windows SideShow. 
        /// </summary>
        SideShow,

        /// <summary>
        /// This class is obsolete.
        /// </summary>
        Adapter,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        AdvancedPowerManagement,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        Computer,

        /// <summary>
        /// This class is reserved for future use.
        /// </summary>
        Decoders,

        /// <summary>
        /// This class is reserved for future use.
        /// </summary>
        GlobalPositioningSystem,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        Ieee1394Debugger,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        Ieee1394NetworkEnumerator,

        /// <summary>
        /// This class is obsolete.
        /// </summary>
        NoDriver,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        LegacyDriver,

        /// <summary>
        /// This class is reserved for system use. Enumerated devices for which the system cannot determine the type 
        /// are installed under this class. Do not use this class if you are unsure in which class your device belongs. 
        /// Either determine the correct device setup class or create a new class.
        /// </summary>
        Unknown,

        /// <summary>
        /// This class is reserved for system use. 
        /// </summary>
        PrinterUpgrade,

        /// <summary>
        /// This class is obsolete.
        /// </summary>
        Sound,

        /// <summary>
        /// This class is reserved for system use.
        /// </summary>
        VolumeSnapshot
    }

    #endregion

    /// <summary>
    /// Contains basic system-provided properties of the corresponding device. 
    /// </summary>
    public interface ISystemDevice
    {
        /// <summary>
        /// Device instance. Unique to this system device identifier. The uniqueness is guaranteed only until the system is restarted.
        /// </summary>
        int Instance
        {
            get;
        }

        /// <summary>
        /// Device setup class.
        /// </summary>
        SystemDeviceType DeviceType
        {
            get;
        }

        /// <summary>
        /// Name that is associated with the device's PDO.
        /// </summary>
        string PhysicalDeviceObjectName
        {
            get;
        }

        /// <summary>
        /// The name of the device's enumerator.
        /// </summary>
        string EnumeratorName
        {
            get;
        }

        /// <summary>
        /// Name of the device's setup class.
        /// </summary>
        string ClassName
        {
            get;
        }

        /// <summary>
        /// Identifier of the device's setup class.
        /// </summary>
        Guid ClassIdentifier
        {
            get;
        }

        /// <summary>
        /// The name of the device manufacturer.
        /// </summary>
        string Manufacturer
        {
            get;
        }

        /// <summary>
        /// The device's description. 
        /// </summary>
        string DeviceDescription
        {
            get;
        }

        /// <summary>
        /// The device's friendly name.
        /// </summary>
        string DisplayedName
        {
            get;
        }

        /// <summary>
        /// The most specific identifier from the list of device's hardware IDs.
        /// </summary>
        string HardwareIdentifier
        {
            get;
        }

        /// <summary>
        /// Device instance identifier which uniquely identifies the device in the system. The uniqueness is guaranteed across system restarts.
        /// </summary>
        string DeviceInstanceIdentifier
        {
            get;
        }

        /// <summary>
        /// Service name.
        /// </summary>
        string Service
        {
            get;
        }

        /// <summary>
        /// Parent device or null if the current device is removable root.
        /// </summary>
        ISystemDevice ParentDevice
        {
            get;
        }

        /// <summary>
        /// Direct child nodes of the current node. Device instance is used as a key in the dictionary.
        /// </summary>
        ReadOnlySet<ISystemDevice> ChildDevices
        {
            get;
        }

        /// <summary>
        /// Flattens device subtree and returns collection that contains all subtree devices including the current device.
        /// </summary>
        ReadOnlySet<ISystemDevice> AllDevices
        {
            get;
        }

        /// <summary>
        /// Device capabilities and flags.
        /// </summary>
        DeviceOptions Options
        {
            get;
        }

        /// <summary>
        /// Device status.
        /// </summary>
        DeviceError Status
        {
            get;
        }

        /// <summary>
        /// Logical disk names associated with the current device.
        /// </summary>
        ReadOnlySet<VolumeName> LogicalDisks
        {
            get;
        }

        /// <summary>
        /// Unique device identifier.
        /// </summary>
        string GlobalIdentifier
        {
            get;
        }

        /// <summary>
        /// True if device is not currently connected.
        /// </summary>
        bool Offline
        {
            get;
        }

        /// <summary>
        /// Blocks or unblocks the current device.
        /// </summary>
        /// <param name="blocked">Blocking state.</param>
        void SetBlockedState(bool blocked);

        /// <summary>
        /// Converts the current device to device descriptions. Result collection can be empty.
        /// </summary>
        /// <returns>Collection of device descriptions. All keys are from the AllDevices property.</returns>
        ReadOnlyDictionary<ISystemDevice, DeviceDescription> Convert();
    }
}
