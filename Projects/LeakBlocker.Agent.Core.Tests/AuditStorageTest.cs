using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Audit;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.ServerShared.AgentCommunication;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public sealed class AuditStorageTest : BaseTest
    {
        private static readonly Time time = new Time(new DateTime(2000, 1, 1, 1, 1, 1, 1));

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest0()
        {
            new AuditStorage(new StackStorageImplementation(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest1()
        {
            new AuditStorage(null, new AgentDataStorageImplementation(false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest2()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.Read((Action<AuditItemPackage>)null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest3()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.DeviceAdded(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest4()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.DeviceRemoved(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest5()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.DeviceStateChanged(null, DeviceAccessType.Allowed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest6()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.DeviceStateChanged(DeviceDescriptionTest.First, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest7()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(null, "qqq", AgentFileActionType.ReadAllowed, "sss", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest8()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, null, AgentFileActionType.ReadAllowed, "sss", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest9()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "", AgentFileActionType.ReadAllowed, "sss", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest10()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "   ", AgentFileActionType.ReadAllowed, "sss", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest11()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "ddd", 0, "sss", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest12()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "ddd", AgentFileActionType.ReadAllowed, null, LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest13()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "ddd", AgentFileActionType.ReadAllowed, "", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AuditStorageTest14()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "ddd", AgentFileActionType.ReadAllowed, "   ", LocalUserAccountTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest15()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.FileAccessed(DeviceDescriptionTest.First, "ddd", AgentFileActionType.ReadAllowed, "www", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest16()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.UserLoggedOff(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuditStorageTest17()
        {
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), new AgentDataStorageImplementation(false));
            target.UserLoggedOn(null);
        }

        [TestMethod]
        public void AuditStorageTest18()
        {
            foreach (BaseUserAccount user in new [] { new AgentDataStorageImplementation(false).ConsoleUser, null })
            {
                var value = new AgentAuditItem(AuditItemType.DeviceConnected, time, user, null, null, DeviceDescriptionTest.First);
                CheckAuditItemCreation(value, target => target.DeviceAdded(DeviceDescriptionTest.First), user != null);
            }
        }

        [TestMethod]
        public void AuditStorageTest19()
        {
            foreach (BaseUserAccount user in new [] { new AgentDataStorageImplementation(false).ConsoleUser, null })
            {
                var value = new AgentAuditItem(AuditItemType.DeviceDisconnected, time, user, null, null, DeviceDescriptionTest.First);
                CheckAuditItemCreation(value, target => target.DeviceRemoved(DeviceDescriptionTest.First), user != null);
            }
        }

        [TestMethod]
        public void AuditStorageTest20()
        {
            int version = new AgentDataStorageImplementation(false).Settings.ConfigurationVersion;

            foreach (DeviceAccessType item in EnumHelper<DeviceAccessType>.Values)
            {
                foreach (BaseUserAccount user in new [] { new AgentDataStorageImplementation(false).ConsoleUser, null })
                {
                    AuditItemType eventType = LinkedEnumHelper<DeviceAccessType, AuditItemType>.GetLinkedEnum(item);

                    var value = new AgentAuditItem(eventType, time, user, null, null, DeviceDescriptionTest.First, version);
                    CheckAuditItemCreation(value, target => target.DeviceStateChanged(DeviceDescriptionTest.First, item), user != null);
                }
            }
        }
        
        [TestMethod]
        public void AuditStorageTest21()
        {
            var value = new AgentAuditItem(AuditItemType.ServerInaccessible, time);
            CheckAuditItemCreation(value, target => target.ServerInaccessible(), true);
            CheckAuditItemCreation(value, target => target.ServerInaccessible(), false);
        }

        [TestMethod]
        public void AuditStorageTest22()
        {
            var value = new AgentAuditItem(AuditItemType.AgentStarted, time);
            CheckAuditItemCreation(value, target => target.ServiceStarted(), true);
            CheckAuditItemCreation(value, target => target.ServiceStarted(), false);
        }

        [TestMethod]
        public void AuditStorageTest23()
        {
            var value = new AgentAuditItem(AuditItemType.AgentStopped, time);
            CheckAuditItemCreation(value, target => target.ServiceStopped(), true);
            CheckAuditItemCreation(value, target => target.ServiceStopped(), false);
        }

        [TestMethod]
        public void AuditStorageTest24()
        {
            var value = new AgentAuditItem(AuditItemType.ComputerTurnedOff, time);
            CheckAuditItemCreation(value, target => target.SystemShutdown(), true);
            CheckAuditItemCreation(value, target => target.SystemShutdown(), false);
        }

        [TestMethod]
        public void AuditStorageTest25()
        {
            var value = new AgentAuditItem(AuditItemType.ComputerTurnedOn, time);
            CheckAuditItemCreation(value, target => target.SystemStarted(), true);
            CheckAuditItemCreation(value, target => target.SystemStarted(), false);
        }

        [TestMethod]
        public void AuditStorageTest26()
        {
            var value = new AgentAuditItem(AuditItemType.UnplannedServiceShutdown, time);
            CheckAuditItemCreation(value, target => target.UnplannedServiceShutdown(), true);
            CheckAuditItemCreation(value, target => target.UnplannedServiceShutdown(), false);
        }

        [TestMethod]
        public void AuditStorageTest27()
        {
            BaseUserAccount user = LocalUserAccountTest.First;

            var value = new AgentAuditItem(AuditItemType.UserLoggedOff, time, user);
            CheckAuditItemCreation(value, target => target.UserLoggedOff(user), false);
            CheckAuditItemCreation(value, target => target.UserLoggedOff(user), true);
        }

        [TestMethod]
        public void AuditStorageTest28()
        {
            BaseUserAccount user = LocalUserAccountTest.First;

            var value = new AgentAuditItem(AuditItemType.UserLoggedOn, time, user);
            CheckAuditItemCreation(value, target => target.UserLoggedOn(user), false);
            CheckAuditItemCreation(value, target => target.UserLoggedOn(user), true);
        }

        [TestMethod]
        public void AuditStorageTest29()
        {
            DeviceDescription device = DeviceDescriptionTest.First;
            BaseUserAccount user = LocalUserAccountTest.First;
            int version = new AgentDataStorageImplementation(false).Settings.ConfigurationVersion;
            const string file = "test.exe";
            const string process = "notepad.exe";

            var auditItemTypes = new Dictionary<AgentFileActionType, AuditItemType>
            {
                { AgentFileActionType.ReadTemporarilyAllowed,                 AuditItemType.ReadFileTemporarilyAllowed },
                { AgentFileActionType.WriteTemporarilyAllowed,                AuditItemType.WriteFileTemporarilyAllowed },
                { AgentFileActionType.ReadWriteTemporarilyAllowed,            AuditItemType.ReadWriteFileTemporarilyAllowed },
                { AgentFileActionType.ReadAllowed,                            AuditItemType.ReadFileAllowed },
                { AgentFileActionType.WriteAllowed,                           AuditItemType.WriteFileAllowed },
                { AgentFileActionType.ReadWriteAllowed,                       AuditItemType.ReadWriteFileAllowed },
                { AgentFileActionType.ReadBlocked,                            AuditItemType.ReadFileBlocked },
                { AgentFileActionType.WriteBlocked,                           AuditItemType.WriteFileBlocked },
                { AgentFileActionType.ReadWriteBlocked,                       AuditItemType.ReadWriteFileBlocked },
            };

            var dataStorage = new AgentDataStorageImplementation(false);
            SystemObjects.Singletons.TimeProvider.SetTestImplementation(new TimeProviderImplementation(time));
            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), dataStorage);
            target.FileAccessed(device, file, AgentFileActionType.Unknown, process, user);
            target.Read(package => Assert.IsFalse(package.Any()));
            dataStorage.Validate();

            dataStorage = new AgentDataStorageImplementation(true);
            SystemObjects.Singletons.TimeProvider.SetTestImplementation(new TimeProviderImplementation(time));
            target = new AuditStorage(new StackStorageImplementation(), dataStorage);
            target.FileAccessed(device, file, AgentFileActionType.Unknown, process, user);
            target.Read(package => Assert.IsFalse(package.Any()));
            dataStorage.Validate();


            foreach (AgentFileActionType item in EnumHelper<AgentFileActionType>.Values)
            {
                if (item == AgentFileActionType.Unknown)
                    continue;

                var value = new AgentAuditItem(auditItemTypes[item], time, user, file, process, device, version, 1);
                CheckAuditItemCreation(value, caller => caller.FileAccessed(device, file, item, process, user), true);
                CheckAuditItemCreation(value, caller => caller.FileAccessed(device, file, item, process, user), false);
            }
        }
        
        private void CheckAuditItemCreation(AgentAuditItem desiredValue, Action<IAuditStorage> eventCaller, bool consoleUser)
        {
            SystemObjects.Singletons.TimeProvider.SetTestImplementation(new TimeProviderImplementation(time));

            var dataStorage = new AgentDataStorageImplementation(consoleUser);

            IAuditStorage target = new AuditStorage(new StackStorageImplementation(), dataStorage);
            eventCaller(target);

            AgentAuditItem result = null;

            target.Read(package =>
            {
                Assert.AreEqual(1, package.Count());
                result = package.First();
            });

            Assert.AreEqual(desiredValue, result);

            dataStorage.Validate();
        }
    }
}
