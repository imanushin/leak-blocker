using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    internal static class NetworkTools
    {
        internal static bool CheckComputerAvailability(string name)
        {
            Check.StringIsMeaningful(name, "name");

            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(name, 139);
                    return true;
                }
                catch (Exception)
                {
                }
            }

            using (var client = new TcpClient())
            {
                try
                {
                    client.Connect(name, 445);
                    return true;
                }
                catch (Exception)
                {
                }
            }

            return false;
        }
    }
}
