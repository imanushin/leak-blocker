using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Network
{
    internal sealed class MailslotClient : Disposable, IMailslotClient
    {
        private readonly uint bufferSize;
        private readonly IntPtr handle;

        internal MailslotClient(string name, int messageSize)
        {
            Check.StringIsMeaningful(name, "name");
            Check.IntegerIsGreaterThanZero(messageSize, "messageSize");
    
            bufferSize = (uint)messageSize;

            string path = @"\\{2}\mailslot\{0}_{1}".Combine(name, messageSize, Environment.MachineName);

            handle = NativeMethods.CreateFile(path, NativeMethods.GENERIC_WRITE,
                NativeMethods.FILE_SHARE_READ, IntPtr.Zero, NativeMethods.OPEN_EXISTING, NativeMethods.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
            if (handle == NativeMethods.INVALID_HANDLE_VALUE)
                NativeErrors.ThrowLastErrorException("CreateFile", path);
        }

        public void SendMessage(byte[] data)
        {
            Check.CollectionIsNotNullOrEmpty(data, "data");

            if (data.Length > bufferSize)
                Exceptions.Throw(ErrorMessage.ObjectTooLarge);

            using (var buffer = new UnmanagedMemory(data))
            using (var writtenData = new UnmanagedInteger())
            {
                if(!NativeMethods.WriteFile(handle, +buffer, buffer.USize, +writtenData, IntPtr.Zero))
                    NativeErrors.ThrowLastErrorException("WriteFile");
            }
        }

        protected override void DisposeUnmanaged()
        {
            if (!NativeMethods.CloseHandle(handle))
                Log.Write(NativeErrors.GetLastErrorMessage("CloseHandle"));
        }
    }
}
