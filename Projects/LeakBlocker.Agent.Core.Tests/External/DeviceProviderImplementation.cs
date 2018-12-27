using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Devices;

namespace LeakBlocker.Agent.Core.Tests.External
{
    internal sealed class DeviceProviderImplementation : BaseTestImplementation, IDeviceProvider
    {
        #region SystemDeviceImplementation

        class SystemDeviceImplementation : ISystemDevice
        {
            readonly DeviceProviderImplementation provider;

            public SystemDeviceImplementation(bool disk, DeviceProviderImplementation provider)
            {
                this.provider = provider;
                DeviceType = disk ? SystemDeviceType.DiskDrive : SystemDeviceType.Bluetooth;
            }

            int ISystemDevice.Instance
            {
                get
                {
                    return (DeviceType == SystemDeviceType.DiskDrive) ? 123 : 234;
                }
            }

            public SystemDeviceType DeviceType
            {
                get;
                set;
            }

            DeviceOptions ISystemDevice.Options
            {
                get
                {
                    return DeviceOptions.Removable;
                }
            }

            DeviceError ISystemDevice.Status
            {
                get
                {
                    return 0;
                }
            }

            public ReadOnlySet<VolumeName> LogicalDisks
            {
                get
                {
                    return (DeviceType != SystemDeviceType.DiskDrive) ? ReadOnlySet<VolumeName>.Empty : new VolumeName[] { CreateVolumeName(((ISystemDevice)this).Instance.ToString()) }.ToReadOnlySet();
                }
            }

            ReadOnlySet<ISystemDevice> ISystemDevice.ChildDevices
            {
                get
                {
                    return ReadOnlySet<ISystemDevice>.Empty;
                }
            }

            ReadOnlySet<ISystemDevice> ISystemDevice.AllDevices
            {
                get
                {
                    return new ISystemDevice[] { this }.ToReadOnlySet();
                }
            }

            string ISystemDevice.PhysicalDeviceObjectName
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_PhysicalDeviceObjectName";
                }
            }

            string ISystemDevice.EnumeratorName
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_Enumerator";
                }
            }

            string ISystemDevice.ClassName
            {
                get
                {
                    return DeviceType.ToString();
                }
            }

            string ISystemDevice.Manufacturer
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_Manufacturer";
                }
            }

            string ISystemDevice.DeviceDescription
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_Description";
                }
            }

            string ISystemDevice.DisplayedName
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_DisplayedName";
                }
            }

            string ISystemDevice.HardwareIdentifier
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_HardwareIdentifier";
                }
            }

            string ISystemDevice.DeviceInstanceIdentifier
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_DeviceInstanceIdentifier";
                }
            }

            ISystemDevice ISystemDevice.ParentDevice
            {
                get
                {
                    return null;
                }
            }

            string ISystemDevice.GlobalIdentifier
            {
                get
                {
                    return ((ISystemDevice)this).Instance.ToString() + "_GlobalIdentifier";
                }
            }

            Guid ISystemDevice.ClassIdentifier
            {
                get
                {
                    return Guid.Empty;
                }
            }

            void ISystemDevice.SetBlockedState(bool blocked)
            {
                provider.RegisterMethodCall("device_SetBlockedState", this, blocked);
            }

            private static VolumeName CreateVolumeName(string name = null)
            {
                VolumeName result = (VolumeName)FormatterServices.GetUninitializedObject(typeof(VolumeName));
                typeof(VolumeName).GetField("name", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(result, name ?? @"\Device\HarddiskVolume1");
                return result;
            }

            ReadOnlyDictionary<ISystemDevice, DeviceDescription> ISystemDevice.Convert()
            {
                DeviceDescription description = new DeviceDescription(((ISystemDevice)this).DisplayedName, ((ISystemDevice)this).GlobalIdentifier, DeviceType == SystemDeviceType.DiskDrive ? DeviceCategory.Storage : DeviceCategory.Network);

                return new Dictionary<ISystemDevice, DeviceDescription> { { this, description } }.ToReadOnlyDictionary();
            }


            string ISystemDevice.Service
            {
                get { return "qqq"; }
            }


            public bool Offline
            {
                get { throw new NotImplementedException(); }
            }
        }

        #endregion SystemDeviceImplementation

        private readonly ReadOnlySet<ISystemDevice> systemDevices;

        public DeviceProviderImplementation()
        {
            systemDevices = new ISystemDevice[] { new SystemDeviceImplementation(true, this), new SystemDeviceImplementation(false, this) }.ToReadOnlySet();
        }

        public ReadOnlySet<ISystemDevice> EnumerateSystemDevices(bool includeOffline = false)
        {
            return systemDevices;
        }

        public ReadOnlySet<DeviceDescription> EnumerateDevices()
        {
            return EnumerateSystemDevices().SelectMany(device => device.Convert().Values).ToReadOnlySet();
        }
    }
}
