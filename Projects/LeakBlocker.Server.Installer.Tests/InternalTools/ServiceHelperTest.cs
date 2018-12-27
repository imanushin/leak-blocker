using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Implementations;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Libraries.SystemTools.ProcessTools;
using LeakBlocker.Server.Installer.InternalTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace LeakBlocker.Server.Installer.Tests.InternalTools
{
    [TestClass]
    public sealed class ServiceHelperTest : BaseTest
    {
        private IFileSystemConstants fileSystemConstants;
        private IFirewallExceptionHelper firewallHelper;
        private IWindowsService service;

        [TestInitialize]
        public void Initialize()
        {
            fileSystemConstants = Mocks.StrictMock<IFileSystemConstants>();
            firewallHelper = Mocks.StrictMock<IFirewallExceptionHelper>();
            service = Mocks.StrictMock<IWindowsService>();

            InternalObjects.Singletons.FirewallExceptionHelper.SetTestImplementation(firewallHelper);
            SystemObjects.Factories.WindowsService.EnqueueTestImplementation(service);
            InternalObjects.Singletons.FileSystemConstants.SetTestImplementation(fileSystemConstants);

            fileSystemConstants.Stub(x => x.ServicePath).Return("123");
        }

        [TestMethod]
        public void Install_New()
        {
            firewallHelper.Expect(x => x.AddExceptionsForCurrentProcess());
            service.Expect(x => x.Create(string.Empty)).IgnoreArguments();

            service.Stub(x => x.Description).PropertyBehavior();
            service.Stub(x => x.DisplayedName).PropertyBehavior();
            service.Stub(x => x.Exists).Return(false);
            service.Stub(x => x.Start(null)).IgnoreArguments();
            service.Stub(x => x.StartType).PropertyBehavior();

            Mocks.ReplayAll();

            var target = new ServiceHelper();

            target.Install();

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void Install_Exists()
        {
            firewallHelper.Expect(x => x.AddExceptionsForCurrentProcess());
            service.Expect(x => x.Create(string.Empty)).IgnoreArguments().Repeat.Never();

            service.Stub(x => x.Description).PropertyBehavior();
            service.Stub(x => x.DisplayedName).PropertyBehavior();
            service.Stub(x => x.Exists).Return(true);

            Mocks.ReplayAll();

            var target = new ServiceHelper();

            target.Install();

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void Uninstall_NoService()
        {
            firewallHelper.Expect(x => x.RemoveExceptionsForCurrentProcess());
            service.Expect(x => x.Delete()).Repeat.Never();

            service.Stub(x => x.Exists).Return(false);

            Mocks.ReplayAll();

            var target = new ServiceHelper();

            target.Uninstall();

            Mocks.VerifyAll();
        }

        [TestMethod]
        public void Uninstall_Exists()
        {
            firewallHelper.Expect(x => x.RemoveExceptionsForCurrentProcess());
            service.Expect(x => x.Delete());

            service.Stub(x => x.Exists).Return(true);
            service.Stub(x => x.State).Return(ServiceState.Running);

            service.Stub(x => x.State).Return(ServiceState.StopPending);
            service.Stub(x => x.State).Return(ServiceState.StopPending);

            service.Stub(x => x.State).Return(ServiceState.Stopped);

            service.Expect(x => x.QueryStop());//Если сервис запущен, то обязательно запланируем его остановку

            Mocks.ReplayAll();

            var target = new ServiceHelper();

            target.Uninstall();

            Mocks.VerifyAll();
        }
    }
}
