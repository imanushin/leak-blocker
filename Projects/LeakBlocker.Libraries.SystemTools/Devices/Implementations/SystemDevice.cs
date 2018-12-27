using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools.Devices.Management;
using LeakBlocker.Libraries.SystemTools.Network;

namespace LeakBlocker.Libraries.SystemTools.Devices.Implementations
{
    internal sealed class SystemDevice : BaseReadOnlyObject, ISystemDevice
    {
        private readonly SystemAccessOptions systemAccessOptions;

        public int Instance
        {
            get;
            private set;
        }

        public SystemDeviceType DeviceType
        {
            get;
            private set;
        }

        public string PhysicalDeviceObjectName
        {
            get;
            private set;
        }

        public string EnumeratorName
        {
            get;
            private set;
        }

        public string ClassName
        {
            get;
            private set;
        }

        public string Manufacturer
        {
            get;
            private set;
        }

        public string DeviceDescription
        {
            get;
            private set;
        }

        public string DisplayedName
        {
            get;
            private set;
        }

        public string HardwareIdentifier
        {
            get;
            private set;
        }

        public string DeviceInstanceIdentifier
        {
            get;
            private set;
        }

        public string Service
        {
            get;
            private set;
        }

        public ISystemDevice ParentDevice
        {
            get;
            private set;
        }

        public ReadOnlySet<ISystemDevice> ChildDevices
        {
            get;
            private set;
        }

        public ReadOnlySet<ISystemDevice> AllDevices
        {
            get
            {
                return new ISystemDevice[] { this }.Union(ChildDevices.SelectMany(device => device.AllDevices)).ToReadOnlySet();
            }
        }

        public DeviceOptions Options
        {
            get;
            private set;
        }

        public DeviceError Status
        {
            get;
            private set;
        }

        public Guid ClassIdentifier
        {
            get;
            private set;
        }

        public ReadOnlySet<VolumeName> LogicalDisks
        {
            get;
            private set;
        }

        public string GlobalIdentifier
        {
            get
            {
                return IdentifierDataProvider.CreateIdentifier(HardwareIdentifier, DeviceInstanceIdentifier, ClassIdentifier,
                    Options.HasFlag(DeviceOptions.UniqueId), (ParentDevice == null) ? null : ParentDevice.GlobalIdentifier);
            }
        }

        public bool Offline
        {
            get
            {
                return !Options.HasFlag(DeviceOptions.NtEnumerator);
            }
        }

        public void SetBlockedState(bool blocked)
        {
            if (!Offline && (!((Status == DeviceError.Disabled) ^ blocked)))
                return;

            if (!Offline && !Options.HasFlag(DeviceOptions.CanBeDisabled))
            {
                Log.Write("Device {0} cannot be disabled".Combine(DisplayedName));
                return;
            }

            using (new AuthenticatedConnection(systemAccessOptions))
            using (var handle = new DeviceManagementHandle(systemAccessOptions.SystemName, true))
            {
                if (!DeviceManagement.SetEnabledState(handle, DeviceInstanceIdentifier, !blocked, Offline))
                {
                    SharedObjects.ExceptionSuppressor.Run(delegate
                    {
                        IWaitHandle waitHandle = SharedObjects.AsyncInvoker.Invoke(delegate
                        {
                            SystemHandle.EnumerateSystemHandles().Where(systemHandle =>
                                systemHandle.ObjectName == PhysicalDeviceObjectName).ForEach(systemHandle => systemHandle.Close());
                        });
                        if (!waitHandle.Wait(TimeSpan.FromSeconds(5)))
                            waitHandle.Abort();

                        DeviceManagement.SetEnabledState(handle, DeviceInstanceIdentifier, !blocked, false);
                    });
                }
            }
        }

        internal static ReadOnlySet<ISystemDevice> EnumerateDevices(bool includeOffline = false, SystemAccessOptions options = default(SystemAccessOptions), bool onlyRemovable = true)
        {
            using (new TimeMeasurement("Device enumeration"))
            using (new AuthenticatedConnection(options))
            using (var handle = new DeviceManagementHandle(options.SystemName, false))
            {
                ReadOnlySet<DeviceTreeNode> nodes = DeviceManagement.EnumerateDevices(handle);

                nodes = FilterVirtualNodes(nodes);

                if (onlyRemovable)
                    nodes = nodes.Where(node => DeviceManagement.GetDeviceOptions(handle, node).HasFlag(DeviceOptions.Removable)).ToReadOnlySet();

                if (!includeOffline)
                    nodes = nodes.Where(node => DeviceManagement.GetDeviceOptions(handle, node).HasFlag(DeviceOptions.Started)).ToReadOnlySet();

                var rootNodes = new HashSet<DeviceTreeNode>();
                IEnumerable<DeviceTreeNode> allNodes = nodes.SelectMany(currentNode => currentNode.AllDevices);
                
                foreach (DeviceTreeNode node in allNodes)
                {
                    if (allNodes.Any(currentNode => currentNode.ChildDevices.Contains(node)))
                        continue;
                    rootNodes.Add(node);
                }

                ReadOnlySet<ISystemDevice> devices = rootNodes.Select(node => (ISystemDevice)new SystemDevice(null, handle, node, options)).ToReadOnlySet();

                //WriteDebugData(devices);

                return devices;
            }
        }

