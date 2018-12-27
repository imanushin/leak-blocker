using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.SystemTools.Drivers;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class DriverPackageImplementation : BaseTestImplementation, IDriverPackage
    {
        public void Install(bool legacy)
        {
            base.RegisterMethodCall("Install", legacy);
        }

        public void Remove()
        {
            base.RegisterMethodCall("Remove");
        }

        public void Dispose()
        {
        }
    }
}
