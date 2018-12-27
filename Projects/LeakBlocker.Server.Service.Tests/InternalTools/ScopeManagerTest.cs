using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules.Conditions;
using LeakBlocker.Libraries.Common.Entities.Settings.TemporaryAccess;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools.Entities;
using LeakBlocker.Libraries.SystemTools.Entities.Implementations;
using LeakBlocker.Libraries.SystemTools.Tests.Entities;
using LeakBlocker.Libraries.SystemTools.Tests.Entities.Implementations;
using LeakBlocker.Server.Service.InternalTools;
using LeakBlocker.Server.Service.InternalTools.LocalStorages;
using LeakBlocker.Server.Service.InternalTools.SingletonImplementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace LeakBlocker.Server.Service.Tests.InternalTools
{
    [TestClass]
    public sealed class ScopeManagerTest : BaseTest
    {
        private static void EmptyMethod()
        {

        }

        [TestMethod]
        public void ForseUpdateScope()
        {
            var cacheContainer = DomainSnapshotTest.First;

            var cache = Mocks.StrictMock<ISecurityObjectCache>();
            var configuration = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(cache);
            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configuration);

            configuration.Stub(x => x.CurrentFullConfiguration).Return(GenerateConfig());
            cache.Stub(x => x.Updated += EmptyMethod).IgnoreArguments();
            cache.Stub(x => x.Data).Do(new Func<IDomainSnapshot>(() => cacheContainer));

            bool changeWasRaised = false;

            Mocks.ReplayAll();

            var target = new ScopeManager();

            target.ScopeChanged += () => changeWasRaised = true;

            Assert.AreEqual(ReadOnlySet<BaseComputerAccount>.Empty, target.CurrentScope());

            cacheContainer = DomainSnapshotTest.Second;//Поменяли обстановку в домене

            target.ForceUpdateScope();

            Assert.AreNotEqual(ReadOnlySet<BaseComputerAccount>.Empty, target.CurrentScope());

            Assert.IsTrue(changeWasRaised);
        }

        [TestMethod]
        public void Events()
        {
            var cacheContainer = DomainSnapshotTest.First;

            var cache = Mocks.StrictMock<ISecurityObjectCache>();
            var configuration = Mocks.StrictMock<IConfigurationStorage>();

            InternalObjects.Singletons.SecurityObjectCache.SetTestImplementation(cache);
            InternalObjects.Singletons.ConfigurationStorage.SetTestImplementation(configuration);

            configuration.Stub(x => x.CurrentFullConfiguration).Return(GenerateConfig());
            IEventRaiser raiser = cache.Stub(x => x.Updated += EmptyMethod).IgnoreArguments().GetEventRaiser();

            cache.Stub(x => x.Data).Do(new Func<IDomainSnapshot>(() => cacheContainer));

            Mocks.ReplayAll();

            bool changeWasRaised = false;

            var target = new ScopeManager();

            target.ScopeChanged += () => changeWasRaised = true;

            Assert.AreEqual(ReadOnlySet<BaseComputerAccount>.Empty, target.CurrentScope());

            cacheContainer = DomainSnapshotTest.Second;//Поменяли обстановку в домене

            raiser.Raise();

            Assert.AreNotEqual(ReadOnlySet<BaseComputerAccount>.Empty, target.CurrentScope());
            Assert.IsTrue(changeWasRaised);
        }

        private static ProgramConfiguration GenerateConfig()
        {
            var entry = DomainSnapshotTest.Second;

            return new ProgramConfiguration(
                1,
                new[]{
                    new Rule("123", 1, 
                        new ComputerListRuleCondition(
                            false, 
                            entry.Domains, 
                            ReadOnlySet<OrganizationalUnit>.Empty,
                            ReadOnlySet<DomainGroupAccount>.Empty,
                            ReadOnlySet<BaseComputerAccount>.Empty),
                            new ActionData(BlockActionType.Complete, AuditActionType.DeviceAndFiles)) }.ToReadOnlySet(),
                ReadOnlySet<BaseTemporaryAccessCondition>.Empty);
        }
    }
}