        internal static ReadOnlySet<DeviceTreeNode> FilterVirtualNodes(ReadOnlySet<DeviceTreeNode> nodes)
        {
            var parentMapping = new Dictionary<DeviceTreeNode, DeviceTreeNode>();
            
            foreach (DeviceTreeNode currentNode in nodes)
            {
                foreach (DeviceTreeNode currentChildNode in currentNode.ChildDevices)
                    parentMapping[currentChildNode] = currentNode;
            }

            ReadOnlySet<DeviceTreeNode> physicalRoots = nodes.Where(node => DeviceManagement.GetDeviceClassType(node) == SystemDeviceType.Computer).ToReadOnlySet();
            
            var result = new HashSet<DeviceTreeNode>();

            foreach(DeviceTreeNode currentNode in nodes)
            {
                DeviceTreeNode parentNode = currentNode;
                while(true)
                {
                    parentNode = parentMapping.TryGetValue(parentNode);
                    if(parentNode == null)
                        break;

                    if (physicalRoots.Contains(parentNode))
                    {
                        result.Add(currentNode);
                        break;
                    }
                }
            }

            //Log.Add("Following nodes are virtual: {0}.", string.Join(", ", nodes.Without(result).Select(node => "{0} ({1})".Combine(node.Instance, node.ClassIdentifier))));

            return result.ToReadOnlySet();
        }

        private SystemDevice(ISystemDevice parentDevice, DeviceManagementHandle handle, DeviceTreeNode node, SystemAccessOptions options = default(SystemAccessOptions))
        {
            var childNodes = new HashSet<ISystemDevice>();
            foreach (DeviceTreeNode currentNode in node.ChildDevices)
                childNodes.Add(new SystemDevice(this, handle, currentNode));

            systemAccessOptions = options;

            var hardwareIdentifiers = DeviceManagement.GetRegistryProperty<IEnumerable<string>>(handle, node, DeviceProperty.HardwareIdentifier);

            ClassIdentifier = node.ClassIdentifier;
            Options = DeviceManagement.GetDeviceOptions(handle, node);
            Instance = node.Instance;
            DeviceType = DeviceManagement.GetDeviceClassType(node);
            ParentDevice = parentDevice;
            DeviceInstanceIdentifier = DeviceManagement.GetDeviceInstanceIdentifier(handle, node);
            PhysicalDeviceObjectName = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.PhysicalDeviceObjectName, true) ?? string.Empty;
            EnumeratorName = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.EnumeratorName) ?? string.Empty;
            ClassName = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.ClassName) ?? string.Empty;
            Manufacturer = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.Manufacturer) ?? string.Empty;
            DeviceDescription = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.DeviceDescription) ?? string.Empty;
            DisplayedName = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.FriendlyName) ?? string.Empty;
            HardwareIdentifier = ((hardwareIdentifiers == null) || (!hardwareIdentifiers.Any())) ? string.Empty : hardwareIdentifiers.First();
            Service = DeviceManagement.GetRegistryProperty<string>(handle, node, DeviceProperty.Service) ?? string.Empty;
            Status = DeviceManagement.GetDeviceError(handle, node);
            ChildDevices = childNodes.ToReadOnlySet();

            LogicalDisks = Options.HasFlag(DeviceOptions.HasProblem) ? ReadOnlySet<VolumeName>.Empty : DeviceManagement.GetDeviceDisks(handle, node);
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return Instance;
        }

        #endregion

        protected override string GetString()
        {
            return "{0}: {1} ({2})".Combine(DeviceType, DeviceInstanceIdentifier, string.Join(", ", LogicalDisks));
        }

        public ReadOnlyDictionary<ISystemDevice, DeviceDescription> Convert()
        {
            return DeviceConverter.ConvertDevice(this);
        }

        private static void WriteDebugData(IEnumerable<ISystemDevice> rootDevices)
        {
            StringBuilder result = new StringBuilder();

            Action<ISystemDevice, int> recursiveMethod = null;
            recursiveMethod = delegate(ISystemDevice device, int offset)
            {
                var data = new Dictionary<string, object>
                {
                    { "Instance", device.Instance },
                    { "DeviceType", device.DeviceType },
                    { "PhysicalDeviceObjectName", device.PhysicalDeviceObjectName },
                    { "EnumeratorName", device.EnumeratorName },
                    { "ClassName", device.ClassName },
                    { "Manufacturer", device.Manufacturer },
                    { "DeviceDescription", device.DeviceDescription },
                    { "DisplayedName", device.DisplayedName },
                    { "HardwareIdentifier", device.HardwareIdentifier },
                    { "DeviceInstanceIdentifier", device.DeviceInstanceIdentifier },
                    { "Service", device.Service },
                    { "Options", device.Options },
                    { "Status", device.Status },
                    { "GlobalIdentifier", device.GlobalIdentifier },
                    { "Offline", device.Offline },
                    { "LogicalDisks", string.Join(", ", device.LogicalDisks.Select(item => item.ToString())) },
                };

                result.AppendLine();
                for (int i = 0; i < offset + 1; i++)
                    result.Append("   ");
                result.Append("***** DEVICE *****");

                foreach (KeyValuePair<string, object> currentItem in data)
                {
                    result.AppendLine();
                    for (int i = 0; i < offset + 1; i++)
                        result.Append("   ");
                    result.Append("{0,-25} {1}".Combine(currentItem.Key, currentItem.Value));
                }

                foreach (ISystemDevice currentChildDevice in device.ChildDevices)
                    recursiveMethod(currentChildDevice, offset + 1);
            };

            foreach(ISystemDevice currentItem in rootDevices)
                recursiveMethod(currentItem, 0);

            Log.Write(result.ToString());
        }
    }
}
