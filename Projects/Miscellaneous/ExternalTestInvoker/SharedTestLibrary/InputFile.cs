using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;

namespace SharedTestLibrary
{
    [DataContract]
    public sealed class InputFile : BaseReadOnlyObject
    {
        [DataMember]
        public readonly string FileName;

        [DataMember]
        private readonly int fileStart;

        [DataMember]
        private readonly int fileLength;

        public byte[] GetFileEntry(byte[] totalBytes)
        {
            var result = new byte[fileLength];

            Array.Copy(totalBytes, fileStart, result, 0, fileLength);

            return result;
        }

        public InputFile(string fileName, int fileStart, int fileLength)
        {
            FileName = fileName;
            this.fileStart = fileStart;
            this.fileLength = fileLength;
        }

        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return FileName;
            yield return fileStart;
            yield return fileLength;
        }

        protected override string GetString()
        {
            return "Name: {0}; Size: {1}".Combine(FileName, fileLength);
        }
    }
}
