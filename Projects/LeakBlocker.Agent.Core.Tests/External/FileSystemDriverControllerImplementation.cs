using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Devices;
using LeakBlocker.Libraries.SystemTools.Drivers;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class FileSystemDriverControllerImplementation : BaseTestImplementation, IDriverController
    {
        public FileSystemDriverControllerImplementation(bool available)
        {
            Available = available;
        }

        public bool Available
        {
            get;
            private set;
        }

        public void SetManagedVolumes(IReadOnlyCollection<VolumeName> volumes)
        {
            base.RegisterMethodCall("SetManagedVolumes", volumes);
        }

        public void SetInstanceConfiguration(long instanceIdentifier, ReadOnlyDictionary<AccountSecurityIdentifier, FileAccessType> rules)
        {
            base.RegisterMethodCall("SetInstanceConfiguration", instanceIdentifier, rules);
        }

        public void Dispose()
        {
        }

        public void Install()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
