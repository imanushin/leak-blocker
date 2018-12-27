using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    internal sealed class MailslotServer : Disposable, IMailslotServer
    {
        private readonly uint bufferSize;
        private readonly IntPtr handle;
        private readonly Thread thread;

        public event Action<byte[]> MessageReceived;

        internal MailslotServer(string name, int messageSize)
        {
            Check.StringIsMeaningful(name, "name");
            Check.IntegerIsGreaterThanZero(messageSize, "messageSize");

            bufferSize = (uint)messageSize;

            string path = @"\\.\mailslot\{0}_{1}".Combine(name, messageSize);

            handle = NativeMethods.CreateMailslot(path, bufferSize, NativeMethods.MAILSLOT_WAIT_FOREVER, IntPtr.Zero);
            if (handle == NativeMethods.INVALID_HANDLE_VALUE)
                NativeErrors.ThrowLastErrorException("CreateMailslot");
                    
            thread = new Thread(MainLoop) { IsBackground = true };
            thread.Start();
        }

        private void MainLoop()
        {
            SharedObjects.ExceptionSuppressor.Run(delegate
            {
                while (true)
                {
                    using (var buffer = new UnmanagedMemory(bufferSize))
                    using (var readData = new UnmanagedInteger())
                    {
                        if (Disposed)
                            break;

                        if (!NativeMethods.ReadFile(handle, +buffer, buffer.USize, +readData, IntPtr.Zero))
                        {
                            uint error = NativeMethods.GetLastError();
                            if (error != NativeMethods.ERROR_HANDLE_EOF)
                                NativeErrors.ThrowException("ReadFile", error);
                        }

                        if ((readData > 0) && (MessageReceived != null))
                            SharedObjects.ExceptionSuppressor.Run(MessageReceived, buffer.BinaryData);
                    }
                }
            });
        }

        protected override void DisposeUnmanaged()
        {
            if (!NativeMethods.CloseHandle(handle))
                Log.Write(NativeErrors.GetLastErrorMessage("CloseHandle"));
            thread.Join();
        }
    }
}
