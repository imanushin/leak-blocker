using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LeakBlocker
{
    internal static class EntryPoint
    {
        private static volatile bool loaded;

        private sealed class SplashScreen : Form
        {
            private readonly System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle |= 0x00080000;
                    return cp;
                }
            }

            public SplashScreen()
            {
                FormBorderStyle = FormBorderStyle.None;
                BackgroundImage = LeakBlocker.AdminView.Resources.Logo;
                AutoScaleMode = AutoScaleMode.None;
                ClientSize = new Size(256, 256);
                ShowIcon = false;
                StartPosition = FormStartPosition.CenterScreen;
                TopMost = true;
                ShowInTaskbar = false;

                timer.Interval = 100;
                timer.Tick += delegate(object sender, EventArgs e)
                {
                    if (loaded)
                    {
                        timer.Enabled = false;
                        Close();
                    }
                };

                timer.Start();
            }

            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                timer.Enabled = false;
                timer.Dispose();
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                DrawForm();

                if (loaded)
                    Close();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                DrawForm();
            }

            private void DrawForm()
            {
                IntPtr screenDeviceContext = GetDC(IntPtr.Zero);
                IntPtr memoryDeviceContext = CreateCompatibleDC(screenDeviceContext);
                IntPtr bitmapHandle = IntPtr.Zero;
                IntPtr oldBitmap = IntPtr.Zero;

                try
                {
                    using (var bitmap = new Bitmap(BackgroundImage))
                    {
                        bitmapHandle = bitmap.GetHbitmap(Color.FromArgb(0));
                        oldBitmap = SelectObject(memoryDeviceContext, bitmapHandle);

                        Size size = bitmap.Size;
                        Point pointSource = Point.Empty;
                        var topPosition = new Point(Left, Top);

                        var blend = new BLENDFUNCTION
                        {
                            SourceConstantAlpha = 255,
                            AlphaFormat = 1
                        };

                        UpdateLayeredWindow(Handle, screenDeviceContext, ref topPosition, ref size, memoryDeviceContext, ref pointSource, 0, ref blend, 2);
                    }

                    ReleaseDC(IntPtr.Zero, screenDeviceContext);
                    if (bitmapHandle != IntPtr.Zero)
                    {
                        SelectObject(memoryDeviceContext, oldBitmap);
                        DeleteObject(bitmapHandle);
                    }
                    DeleteDC(memoryDeviceContext);
                }
                catch (Exception)
                {
                }
            }

            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            private struct BLENDFUNCTION
            {
                internal byte BlendOp;
                internal byte BlendFlags;
                internal byte SourceConstantAlpha;
                internal byte AlphaFormat;
            }

            [DllImport("user32.dll")]
            private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

            [DllImport("user32.dll")]
            private static extern IntPtr GetDC(IntPtr hWnd);

            [DllImport("gdi32.dll")]
            private static extern IntPtr CreateCompatibleDC(IntPtr hDC);

            [DllImport("user32.dll")]
            private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

            [DllImport("gdi32.dll")]
            private static extern bool DeleteDC(IntPtr hdc);

            [DllImport("gdi32.dll")]
            private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

            [DllImport("gdi32.dll")]
            private static extern bool DeleteObject(IntPtr hObject);
        }

        [STAThread]
        internal static int Main(string[] parameters)
        {
            var thread = new Thread((ThreadStart)delegate
            {
                using (var form = new SplashScreen())
                {
                    Application.Run(form);
                }
            });
            thread.IsBackground = true;

            return Program.Start(parameters, thread);
        }
    }
}
