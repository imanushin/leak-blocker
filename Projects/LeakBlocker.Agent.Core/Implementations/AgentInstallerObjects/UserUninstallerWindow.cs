using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Resources;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    internal sealed class UserUninstallerWindow : Form
    {
        private bool uninstallActive;
        private Button buttonUninstall = new Button();
        private TextBox textBoxPassword = new TextBox();
        private Label labelPassword = new Label();

        internal event Action<UserUninstallerWindow, string> UninstallRequest;

        internal UserUninstallerWindow()
        {
            SuspendLayout();

            buttonUninstall.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonUninstall.Location = new Point(257, 86);
            buttonUninstall.Size = new Size(75, 23);
            buttonUninstall.TabIndex = 0;
            buttonUninstall.UseVisualStyleBackColor = true;
            buttonUninstall.Enabled = false;
            buttonUninstall.Click += button1_Click;
            buttonUninstall.Text = AgentServiceStrings.UninstallButton;

            textBoxPassword.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBoxPassword.Location = new Point(12, 25);
            textBoxPassword.Multiline = true;
            textBoxPassword.Size = new Size(320, 55);
            textBoxPassword.TabIndex = 1;
            textBoxPassword.TextChanged += textBox1_TextChanged;
            textBoxPassword.PasswordChar = '*';

            labelPassword.AutoSize = true;
            labelPassword.Location = new Point(12, 9);
            labelPassword.Size = new Size(300, 13);
            labelPassword.TabIndex = 2;
            labelPassword.Text = AgentServiceStrings.PasswordPrompt;

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(344, 121);            
            MaximizeBox = false;
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = AgentServiceStrings.UninstallerHeader;

            Controls.Add(this.labelPassword);
            Controls.Add(this.textBoxPassword);
            Controls.Add(this.buttonUninstall);

            ResumeLayout(false);
            PerformLayout();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            buttonUninstall.Enabled = !uninstallActive && (textBoxPassword.TextLength > 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                uninstallActive = true;
                buttonUninstall.Enabled = false;
                labelPassword.Text = AgentServiceStrings.WaitMessage;
                textBoxPassword.Enabled = false;

                if (UninstallRequest != null)
                    UninstallRequest(this, textBoxPassword.Text);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show(AgentServiceStrings.PasswordIncorrect, AgentServiceStrings.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);

                labelPassword.Text = AgentServiceStrings.PasswordPrompt;
                textBoxPassword.Enabled = true;
                uninstallActive = false;
                buttonUninstall.Enabled = !uninstallActive && (textBoxPassword.TextLength > 0);
            }
            catch (Exception exception)
            {
                Log.Write(exception);

                MessageBox.Show(AgentServiceStrings.UnexpectedError, AgentServiceStrings.ErrorHeader, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            DialogResult result = MessageBox.Show(AgentServiceStrings.UninstallerWarning, AgentServiceStrings.WarningHeader, 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0);

            if (result == DialogResult.No)
                Close();
        }

        internal void InvokeClose()
        {
            if (InvokeRequired)
                Invoke((Action)InvokeClose);
            else
                Close();
        }
    }
}
