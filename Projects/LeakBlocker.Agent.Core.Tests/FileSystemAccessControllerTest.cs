using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LeakBlocker.Agent.Core.Implementations;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.Common.Tests.Entities;
using LeakBlocker.Libraries.Common.Tests.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class FileSystemAccessControllerTest : BaseTest
    {
        private static VolumeName CreateVolumeName(string name = null)
        {
            VolumeName result = (VolumeName)FormatterServices.GetUninitializedObject(typeof(VolumeName));
            typeof(VolumeName).GetField("name", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(result, name ?? @"\Device\HarddiskVolume1");
            return result;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest1()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.AddDriverInstance(null, 123);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest2()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.CheckMonitoredVolume(null, AccountSecurityIdentifierTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest3()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.CheckMonitoredVolume(CreateVolumeName(), null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest4()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetConfiguredAccessType(null, AccountSecurityIdentifierTest.First);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest5()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetConfiguredAccessType(CreateVolumeName(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest6()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetDevice(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FileSystemAccessControllerTest7()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession();
            target.GetUpdateSession();
        }

        [TestMethod]
        public void FileSystemAccessControllerTest8()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().Dispose();
            target.GetUpdateSession();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest9()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAccessRule(null, CreateVolumeName(), AccountSecurityIdentifierTest.First, DeviceAccessType.Allowed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest9a()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAuditRule(null, CreateVolumeName(), AccountSecurityIdentifierTest.First, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest10()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAccessRule(DeviceDescriptionTest.First, null, AccountSecurityIdentifierTest.First, DeviceAccessType.Allowed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest10a()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAuditRule(DeviceDescriptionTest.First, null, AccountSecurityIdentifierTest.First, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest11()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAccessRule(DeviceDescriptionTest.First, CreateVolumeName(), null, DeviceAccessType.Allowed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FileSystemAccessControllerTest11a()
        {
            IFileSystemAccessController target = new FileSystemAccessController();

            target.GetUpdateSession().SetAuditRule(DeviceDescriptionTest.First, CreateVolumeName(), null, true);
        }

        [TestMethod]
        public void FileSystemAccessControllerTest12()
        {
            VolumeName volume = CreateVolumeName();
            AccountSecurityIdentifier user = AccountSecurityIdentifierTest.First;
            DeviceDescription device = DeviceDescriptionTest.First;

            IFileSystemAccessController target = new FileSystemAccessController();

            Assert.IsFalse(target.CheckMonitoredVolume(volume, user));

            using (IFileSystemAccessControllerUpdateSession session = target.GetUpdateSession())
            {
                session.SetAccessRule(device, volume, user, DeviceAccessType.ReadOnly);
                session.SetAuditRule(device, volume, user, true);
            }

            Assert.IsTrue(target.Volumes.Contains(volume));
            Assert.AreEqual(1, target.Volumes.Count);

            Assert.IsTrue(target.CheckMonitoredVolume(volume, user));
            Assert.IsFalse(target.CheckMonitoredVolume(volume, AccountSecurityIdentifierTest.Second));
            Assert.AreEqual(target.GetConfiguredAccessType(volume, user), DeviceAccessType.ReadOnly);

            Assert.AreEqual(target.GetDevice(volume), device);
            Assert.AreEqual(target.GetDevice(CreateVolumeName("test")), null);

            Assert.AreEqual(target.GetDriverInstanceConfiguration(123).Count, 0);

            target.AddDriverInstance(CreateVolumeName("test"), 123);
            Assert.AreEqual(target.GetDriverInstanceConfiguration(123).Count, 0);

            target.AddDriverInstance(volume, 234);
            Assert.AreEqual(target.GetDriverInstanceConfiguration(234).Count, 1);

            Assert.AreEqual(target.GetDriverInstanceConfiguration(234).First().Key, user);
            Assert.AreEqual(target.GetDriverInstanceConfiguration(234).First().Value, DeviceAccessType.ReadOnly);

            target.RemoveDriverInstance(234);
            Assert.AreEqual(target.GetDriverInstanceConfiguration(234).Count, 0);

            target.GetUpdateSession().Dispose();

            Assert.AreEqual(target.GetConfiguredAccessType(volume, user), DeviceAccessType.Allowed);
            Assert.IsFalse(target.CheckMonitoredVolume(volume, user));
            Assert.AreEqual(target.GetDriverInstanceConfiguration(234).Count, 0);
        }

        [TestMethod]
        public void FileSystemAccessControllerTest13()
        {
            VolumeName volume = CreateVolumeName();
            AccountSecurityIdentifier user = AccountSecurityIdentifierTest.First;
            DeviceDescription device = DeviceDescriptionTest.First;

            IFileSystemAccessController target = new FileSystemAccessController();

            Assert.IsFalse(target.CheckMonitoredVolume(volume, user));

            using (IFileSystemAccessControllerUpdateSession session = target.GetUpdateSession())
            {
                session.SetAuditRule(device, CreateVolumeName("1dwfewggewge"), AccountSecurityIdentifierTest.Second, true);
                session.SetAuditRule(device, volume, AccountSecurityIdentifierTest.First, false);
                session.SetAuditRule(device, volume, AccountSecurityIdentifierTest.Second, true);
            }

            Assert.IsTrue(target.CheckMonitoredVolume(volume, AccountSecurityIdentifierTest.Second));
            Assert.IsFalse(target.CheckMonitoredVolume(volume, AccountSecurityIdentifierTest.First));
        }
    }
}
