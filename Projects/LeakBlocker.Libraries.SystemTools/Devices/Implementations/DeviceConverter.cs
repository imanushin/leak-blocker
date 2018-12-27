using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools.Devices.Implementations.CustomConverters;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations
{
    internal abstract class DeviceConverter
    {
        private static readonly ReadOnlyList<DeviceConverter> converters = new List<DeviceConverter>
        {
            new UniversalSerialBusHubConverter(),
            new UniversalSerialBusDriveConverter(),
        }.ToReadOnlyList();

        internal static ReadOnlyDictionary<ISystemDevice, DeviceDescription> ConvertDevice(ISystemDevice systemDevice)
        {
            Check.ObjectIsNotNull(systemDevice, "systemDevice");

            return converters.UnionWith(new DefaultDeviceConverter()).First(converter => converter.CheckDevice(systemDevice)).Convert(systemDevice);
        }
        
        protected abstract bool CheckDevice(ISystemDevice systemDevice);

        protected virtual ReadOnlyDictionary<ISystemDevice, DeviceDescription> Convert(ISystemDevice systemDevice)
        {
            Check.ObjectIsNotNull(systemDevice, "systemDevice");

            var result = new Dictionary<ISystemDevice, DeviceDescription> { { systemDevice, GetDescription(systemDevice) } };
            if(!SupportedFilter(systemDevice))
                result.Remove(systemDevice);

            if(systemDevice.ChildDevices.Count == 0)
                return result.ToReadOnlyDictionary();
            
            ReadOnlySet<ISystemDevice> devices = systemDevice.ChildDevices;
            foreach (KeyValuePair<ISystemDevice, DeviceDescription> device in devices.SelectMany(ConvertDevice).Where(device => SupportedFilter(device.Key)))
                result[device.Key] = device.Value;

            return result.ToReadOnlyDictionary();
        }

        private static bool SupportedFilter(ISystemDevice device)
        {
            return !device.EnumeratorName.Equals(@"PCI", StringComparison.OrdinalIgnoreCase) && 
                !device.DeviceInstanceIdentifier.StartsWith(@"USB\ROOT_HUB", StringComparison.OrdinalIgnoreCase);
        }

        private static DeviceDescription GetDescription(ISystemDevice device)
        {
            return new DeviceDescription(GetFriendlyName(device), device.GlobalIdentifier, ConvertType(device));
        }

        private static string GetFriendlyName(ISystemDevice systemDevice)
        {
            Check.ObjectIsNotNull(systemDevice, "systemDevice");

            if (!string.IsNullOrEmpty(systemDevice.DisplayedName))
                return systemDevice.DisplayedName;

            return !string.IsNullOrEmpty(systemDevice.DeviceDescription) ? systemDevice.DeviceDescription : systemDevice.DeviceType.ToString();
        }

        private static DeviceCategory ConvertType(ISystemDevice systemDevice)
        {
            if (systemDevice.LogicalDisks.Count > 0)
                return DeviceCategory.Storage;

            switch (systemDevice.DeviceType)
            {
                case SystemDeviceType.Bluetooth:
                case SystemDeviceType.Infrared:
                case SystemDeviceType.Modem:
                case SystemDeviceType.Net:
                case SystemDeviceType.NetworkClient:
                case SystemDeviceType.NetworkService:
                case SystemDeviceType.NetworkTransport:
                    return DeviceCategory.Network;

                case SystemDeviceType.Iec61883:
                case SystemDeviceType.Ieee1394:
                case SystemDeviceType.Ieee1394NetworkEnumerator:
                    return DeviceCategory.FireWire;

                case SystemDeviceType.Image:
                    return DeviceCategory.ImageAcquisition;

                case SystemDeviceType.WindowsCeActiveSync:
                case SystemDeviceType.WindowsPortableDevice:
                    return DeviceCategory.Mobile;

                case SystemDeviceType.OpticalDrive:
                    return DeviceCategory.OpticalDrive;

                case SystemDeviceType.Dot4:
                case SystemDeviceType.Dot4Print:
                case SystemDeviceType.Printer:
                case SystemDeviceType.PlugAndPlayPrinters:
                    return DeviceCategory.Printer;

                case SystemDeviceType.MemoryTechnologyDevice:
                case SystemDeviceType.DiskDrive:
                case SystemDeviceType.FloppyDisk:
                case SystemDeviceType.SerialBusProtocol2:
                case SystemDeviceType.FloppyDiskController:
                case SystemDeviceType.HardDiskController:
                case SystemDeviceType.ScsiAdapter:
                case SystemDeviceType.Volume:
                    return DeviceCategory.Storage;

                case SystemDeviceType.UniversalSerialBus:
                case SystemDeviceType.UniversalSerialBusDevice:
                    return DeviceCategory.UniversalSerialBus;

                case SystemDeviceType.HidClass:
                case SystemDeviceType.Keyboard:
                case SystemDeviceType.Mouse:
                    return DeviceCategory.UserInput;

                case SystemDeviceType.Ports:
                case SystemDeviceType.TapeDrive:
                case SystemDeviceType.Media:
                case SystemDeviceType.AdvancedVideoCoding:
                case SystemDeviceType.Multifunction:
                case SystemDeviceType.Battery:
                case SystemDeviceType.Biometric:
                case SystemDeviceType.Display:
                case SystemDeviceType.MediumChanger:
                case SystemDeviceType.Monitor:
                case SystemDeviceType.MultiportSerial:
                case SystemDeviceType.SecurityAccelerator:
                case SystemDeviceType.Pcmcia:
                case SystemDeviceType.Processor:
                case SystemDeviceType.Sensor:
                case SystemDeviceType.SmartcardReader:
                case SystemDeviceType.System:
                case SystemDeviceType.SideShow:
                case SystemDeviceType.Adapter:
                case SystemDeviceType.AdvancedPowerManagement:
                case SystemDeviceType.Computer:
                case SystemDeviceType.Decoders:
                case SystemDeviceType.GlobalPositioningSystem:
                case SystemDeviceType.Ieee1394Debugger:
                case SystemDeviceType.NoDriver:
                case SystemDeviceType.LegacyDriver:
                case SystemDeviceType.Unknown:
                case SystemDeviceType.PrinterUpgrade:
                case SystemDeviceType.Sound:
                case SystemDeviceType.VolumeSnapshot:
                    return DeviceCategory.Unsupported;

                default:
                    return DeviceCategory.Unknown;
            }
        }
    }
}
