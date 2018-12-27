using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Tests.Entities.Settings;
using LeakBlocker.Libraries.Storage;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation.LocalStorages;
using LeakBlocker.ServerShared.AdminViewCommunication;
using LeakBlocker.ServerShared.AdminViewCommunication.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Service.Tests.InternalTools.LocalStorages
{
    [TestClass]
    public sealed class ConfigurationStorageTest : BaseConfigurationManagerTest
    {
        [TestMethod]
        public void SaveLoadNewTest()
        {
            var configurationManager = Mocks.Stub<IConfigurationManager>();

            StorageObjects.Singletons.ConfigurationManager.SetTestImplementation(configurationManager);

            configurationManager.Expect(x => x.SaveConfiguration(null)).IgnoreArguments();
            configurationManager.Stub(x => x.GetLastConfiguration()).IgnoreArguments().Return(null);

            Mocks.ReplayAll();

            CleanupDirectory();

            var target = new ConfigurationStorage();

            Assert.IsNull(target.Current);
            Assert.IsNotNull(target.CurrentOrDefault);
            Assert.IsNotNull(target.CurrentFullConfiguration);

            SimpleConfiguration first = SimpleConfigurationTest.First;

            target.Save(first);

            Assert.AreEqual(first, target.Current);
            Assert.IsNotNull(target.CurrentFullConfiguration);

            var another = new ConfigurationStorage();

            Assert.AreEqual(first, another.Current);
        }

        [TestMethod]
        public void UpdateTest()
        {
            ProgramConfiguration previousFull = ProgramConfigurationTest.First;

            var configurationManager = Mocks.Stub<IConfigurationManager>();

            StorageObjects.Singletons.ConfigurationManager.SetTestImplementation(configurationManager);

            configurationManager.Expect(x => x.SaveConfiguration(null)).IgnoreArguments().Repeat.Twice();//Подготовка и непосредственно сохранение
            configurationManager.Stub(x => x.GetLastConfiguration()).IgnoreArguments().Return(previousFull);

            Mocks.ReplayAll();

            CleanupDirectory();

            SimpleConfiguration firstConfig = SimpleConfigurationTest.First;
            SimpleConfiguration secondConfig = SimpleConfigurationTest.Second;

            {//Скрываем область видимости, чтобы косвенно не использовать
                var preparationTarget = new ConfigurationStorage();

                preparationTarget.Save(firstConfig);
            }

            var target = new ConfigurationStorage();

            Assert.AreEqual(firstConfig, target.Current);

            target.Save(secondConfig);

            Assert.AreEqual(secondConfig,target.Current);

            Assert.AreNotEqual(previousFull, target.CurrentFullConfiguration);
        }
    }
}
