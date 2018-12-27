using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class PrivateFileImplementation : IPrivateFile
    {
        readonly List<byte> binaryData = new List<byte>();

        public int Size
        {
            get
            {
                return binaryData.Count();
            }
            set
            {
                while (value < binaryData.Count)
                    binaryData.RemoveAt(binaryData.Count - 1);

                while (value > binaryData.Count)
                    binaryData.Add(0);
            }
        }

        public void AppendData(byte[] data)
        {
            Check.ObjectIsNotNull(data);

            binaryData.AddRange(data);
        }

        public void Overwrite(byte[] data)
        {
            Check.ObjectIsNotNull(data);

            binaryData.Clear();
            binaryData.AddRange(data);
        }
         
        public byte[] ReadData(int offset = 0, int desiredSize = 0)
        {
            Check.IntegerIsNotLessThanZero(offset);
            Check.IntegerIsNotLessThanZero(desiredSize);

            if (desiredSize == 0)
                desiredSize = Math.Max(0, binaryData.Count - offset);

            return binaryData.Skip(offset).Take(desiredSize).ToArray();
        }

        public void Dispose()
        {
        }


        public void Delete()
        {
            //base.RegisterMethodCall("Delete");
        }
    }
}
