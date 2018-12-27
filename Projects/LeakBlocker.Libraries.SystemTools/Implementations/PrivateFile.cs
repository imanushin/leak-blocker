using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class PrivateFile : Disposable, IPrivateFile
    {
        private readonly FileStream databaseFile;
        private readonly object synchronization = new object();
        private readonly string path;

        private bool deleted;

        int IPrivateFile.Size
        {
            get
            {
                ThrowIfRemoved();

                lock (synchronization)
                {
                    return (int)databaseFile.Length;
                }
            }
            set
            {
                Check.IntegerIsNotLessThanZero(value);

                ThrowIfRemoved();

                lock (synchronization)
                {
                    databaseFile.SetLength(value);
                }
            }
        }

        internal PrivateFile(string path)
        {
            Check.StringIsMeaningful(path, "path");

            string folder = Path.GetDirectoryName(path);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            databaseFile = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            this.path = path;
        }

        void IPrivateFile.AppendData(byte[] data)
        {
            Check.ObjectIsNotNull(data, "data");

            if (data.Length == 0)
                return;

            ThrowIfRemoved();

            lock (synchronization)
            {
                databaseFile.Position = databaseFile.Length;
                databaseFile.SetLength(databaseFile.Length + data.Length);
                databaseFile.Write(data, 0, data.Length);

                databaseFile.Flush(true);
            }
        }

        void IPrivateFile.Overwrite(byte[] data)
        {
            Check.ObjectIsNotNull(data, "data");

            ThrowIfRemoved();

            lock (synchronization)
            {
                databaseFile.SetLength(0);

                if (data.Length == 0)
                    return;

                databaseFile.Position = 0;
                databaseFile.SetLength(data.Length);
                databaseFile.Write(data, 0, data.Length);

                databaseFile.Flush(true);
            }
        }

        byte[] IPrivateFile.ReadData(int offset, int desiredSize)
        {
            Check.IntegerIsNotLessThanZero(offset, "offset");
            Check.IntegerIsNotLessThanZero(desiredSize, "desiredSize");

            ThrowIfRemoved();

            var size = (int)Math.Min((desiredSize == 0) ? databaseFile.Length : desiredSize, databaseFile.Length - offset);
            if (size == 0)
                return new byte[0];

            lock (synchronization)
            {
                var data = new byte[size];
                databaseFile.Position = offset;

                databaseFile.Read(data, 0, data.Length);

                return data;
            }
        }

        void IPrivateFile.Delete()
        {
            lock (synchronization)
            {
                if (deleted)
                    return;

                databaseFile.Dispose();
                SystemObjects.FileTools.Delete(path);
                deleted = true;
            }
        }

        protected override void DisposeManaged()
        {
            DisposeSafe(databaseFile);
        }

        private void ThrowIfRemoved()
        {
            lock (synchronization)
            {
                if (deleted)
                    Exceptions.Throw(ErrorMessage.FileNotFound, "File was removed.");
            }
        }
    }
}
