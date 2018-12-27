using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class UserUninstallMode : BaseInstallerMode
    {
        protected override IEnumerable<InstallerArgument> Arguments
        {
            get 
            {
                return ReadOnlySet<InstallerArgument>.Empty;
            }
        }

        protected override IEnumerable<InstallerCondition> Conditions
        {
            get 
            {
                yield return InstallerCondition.SameVersionIsInstalled;
            }
        }

        protected override void PerformActions()
        {
            Thread mainThread = new Thread(MainThread);
            mainThread.Start();
            mainThread.Join();
        }

        internal override void Start()
        {
            try
            {
                base.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.GetExceptionMessage(), AgentServiceStrings.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
                throw;
            }
        }

        [STAThread]
        private static void MainThread()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var window = new UserUninstallerWindow())
            {
                window.UninstallRequest += UninstallRequest;

                Application.Run(window);
            }
        }

        private static void UninstallRequest(UserUninstallerWindow window, string password)
        {
            if (!VerifyPassword(password))
                throw new UnauthorizedAccessException();

            Thread uninstallThread = new Thread(UninstallThread) { IsBackground = true };
            uninstallThread.Start(window);
        }

        private static void UninstallThread(object window)
        {
            using (ILocalControlClient controlClient = AgentObjects.CreateLocalControlClient())
            {
                controlClient.RequestUninstall(AgentObjects.AgentPrivateStorage.SecretKey);
            }

            ((UserUninstallerWindow)window).InvokeClose();
        }
    }
}
