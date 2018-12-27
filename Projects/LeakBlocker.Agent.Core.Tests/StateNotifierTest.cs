using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class StateNotifierTest : BaseTest
    {
        sealed class Handler : BaseTestImplementation, IStateNotifierHandler
        {
            public bool Throw;

            public void ServiceStarted()
            {
                base.RegisterMethodCall("ServiceStarted");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void ServiceStopped()
            {
                base.RegisterMethodCall("ServiceStopped");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void SystemStarted()
            {
                base.RegisterMethodCall("SystemStarted");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void SystemShutdown()
            {
                base.RegisterMethodCall("SystemShutdown");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void UnplannedServiceShutdown()
            {
                base.RegisterMethodCall("UnplannedServiceShutdown");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void ServerInaccessible()
            {
                base.RegisterMethodCall("ServerInaccessible");

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void FileAccessed(DeviceDescription device, string file, AgentFileActionType state, string process, BaseUserAccount user)
            {
                Assert.IsNotNull(device);
                Assert.IsNotNull(user);
                Assert.IsFalse(string.IsNullOrWhiteSpace(file));
                Assert.IsFalse(string.IsNullOrWhiteSpace(process));

                base.RegisterMethodCall("FileAccessed", device, file, state, process, user);

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void DeviceAdded(DeviceDescription device)
            {
                base.RegisterMethodCall("DeviceAdded", device);

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void DeviceRemoved(DeviceDescription device)
            {
                base.RegisterMethodCall("DeviceRemoved", device);

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void UserLoggedOn(BaseUserAccount user)
            {
                base.RegisterMethodCall("UserLoggedOn", user);

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void UserLoggedOff(BaseUserAccount user)
            {
                base.RegisterMethodCall("UserLoggedOff", user);

                if (Throw)
                    throw new InvalidOperationException();
            }

            public void DeviceStateChanged(DeviceDescription device, DeviceAccessType state)
            {
                base.RegisterMethodCall("DeviceStateChanged", device, state);

                if (Throw)
                    throw new InvalidOperationException();
            }
        }

        [TestMethod]
        public void StateNotifierTest1()
        {
            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            Assert.IsNotNull(target.DeviceAccess);
            Assert.IsFalse(target.DeviceAccess.Keys1.Without(users).Any());
            Assert.IsFalse(target.DeviceAccess.Keys2.Without(devices).Any());

            handler.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StateNotifierTest2()
        {
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            new StateNotifier(null, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StateNotifierTest3()
        {
            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            new StateNotifier(handler, null, devices.ToReadOnlySet(), states.ToReadOnlyDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StateNotifierTest4()
        {
            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            new StateNotifier(handler, users.ToReadOnlySet(), null, states.ToReadOnlyDictionary());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StateNotifierTest5()
        {
            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StateNotifierTest6()
        {
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = ReadOnlySet<BaseUserAccount>.Empty;
            IEnumerable<DeviceDescription> devices = ReadOnlySet<DeviceDescription>.Empty;
            IDictionary<DeviceDescription, DeviceAccessType> states = ReadOnlyDictionary<DeviceDescription, DeviceAccessType>.Empty;

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());
            target.ServiceStart();
            target.ServiceStart();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StateNotifierTest7()
        {
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorageImplementation());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = ReadOnlySet<BaseUserAccount>.Empty;
            IEnumerable<DeviceDescription> devices = ReadOnlySet<DeviceDescription>.Empty;
            IDictionary<DeviceDescription, DeviceAccessType> states = ReadOnlyDictionary<DeviceDescription, DeviceAccessType>.Empty;

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());
            target.ServiceStop();
            target.ServiceStop();
        }
        
        [TestMethod]
        public void StateNotifierTest8()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStarted");

            target.ServiceStart();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest8a()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStarted");

            target.ServiceStart();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest9()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            SystemObjects.FileTools.WriteFile(AgentObjects.AgentConstants.UnexpectedTerminationFlagFile, new byte[1].ToReadOnlyList());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStarted");
            handler.AddExpectedMethodCall("UnplannedServiceShutdown");

            target.ServiceStart();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest9a()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            SystemObjects.FileTools.WriteFile(AgentObjects.AgentConstants.UnexpectedTerminationFlagFile, new byte[1].ToReadOnlyList());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStarted");
            handler.AddExpectedMethodCall("UnplannedServiceShutdown");

            target.ServiceStart();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest10()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStopped");

            target.ServiceStop();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest10a()
        {
            AgentObjects.Singletons.AgentConstants.SetTestImplementation(new AgentConstants());
            SystemObjects.Singletons.FileTools.SetTestImplementation(new FileToolsImplementation());
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());
            SystemObjects.Factories.PrivateRegistryStorage.EnqueueConstructor(id => new PrivateRegistryStorageImplementation());
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(new AgentPrivateStorage());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServiceStopped");

            target.ServiceStop();

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest11()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("SystemStarted");

            target.SystemStart();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest11a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("SystemStarted");

            target.SystemStart();

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest12()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("SystemShutdown");

            target.SystemShutdown();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest12a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("SystemShutdown");

            target.SystemShutdown();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest13()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServerInaccessible");

            target.ServerInaccessible();

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest13a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("ServerInaccessible");

            target.ServerInaccessible();

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest14()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            DeviceDescription device = DeviceDescriptionTest.First;
            string file = "test.txt";
            AgentFileActionType action = AgentFileActionType.ReadBlocked;
            string process = "notepad.exe";
            BaseUserAccount user = LocalUserAccountTest.First;

            handler.AddExpectedMethodCall("FileAccessed", device, file, action, process, user);

            target.FileAccessed(device, file, action, process, user);

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest14a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            DeviceDescription device = DeviceDescriptionTest.First;
            string file = "test.txt";
            AgentFileActionType action = AgentFileActionType.ReadBlocked;
            string process = "notepad.exe";
            BaseUserAccount user = LocalUserAccountTest.First;

            handler.AddExpectedMethodCall("FileAccessed", device, file, action, process, user);

            target.FileAccessed(device, file, action, process, user);

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest15()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            DeviceDescription device = DeviceDescriptionTest.First;
            DeviceAccessType state = DeviceAccessType.ReadOnly;

            handler.AddExpectedMethodCall("DeviceStateChanged", device, state);

            target.SetDeviceState(device, state);

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest15a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            DeviceDescription device = DeviceDescriptionTest.First;
            DeviceAccessType state = DeviceAccessType.ReadOnly;

            handler.AddExpectedMethodCall("DeviceStateChanged", device, state);

            target.SetDeviceState(device, state);

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest16()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            DeviceDescription device = DeviceDescriptionTest.First;
            DeviceAccessType state = DeviceAccessType.ReadOnly;

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("DeviceStateChanged", device, state);
            
            target.SetDeviceState(device, state);
            handler.Validate();

            target.SetDeviceState(device, state);

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest17()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects.Skip(1);
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("DeviceAdded", DeviceDescriptionTest.objects.First());

            target.SetNewDevices(DeviceDescriptionTest.objects.ToReadOnlySet());

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest17a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects.Skip(1);
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("DeviceAdded", DeviceDescriptionTest.objects.First());

            target.SetNewDevices(DeviceDescriptionTest.objects.ToReadOnlySet());

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest18()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("DeviceRemoved", DeviceDescriptionTest.objects.First());

            target.SetNewDevices(DeviceDescriptionTest.objects.Skip(1).ToReadOnlySet());

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest19()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects.Skip(1).ToList();
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("DeviceAdded", DeviceDescriptionTest.objects.First());
            handler.AddExpectedMethodCall("DeviceRemoved", DeviceDescriptionTest.objects.Last());

            target.SetNewDevices(DeviceDescriptionTest.objects.Take(devices.Count()).ToReadOnlySet());

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest20()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            target.SetNewDevices(DeviceDescriptionTest.objects.ToReadOnlySet());

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest21()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects.Skip(1);
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("UserLoggedOn", LocalUserAccountTest.objects.First());

            target.SetNewUsers(LocalUserAccountTest.objects.ToReadOnlySet());

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest21a()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            handler.Throw = true;
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects.Skip(1);
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("UserLoggedOn", LocalUserAccountTest.objects.First());

            target.SetNewUsers(LocalUserAccountTest.objects.ToReadOnlySet());

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest22()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("UserLoggedOff", LocalUserAccountTest.objects.First());

            target.SetNewUsers(LocalUserAccountTest.objects.Skip(1).ToReadOnlySet());

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest23()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects.Skip(1).ToList();
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            handler.AddExpectedMethodCall("UserLoggedOn", LocalUserAccountTest.objects.First());
            handler.AddExpectedMethodCall("UserLoggedOff", LocalUserAccountTest.objects.Last());

            target.SetNewUsers(LocalUserAccountTest.objects.Take(users.Count()).ToReadOnlySet());

            handler.Validate();
        }
        
        [TestMethod]
        public void StateNotifierTest24()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            target.SetNewUsers(LocalUserAccountTest.objects.ToReadOnlySet());

            handler.Validate();
        }

        [TestMethod]
        public void StateNotifierTest25()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            target.DeviceAccess = DeviceAccessMapTest.First;

            handler.Validate();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StateNotifierTest26()
        {
            SharedObjects.Singletons.ExceptionSuppressor.SetTestImplementation(new ExceptionSuppressor());

            var handler = new Handler();
            IEnumerable<BaseUserAccount> users = LocalUserAccountTest.objects;
            IEnumerable<DeviceDescription> devices = DeviceDescriptionTest.objects;
            IDictionary<DeviceDescription, DeviceAccessType> states = devices.ToDictionary(item => item, item => DeviceAccessType.Blocked);

            IStateNotifier target = new StateNotifier(handler, users.ToReadOnlySet(), devices.ToReadOnlySet(), states.ToReadOnlyDictionary());

            target.DeviceAccess = null;

            handler.Validate();
        }
    }
}
