using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Agent.Core.Tests.External;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Cryptography;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class LocalControlServerTest : BaseTest
    {
        #region Handler

        class Handler : ILocalControlServerHandler
        {
            public bool ExpectSetConfiguration;
            public bool ExpectRequestUninstall;

            public ProgramConfiguration ExpectArgumentSetConfiguration;
            public bool ExpectArgumentRequestUninstall;

            int counterSetConfiguration;
            int counterRequestUninstall;

            public void SetConfiguration()
            {
                counterSetConfiguration++;
            }

            public void RequestUninstall()
            {
                counterRequestUninstall++;
            }

            public void Check()
            {
                if (ExpectSetConfiguration)
                    counterSetConfiguration--;

                if (ExpectRequestUninstall)
                    counterRequestUninstall--;

                Assert.AreEqual(0, counterSetConfiguration);
                Assert.AreEqual(0, counterRequestUninstall);
            }
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalControlServerTest1()
        {
            new LocalControlServer(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalControlServerTest2()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);
            
            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.RequestUninstall(null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalControlServerTest3()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.RequestUninstall("");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalControlServerTest4()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.RequestUninstall("   ");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalControlServerTest5()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.SetConfiguration(null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalControlServerTest6()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.SetConfiguration("");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LocalControlServerTest7()
        {
            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(new Handler()))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.SetConfiguration("   ");
            }
        }

        [TestMethod]
        public void LocalControlServerTest16()
        {
            AgentPrivateStorageImplementation privateStorage = new AgentPrivateStorageImplementation();

            AgentInstallerImplementation installer = new AgentInstallerImplementation();
            AgentObjects.Singletons.AgentInstaller.SetTestImplementation(installer);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            var handler = new Handler();

            privateStorage.SecretKey = SymmetricEncryptionKeyTest.Second.ToBase64String();

            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(handler))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.RequestUninstall("qwe");
                handler.Check();
            }
            installer.Validate();
        }

        [TestMethod]
        public void LocalControlServerTest18()
        {
            AgentPrivateStorageImplementation privateStorage = new AgentPrivateStorageImplementation();

            AgentInstallerImplementation installer = new AgentInstallerImplementation();
            AgentObjects.Singletons.AgentInstaller.SetTestImplementation(installer);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            ProgramConfiguration config = ProgramConfigurationTest.First;

            var handler = new Handler();

            privateStorage.PasswordHash = "dsgsghdhdhhe";

            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(handler))
            using (ILocalControlClient client = new LocalControlClient())
            {
                client.SetConfiguration("qwe");
                handler.Check();

                Thread.Sleep(1000);

                handler.Check();
            }
            installer.Validate();
        }

        [TestMethod]
        public void LocalControlServerTest19()
        {
            AgentPrivateStorageImplementation privateStorage = new AgentPrivateStorageImplementation();

            AgentInstallerImplementation installer = new AgentInstallerImplementation();
            AgentObjects.Singletons.AgentInstaller.SetTestImplementation(installer);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            var handler = new Handler();

            privateStorage.SecretKey = SymmetricEncryptionKeyTest.First.ToBase64String();

            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(handler))
            using (ILocalControlClient client = new LocalControlClient())
            {
                handler.ExpectRequestUninstall = true;
                handler.ExpectArgumentRequestUninstall = false;
                client.RequestUninstall(SymmetricEncryptionKeyTest.First.ToBase64String());

                Thread.Sleep(1000);

                handler.Check();
            }
            installer.Validate();
        }
                
        [TestMethod]
        public void LocalControlServerTest21()
        {
            AgentPrivateStorageImplementation privateStorage = new AgentPrivateStorageImplementation();

            AgentInstallerImplementation installer = new AgentInstallerImplementation();
            AgentObjects.Singletons.AgentInstaller.SetTestImplementation(installer);
            AgentObjects.Singletons.AgentPrivateStorage.SetTestImplementation(privateStorage);

            ProgramConfiguration config = ProgramConfigurationTest.First;

            var handler = new Handler();

            privateStorage.SecretKey = SymmetricEncryptionKeyTest.First.ToBase64String();

            Mailslot mailslot = new Mailslot();
            SystemObjects.Factories.MailslotClient.EnqueueConstructor((name, messageSize) => mailslot);
            SystemObjects.Factories.MailslotServer.EnqueueConstructor((name, messageSize) => mailslot);

            using (ILocalControlServer server = new LocalControlServer(handler))
            using (ILocalControlClient client = new LocalControlClient())
            {
                handler.ExpectArgumentSetConfiguration = config;
                handler.ExpectSetConfiguration = true;
                client.SetConfiguration(SymmetricEncryptionKeyTest.First.ToBase64String());

                Thread.Sleep(1000);

                handler.Check();
            }
            installer.Validate();
        }
    }
}
