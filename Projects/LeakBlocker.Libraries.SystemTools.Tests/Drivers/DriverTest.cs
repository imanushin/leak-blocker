using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Drivers;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests.Drivers
{
    [TestClass]
    public class DriverTest
    {
        public void TestMethod()
        {
            IEnumerable<AccountSecurityIdentifier> users = LocalUserSession.EnumerateLocalSessions().Select(session => new AccountSecurityIdentifier(session.UserIdentifier));
            ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> settings = users.ToReadOnlyDictionary(user => user, user => FileAccessType.ReadOnly);

            IDriverController controller = null;
            var handler = new Handler();
            handler.Attached += identifier => controller.SetInstanceConfiguration(identifier, settings);
            ((IDriverController)new DriverController()).Install();
            controller = new DriverController(handler);
            using (controller)
            {
              


            } 
            ((IDriverController)new DriverController()).Remove();
        }

        private sealed class Handler : IDriverControllerHandler
        {
            public event Action<long> Attached;

            public void VolumeAttachMessageReceived(IVolumeAttachMessage message)
            {
                Attached(message.InstanceIdentifier);
            }

            public void VolumeDetachMessageReceived(IVolumeDetachMessage message)
            {
            }

            public void VolumeListUpdateMessageReceived()
            {
            }

            public void FileNotificationMessageReceived(IFileNotificationMessage message)
            {
              //  message.


        //string FileName
        //VolumeName Volume
        //AccountSecurityIdentifier UserIdentifier
        //bool Read
        //bool Write
        //bool Delete
        //bool Directory
        //Time SystemTime
        //FileAccessType AppliedAction
        //string ProcessName
        //FileActionType ResultAction


                

            }
        }
    }
}
