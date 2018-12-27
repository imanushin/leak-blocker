using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.ServiceProcess;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Server.Service.Engine
{
    [ExcludeFromCodeCoverage]
    internal partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MainInvoker.Start();
        }

        protected override void OnStop()
        {
            MainInvoker.Stop();
        }
    }
}
