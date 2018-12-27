using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Network;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class FileTools : IFileTools
    {
        public void RemoveCurrentExecutable()
        {
            string toolPath = SharedObjects.Constants.TemporaryFolder + Guid.NewGuid() + ".exe";

            File.WriteAllBytes(toolPath, Binaries.CleanupTool);
            Process.Start(new ProcessStartInfo
            {
                FileName = toolPath,
                WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = '\"' + SharedObjects.Constants.MainModulePath + '\"'
            });
        }

        public void CreateDirectory(string folder, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(folder, "folder");

            using (new AuthenticatedConnection(options))
            {
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
            }
        }

        public void CopyFile(string source, string destination, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(source, "source");
            Check.StringIsMeaningful(destination, "destination");

            using (new AuthenticatedConnection(options))
            {
                string targetFolder = Path.GetDirectoryName(destination);
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                File.Copy(source, destination, true);
            }
        }

        public bool Exists(string path, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(path, "path");

            using (new AuthenticatedConnection(options))
            {
                return File.Exists(path);
            }
        }

        public void WriteFile(string path, IReadOnlyCollection<byte> data, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(path, "path");
            Check.ObjectIsNotNull(data, "data");

            using (new AuthenticatedConnection(options))
            {
                File.WriteAllBytes(path, data.ToArray());
            }
        }

        public IReadOnlyCollection<byte> ReadFile(string path, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(path, "path");

            using (new AuthenticatedConnection(options))
            {
                return File.ReadAllBytes(path).ToReadOnlyList();
            }
        }

        public void Delete(string path, SystemAccessOptions options)
        {
            Check.StringIsMeaningful(path, "path");

            using (new AuthenticatedConnection(options))
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}
