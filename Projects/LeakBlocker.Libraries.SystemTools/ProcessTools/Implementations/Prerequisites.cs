using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.SystemTools.Entities.Management;
using LeakBlocker.Libraries.SystemTools.Network;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;
using Microsoft.Win32;

namespace LeakBlocker.Libraries.SystemTools.ProcessTools.Implementations
{
    internal sealed class Prerequisites : IPrerequisites
    {
        bool IPrerequisites.ComputerIsAccessible(string name)
        {
            Check.StringIsMeaningful(name, "name");

            bool result = false;

            SharedObjects.ExceptionSuppressor.Run(() => result = NetworkTools.CheckComputerAvailability(name));

            return result;
        }

        Version IPrerequisites.GetRemoteSystemVersion(SystemAccessOptions options)
        {
            return ComputerInformation.GetComputerSystemVersion(options.SystemName, options);
        }

        bool IPrerequisites.FirewallIsActive(string name)
        {
            Check.StringIsMeaningful(name, "name");

            return !AuthenticatedConnection.CanBeEstablished(name);
        }
    }
}
