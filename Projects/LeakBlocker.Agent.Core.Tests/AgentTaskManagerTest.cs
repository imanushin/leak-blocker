using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Settings;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Agent.Core.Tests.Settings;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.ServerShared.AgentCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public sealed class AgentTaskManagerTest : BaseTest
    {
        private DeviceProviderImplementation deviceProvider;
        private FileSystemAccessControllerImplementation fileSystemAccessController;
        private StateNotifierImplementation stateNotifier;
        private IEnumerable<ISystemDevice> checkedDevices;

        [TestInitialize]
        public void Init()
        {
            deviceProvider = new DeviceProviderImplementation();
            checkedDevices = deviceProvider.EnumerateSystemDevices();

            fileSystemAccessController = new FileSystemAccessControllerImplementation();
            stateNotifier = new StateNotifierImplementation();

            SystemObjects.Singletons.DeviceProvider.SetTestImplementation(deviceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AgentTaskManagerTest1()
        {
            new AgentTaskManager(null, new FileSystemDriverControllerImplementation(true), new AgentDataStorageImplementation(false), new AgentServiceControllerImplementation());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AgentTaskManagerTest2()
        {
            new AgentTaskManager(new AuditStorageImplementation(), null, new AgentDataStorageImplementation(false), new AgentServiceControllerImplementation());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AgentTaskManagerTest3()
        {
            new AgentTaskManager(new AuditStorageImplementation(), new FileSystemDriverControllerImplementation(true), null, new AgentServiceControllerImplementation());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AgentTaskManagerTest3a()
        {
            new AgentTaskManager(new AuditStorageImplementation(), new FileSystemDriverControllerImplementation(true), new AgentDataStorageImplementation(false), null);
        }

        [TestMethod]
        public void AgentTaskManagerTest4()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest5()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();
            var fileTools = new FileToolsImplementation();
            var agentConstants = new AgentConstants();

            AgentObjects.Singletons.AgentConstants.SetTestImplementation(agentConstants);
            SystemObjects.Singletons.FileTools.SetTestImplementation(fileTools);

            IReadOnlyCollection<byte> serializeToXml = ProgramConfigurationTest.First.SerializeToXml().ToReadOnlyList();

            fileTools.WriteFile(AgentObjects.AgentConstants.AgentConfigurationOverrideFile, serializeToXml);
            
            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            stateNotifier.AddExpectedMethodCall("Start");
            dataStorage.AddExpectedMethodCall("Settings", ProgramConfigurationTest.First);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.OverrideConfiguration();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest6()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            stateNotifier.AddExpectedMethodCall("Start");
            dataStorage.AddExpectedMethodCall("Update");
            stateNotifier.AddExpectedMethodCall("SetNewUsers", dataStorage.Users.Select(item => item.User));

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.QueryTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest7()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var client = new AgentControlServiceClientImplementation(ProgramConfigurationTest.First);
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            AgentObjects.Singletons.AgentControlServiceClient.SetTestImplementation(client);

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("SystemShutdown");
            client.AddExpectedMethodCall("SendShutdownNotification");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.SystemShutdownTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            client.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest8()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("SystemStart");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.SystemStartTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest9()
        {
            var package = new AuditItemPackage(new[] { new AgentAuditItem(AuditItemType.AgentComputerTurnedOn, new Time(new DateTime(2000, 1, 1))) }.ToReadOnlySet());
            var state = new AgentState(package, DeviceAccessMapTest.First);

            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation(package);
            var client = new AgentControlServiceClientImplementation(ProgramConfigurationTest.First);
            var privateStorage = new AgentPrivateStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            AgentObjects.Singletons.AgentControlServiceClient.SetTestImplementation(client);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            stateNotifier.AddExpectedMethodCall("Start");
            client.AddExpectedMethodCall("Synchronize", state);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.NetworkTask();

            Assert.IsTrue(privateStorage.Licensed);

            driverController.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            client.Validate();
            deviceProvider.Validate();

            AgentObjects.Factories.HardwareProfile.EnqueueTestImplementation(new HardwareProfileImplementation());
            dataStorage.AddExpectedMethodCall("Settings", ProgramConfigurationTest.First);

            target.MainTask();
            dataStorage.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest9a()
        {
            var package = new AuditItemPackage(new AgentAuditItem[] { new AgentAuditItem(AuditItemType.AgentComputerTurnedOn, new Time(new DateTime(2000, 1, 1))) }.ToReadOnlySet());
            var state = new AgentState(package, DeviceAccessMapTest.First);

            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation(package);
            var client = new AgentControlServiceClientImplementation(null);
            var privateStorage = new AgentPrivateStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            AgentObjects.Singletons.AgentControlServiceClient.SetTestImplementation(client);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            stateNotifier.AddExpectedMethodCall("Start");
            client.AddExpectedMethodCall("Synchronize", state);
            stateNotifier.AddExpectedMethodCall("ServerInaccessible");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.NetworkTask();

            Assert.IsTrue(privateStorage.Licensed);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            client.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest10()
        {
            var package = new AuditItemPackage(new[] { new AgentAuditItem(AuditItemType.AgentComputerTurnedOn, new Time(new DateTime(2000, 1, 1))) }.ToReadOnlySet());
            var state = new AgentState(package, DeviceAccessMapTest.First);

            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation(package);
            var client = new AgentControlServiceClientImplementation(ProgramConfigurationTest.First, false);
            var privateStorage = new AgentPrivateStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            AgentObjects.Singletons.AgentControlServiceClient.SetTestImplementation(client);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            stateNotifier.AddExpectedMethodCall("Start");
            client.AddExpectedMethodCall("Synchronize", state);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);

            target.NetworkTask();

            driverController.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            client.Validate();
            deviceProvider.Validate();

            AgentObjects.Factories.HardwareProfile.EnqueueTestImplementation(new HardwareProfileImplementation());
            dataStorage.AddExpectedMethodCall("Settings", ProgramConfigurationTest.First);

            target.MainTask();
            Assert.IsFalse(privateStorage.Licensed);
            dataStorage.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest10a()
        {
            var package = new AuditItemPackage(new[] { new AgentAuditItem(AuditItemType.AgentComputerTurnedOn, new Time(new DateTime(2000, 1, 1))) }.ToReadOnlySet());
            var state = new AgentState(package, DeviceAccessMapTest.First);

            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation(package);
            var client = new AgentControlServiceClientImplementation(ProgramConfigurationTest.First, true, false);
            var privateStorage = new AgentPrivateStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            AgentObjects.Singletons.AgentControlServiceClient.SetTestImplementation(client);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            stateNotifier.AddExpectedMethodCall("Start");
            client.AddExpectedMethodCall("Synchronize", state);
            serviceController.AddExpectedMethodCall("Uninstall");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.NetworkTask();
            
            driverController.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            client.Validate();
            deviceProvider.Validate();

            AgentObjects.Factories.HardwareProfile.EnqueueTestImplementation(new HardwareProfileImplementation());
            dataStorage.AddExpectedMethodCall("Settings", ProgramConfigurationTest.First);

            target.MainTask();
            dataStorage.Validate();
        }
        
        [TestMethod]
        public void AgentTaskManagerTest12()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("Stop");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.ServiceStopTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest13()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IVolumeDetachMessage message = new VolumeDetachMessageImplementation
            {
                InstanceIdentifier = 456
            };

            stateNotifier.AddExpectedMethodCall("Start");
            fileSystemAccessController.AddExpectedMethodCall("RemoveDriverInstance", message.InstanceIdentifier);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.DriverDetachedFromVolume(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest14()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IVolumeAttachMessage message = new VolumeAttachMessageImplementation
            {
                InstanceIdentifier = 456,
                Name = CreateVolumeName("test")
            };

            fileSystemAccessController.SetAccessRule(DeviceDescriptionTest.First, message.Name, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            IDictionary<AccountSecurityIdentifier, FileAccessType> accessRules = new Dictionary<AccountSecurityIdentifier, FileAccessType>
                {
                    { AccountSecurityIdentifierTest.First, FileAccessType.ReadOnly }
                }.ToReadOnlyDictionary();
            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", DeviceDescriptionTest.First, message.Name, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);

            stateNotifier.AddExpectedMethodCall("Start");
            fileSystemAccessController.AddExpectedMethodCall("AddDriverInstance", message.Name, message.InstanceIdentifier);
            driverController.AddExpectedMethodCall("SetInstanceConfiguration", message.InstanceIdentifier, accessRules);
            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.DriverAttachedToVolume(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest15()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.ReadAllowed,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName()
            };

            fileSystemAccessController.SetAccessRule(DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            fileSystemAccessController.SetAuditRule(DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, true);
            IDictionary<AccountSecurityIdentifier, FileAccessType> accessRules = new Dictionary<AccountSecurityIdentifier, FileAccessType>
                {
                    { AccountSecurityIdentifierTest.First, FileAccessType.ReadOnly }
                }.ToReadOnlyDictionary();
            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            fileSystemAccessController.AddExpectedMethodCall("SetAuditRule", DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, true);
            fileSystemAccessController.OverrideDevices[message.Volume] = DeviceDescriptionTest.First;

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("FileAccessed", DeviceDescriptionTest.First, message.FileName, AgentFileActionType.ReadAllowed,
                message.ProcessName, dataStorage.Users.First(user => user.User.Identifier == message.UserIdentifier).User);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest16()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.Unknown,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName()
            };

            fileSystemAccessController.SetAccessRule(DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            IDictionary<AccountSecurityIdentifier, FileAccessType> accessRules = new Dictionary<AccountSecurityIdentifier, FileAccessType>
                {
                    { AccountSecurityIdentifierTest.First, FileAccessType.ReadOnly }
                }.ToReadOnlyDictionary();
            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            fileSystemAccessController.OverrideDevices[message.Volume] = DeviceDescriptionTest.First;

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest17()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.ReadAllowed,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName(),
                Directory = true
            };

            fileSystemAccessController.SetAccessRule(DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            IDictionary<AccountSecurityIdentifier, FileAccessType> accessRules = new Dictionary<AccountSecurityIdentifier, FileAccessType>
                {
                    { AccountSecurityIdentifierTest.First, FileAccessType.ReadOnly }
                }.ToReadOnlyDictionary();
            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            fileSystemAccessController.OverrideDevices[message.Volume] = DeviceDescriptionTest.First;

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest18()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.Unknown,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName()
            };

            fileSystemAccessController.SetAccessRule(DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            IDictionary<AccountSecurityIdentifier, FileAccessType> accessRules = new Dictionary<AccountSecurityIdentifier, FileAccessType>
                {
                    { AccountSecurityIdentifierTest.First, FileAccessType.ReadOnly }
                }.ToReadOnlyDictionary();
            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", DeviceDescriptionTest.First, message.Volume, AccountSecurityIdentifierTest.First, DeviceAccessType.ReadOnly);
            fileSystemAccessController.OverrideDevices[message.Volume] = DeviceDescriptionTest.First;

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest19()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.Unknown,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName()
            };

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest19a()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            fileSystemAccessController.NoDevices = true;

            IFileNotificationMessage message = new FileNotificationMessageImplementation
            {
                AppliedAction = FileAccessType.Allow,
                FileName = "qqq",
                ProcessName = "www",
                ResultAction = FileActionType.ReadBlocked,
                SystemTime = new Time(new DateTime(2000, 1, 1)),
                UserIdentifier = AccountSecurityIdentifierTest.First,
                Volume = CreateVolumeName()
            };

            stateNotifier.AddExpectedMethodCall("Start");

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.FileAccessNotification(message);

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest20()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var hardwareProfile = new HardwareProfileImplementation();
            var serviceController = new AgentServiceControllerImplementation();
            
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation { Licensed = true });
            AgentObjects.Factories.HardwareProfile.EnqueueConstructor((a1, a2, a3, a4) => hardwareProfile);
            IEnumerable<CachedUserData> checkedUsers = dataStorage.Users;

            var ruleCheckResult = RuleCheckResultTest.CreateWithFilling(
                checkedUsers.Select(user => user.User),
                checkedDevices.SelectMany(device => device.Convert().Values),
                new CommonActionData(DeviceAccessType.Blocked, AuditActionType.DeviceAndFiles));
            hardwareProfile.AccessMap = ruleCheckResult.AccessMap;
            hardwareProfile.SetAuditMap(ruleCheckResult.AuditMap);

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            foreach (ISystemDevice device in checkedDevices)
            {
                DeviceDescription deviceDescription = device.Convert().First().Value;

                stateNotifier.AddExpectedMethodCall("SetDeviceState", deviceDescription, DeviceAccessType.Blocked);
                foreach (BaseUserAccount user in checkedUsers.Select(user => user.User))
                {
                    //if (driverController.Available)
                    //{
                        foreach (VolumeName volume in device.LogicalDisks)
                        {
                            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", deviceDescription, volume, user.Identifier, DeviceAccessType.Blocked);
                            fileSystemAccessController.AddExpectedMethodCall("SetAuditRule", deviceDescription, volume, user.Identifier, true);
                        }
                    //}
                }

                if ((device.LogicalDisks.Count == 0) || !driverController.Available)
                    deviceProvider.AddExpectedMethodCall("device_SetBlockedState", device, true);

                dataStorage.AddExpectedMethodCall("SaveDeviceState", deviceDescription, DeviceAccessType.Blocked);
            }

            var allVolumes = new HashSet<VolumeName>(checkedDevices.SelectMany(device => device.LogicalDisks));

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("DeviceAccess", hardwareProfile.AccessMap);
            stateNotifier.AddExpectedMethodCall("SetNewDevices", deviceProvider.EnumerateDevices());
            driverController.AddExpectedMethodCall("SetManagedVolumes", allVolumes);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.MainTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();

            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest21()
        {
            var driverController = new FileSystemDriverControllerImplementation(false);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var hardwareProfile = new HardwareProfileImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation { Licensed = true });
            AgentObjects.Factories.HardwareProfile.EnqueueConstructor((a1, a2, a3, a4) => hardwareProfile);

            IEnumerable<CachedUserData> checkedUsers = dataStorage.Users;

            var ruleCheckResult = RuleCheckResultTest.CreateWithFilling(
                 checkedUsers.Select(user => user.User),
                 checkedDevices.SelectMany(device => device.Convert().Values),
                 new CommonActionData(DeviceAccessType.Blocked, AuditActionType.DeviceAndFiles));
            hardwareProfile.AccessMap = ruleCheckResult.AccessMap;
            hardwareProfile.SetAuditMap(ruleCheckResult.AuditMap);

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            foreach (ISystemDevice device in checkedDevices)
            {
                DeviceDescription deviceDescription = device.Convert().First().Value;

                stateNotifier.AddExpectedMethodCall("SetDeviceState", deviceDescription, DeviceAccessType.Blocked);
                foreach (BaseUserAccount user in checkedUsers.Select(user => user.User))
                {
                    //if (driverController.Available)
                    //{
                        foreach (VolumeName volume in device.LogicalDisks)
                        {
                            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", deviceDescription, volume, user.Identifier, DeviceAccessType.Blocked);
                            fileSystemAccessController.AddExpectedMethodCall("SetAuditRule", deviceDescription, volume, user.Identifier, true);
                        }
                    //}
                }

                if ((device.LogicalDisks.Count == 0) || !driverController.Available)
                    deviceProvider.AddExpectedMethodCall("device_SetBlockedState", device, true);

                dataStorage.AddExpectedMethodCall("SaveDeviceState", deviceDescription, DeviceAccessType.Blocked);
            }

            var allVolumes = new HashSet<VolumeName>(checkedDevices.SelectMany(device => device.LogicalDisks));

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("DeviceAccess", hardwareProfile.AccessMap);
            stateNotifier.AddExpectedMethodCall("SetNewDevices", deviceProvider.EnumerateDevices());
            driverController.AddExpectedMethodCall("SetManagedVolumes", allVolumes);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.MainTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();

            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest22()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var hardwareProfile = new HardwareProfileImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation { Licensed = true });
            AgentObjects.Factories.HardwareProfile.EnqueueConstructor((a1, a2, a3, a4) => hardwareProfile);

            IEnumerable<CachedUserData> checkedUsers = dataStorage.Users;

            var ruleCheckResult = RuleCheckResultTest.CreateWithFilling(
                   checkedUsers.Select(user => user.User),
                   checkedDevices.SelectMany(device => device.Convert().Values),
                   new CommonActionData(DeviceAccessType.TemporarilyAllowed, AuditActionType.DeviceAndFiles));
            hardwareProfile.AccessMap = ruleCheckResult.AccessMap;
            hardwareProfile.SetAuditMap(ruleCheckResult.AuditMap);

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            foreach (ISystemDevice device in checkedDevices)
            {
                DeviceDescription deviceDescription = device.Convert().First().Value;

                stateNotifier.AddExpectedMethodCall("SetDeviceState", deviceDescription, DeviceAccessType.TemporarilyAllowed);
                foreach (BaseUserAccount user in checkedUsers.Select(user => user.User))
                {
                    //if (driverController.Available)
                    //{
                        foreach (VolumeName volume in device.LogicalDisks)
                        {
                            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", deviceDescription, volume, user.Identifier, DeviceAccessType.TemporarilyAllowed);
                            fileSystemAccessController.AddExpectedMethodCall("SetAuditRule", deviceDescription, volume, user.Identifier, true);
                        }
                    //}
                }

                if ((device.LogicalDisks.Count == 0) || !driverController.Available)
                    deviceProvider.AddExpectedMethodCall("device_SetBlockedState", device, false);

                dataStorage.AddExpectedMethodCall("SaveDeviceState", deviceDescription, DeviceAccessType.TemporarilyAllowed);
            }

            var allVolumes = new HashSet<VolumeName>(checkedDevices.SelectMany(device => device.LogicalDisks));

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("DeviceAccess", hardwareProfile.AccessMap);
            stateNotifier.AddExpectedMethodCall("SetNewDevices", deviceProvider.EnumerateDevices());
            driverController.AddExpectedMethodCall("SetManagedVolumes", allVolumes);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.MainTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        [TestMethod]
        public void AgentTaskManagerTest23()
        {
            var driverController = new FileSystemDriverControllerImplementation(true);
            var dataStorage = new AgentDataStorageImplementation(false);
            var auditStorage = new AuditStorageImplementation();
            var hardwareProfile = new HardwareProfileImplementation();
            var serviceController = new AgentServiceControllerImplementation();

            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation { Licensed = true });
            AgentObjects.Factories.HardwareProfile.EnqueueConstructor((a1, a2, a3, a4) => hardwareProfile);

            IEnumerable<CachedUserData> checkedUsers = dataStorage.Users;

            var ruleCheckResult = RuleCheckResultTest.CreateWithFilling(
            checkedUsers.Select(user => user.User),
            checkedDevices.SelectMany(device => device.Convert().Values),
            new CommonActionData(DeviceAccessType.Allowed, AuditActionType.DeviceAndFiles));
            hardwareProfile.AccessMap = ruleCheckResult.AccessMap;
            hardwareProfile.SetAuditMap(ruleCheckResult.AuditMap);

            bool oneFileSystemAccessControllerInstance = false;
            AgentObjects.Factories.FileSystemAccessController.EnqueueConstructor(delegate
            {
                Assert.IsFalse(oneFileSystemAccessControllerInstance);
                oneFileSystemAccessControllerInstance = true;
                return fileSystemAccessController;
            });

            bool oneStateNotifierInstance = false;
            AgentObjects.Factories.StateNotifier.EnqueueConstructor((handler, users, devices, states) =>
            {
                Assert.IsFalse(oneStateNotifierInstance);
                oneStateNotifierInstance = true;
                return stateNotifier;
            });

            foreach (ISystemDevice device in checkedDevices)
            {
                DeviceDescription deviceDescription = device.Convert().First().Value;

                stateNotifier.AddExpectedMethodCall("SetDeviceState", deviceDescription, DeviceAccessType.Allowed);
                foreach (BaseUserAccount user in checkedUsers.Select(user => user.User))
                {
                    //if (driverController.Available)
                    //{
                        foreach (VolumeName volume in device.LogicalDisks)
                        {
                            fileSystemAccessController.AddExpectedMethodCall("SetAccessRule", deviceDescription, volume, user.Identifier, DeviceAccessType.Allowed);
                            fileSystemAccessController.AddExpectedMethodCall("SetAuditRule", deviceDescription, volume, user.Identifier, true);
                        }
                   // }
                }

                if ((device.LogicalDisks.Count == 0) || !driverController.Available)
                    deviceProvider.AddExpectedMethodCall("device_SetBlockedState", device, false);
                
                dataStorage.AddExpectedMethodCall("SaveDeviceState", deviceDescription, DeviceAccessType.Allowed);
            }

            var allVolumes = new HashSet<VolumeName>(checkedDevices.SelectMany(device => device.LogicalDisks));

            stateNotifier.AddExpectedMethodCall("Start");
            stateNotifier.AddExpectedMethodCall("DeviceAccess", hardwareProfile.AccessMap);
            stateNotifier.AddExpectedMethodCall("SetNewDevices", deviceProvider.EnumerateDevices());
            driverController.AddExpectedMethodCall("SetManagedVolumes", allVolumes);

            IAgentTaskManager target = new AgentTaskManager(auditStorage, driverController, dataStorage, serviceController);
            target.MainTask();

            driverController.Validate();
            dataStorage.Validate();
            fileSystemAccessController.Validate();
            stateNotifier.Validate();
            auditStorage.Validate();
            deviceProvider.Validate();
        }

        private struct FileNotificationMessageImplementation : IFileNotificationMessage
        {
            public string FileName
            {
                get;
                set;
            }
            public VolumeName Volume
            {
                get;
                set;
            }
            public AccountSecurityIdentifier UserIdentifier
            {
                get;
                set;
            }
            public bool Read
            {
                get;
                set;
            }
            public bool Write
            {
                get;
                set;
            }
            public bool Delete
            {
                get;
                set;
            }
            public bool Directory
            {
                get;
                set;
            }
            public Time SystemTime
            {
                get;
                set;
            }
            public FileAccessType AppliedAction
            {
                get;
                set;
            }
            public string ProcessName
            {
                get;
                set;
            }
            public FileActionType ResultAction
            {
                get;
                set;
            }
        }

        private struct VolumeAttachMessageImplementation : IVolumeAttachMessage
        {
            public long InstanceIdentifier
            {
                get;
                set;
            }
            public VolumeName Name
            {
                get;
                set;
            }
        }

        private struct VolumeDetachMessageImplementation : IVolumeDetachMessage
        {
            public long InstanceIdentifier
            {
                get;
                set;
            }
        }

        private static VolumeName CreateVolumeName(string name = null)
        {
            var result = (VolumeName)FormatterServices.GetUninitializedObject(typeof(VolumeName));
            var fieldInfo = typeof(VolumeName).GetField("name", BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo != null)
                fieldInfo.SetValue(result, name ?? @"\Device\HarddiskVolume1");

            return result;
        }
    }
}
