using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Implementations
{
    internal sealed class StackStorage : Disposable, IStackStorage
    {
        private readonly object synchronization = new object();
        private readonly IPrivateFile databaseFile;

        private volatile bool deleted;

        internal StackStorage()
        {
            Log.Write("Stack storage was initialized in abstract mode.");
        }

        internal StackStorage(string file)
        {
            Check.StringIsMeaningful(file, "file");

            databaseFile = SystemObjects.CreatePrivateFile(file);

            Log.Write("Stack storage was initialized.");
        }

        void IStackStorage.Write(IReadOnlyCollection<string> data)
        {
            Check.CollectionHasOnlyMeaningfulData(data, "data");

            Log.Add("Writing data to stack storage.");

            if (databaseFile == null)
            {
                Log.Add("Stack storage is not initialized.");
                return;
            }

            if(!data.Any())
            {
                Log.Add("No data for stack storage.");
                return;
            }

            if (deleted)
                Log.Write("Stack storage was deleted.");

            lock (synchronization)
            {
                string dataString = string.Join("\n", data.Select(Encoding.Unicode.GetBytes).Select(Convert.ToBase64String)) + '\n';

                byte[] binary = Encoding.ASCII.GetBytes(dataString);

                databaseFile.AppendData(binary);
            }
        }

        void IStackStorage.Read(int sizeLimit, Action<IReadOnlyCollection<string>> processingCallback)
        {
            Check.IntegerIsGreaterThanZero(sizeLimit, "sizeLimit");
            Check.ObjectIsNotNull(processingCallback, "processingCallback");

            using (new TimeMeasurement("Reading from stack storage"))
            {
                if (databaseFile == null)
                    return;

                if (deleted)
                    Log.Write("Stack storage was deleted.");

                lock (synchronization)
                {
                    if (databaseFile.Size == 0)
                    {
                        Log.Write("Storage is empty.");
                        processingCallback(ReadOnlySet<string>.Empty);
                        return;
                    }

                    int offset = Math.Max(0, databaseFile.Size - sizeLimit);
                    int size = Math.Min(sizeLimit, databaseFile.Size);

                    byte[] data = databaseFile.ReadData(offset, size);

                    string[] entries = Encoding.ASCII.GetString(data).Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    var result = new List<string>();

                    for (int i = (databaseFile.Size <= sizeLimit) ? 0 : 1; i < entries.Length; i++)
                        result.Add(Encoding.Unicode.GetString(Convert.FromBase64String(entries[i])));

                    int removedDataSize = (databaseFile.Size <= sizeLimit) ? size : (size - (entries[0].Length + 1));

                    Log.Write("{0} entries were read. Calling handler.".Combine(result.Count));
                    try
                    {
                        processingCallback(result.ToReadOnlySet());

                        databaseFile.Size = Math.Max(0, databaseFile.Size - removedDataSize);
                        Log.Write("New file size is {0}".Combine(databaseFile.Size));
                    }
                    catch (Exception exception)
                    {
                        Log.Write(exception);
                    }
                }
            }
        }

        void IStackStorage.Delete()
        {
            lock (synchronization)
            {
                Log.Write("Deleting stack storage.");
                databaseFile.Delete();
                deleted = true;
            }
        }

        protected override void DisposeManaged()
        {
            if (databaseFile != null)
                DisposeSafe(databaseFile);

            Log.Write("Stack storage was closed.");
        }
    }
}
