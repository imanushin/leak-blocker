﻿using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.Network.AdminView
{
    internal interface IAdminViewWcfSession
    {
        AccountSecurityIdentifier CurrentUserIdentifier
        {
            get;
        }
    }
}