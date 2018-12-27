using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class FileToolsImplementation : IFileTools
    {
        readonly Dictionary<string, string> files = new Dictionary<string, string>();
        public bool Throw;
        readonly Dictionary<string, IReadOnlyCollection<byte>> files2 = new Dictionary<string, IReadOnlyCollection<byte>>();
        
        public Dictionary<string, string> Files
        {
            get
            {
                return files;
            }
        }

        public void RemoveCurrentExecutable()
        {
        }

        public void CreateDirectory(string folder, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(folder);
        }

        public void CopyFile(string source, string destination, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(source);
            Check.StringIsMeaningful(destination);

            if (Throw)
                throw new InvalidOperationException();
        }

        public string ReadTextFile(string path, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(path);

            if (Throw)
                throw new InvalidOperationException();

            return files[path.ToUpperInvariant()];
        }

        public void WriteTextFile(string path, string data, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(path);
            Check.StringIsMeaningful(data);

            if (Throw)
                throw new InvalidOperationException();

            files[path.ToUpperInvariant()] = data;
        }

        public bool Exists(string path, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(path);

            return (Files.ContainsKey(path.ToUpperInvariant()) && Files[path.ToUpperInvariant()] != null) ||
                (files2.ContainsKey(path.ToUpperInvariant()) && files2[path.ToUpperInvariant()].Count > 0);
        }


        public void WriteFile(string path, IReadOnlyCollection<byte> data, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(path);
            Check.ObjectIsNotNull(data);

            if (Throw)
                throw new InvalidOperationException();

            files2[path.ToUpperInvariant()] = data;
        }

        public void Delete(string path, SystemAccessOptions options = default(SystemAccessOptions))
        {
        }

        public IReadOnlyCollection<byte> ReadFile(string path, SystemAccessOptions options = default(SystemAccessOptions))
        {
            Check.StringIsMeaningful(path);

            if (Throw)
                throw new InvalidOperationException();

            return files2[path.ToUpperInvariant()];
        }
    }
}
