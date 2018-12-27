using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.AdminView.Desktop.Network
{
    internal static class LocalConnectionHelper
    {
        public static IPEndPoint Endpoint
        {
            get
            {
                IPAddress ipAddress = Dns.GetHostEntry(IPAddress.Loopback).AddressList.First();//Хак, чтобы избежать адресов 127.0.0.1 .. 127.255.255.254

                return new IPEndPoint(ipAddress, SharedObjects.Constants.DefaultTcpPort);
            }
        }
    }
}
