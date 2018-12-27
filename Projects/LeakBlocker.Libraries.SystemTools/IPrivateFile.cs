using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.SystemTools
{
    /// <summary>
    /// File that is intended for access only by the current process. This is an alternative to the private
    /// registry storage but can be used for storing large ammounts of data.
    /// </summary>
    public interface IPrivateFile : IDisposable
    {
        /// <summary>
        /// Current file size. Setting this value will either append zero bytes to the end of the file or truncate the file.
        /// </summary>
        int Size
        {
            get;
            set;
        }

        /// <summary>
        /// Adds the binary block to the end of the file.
        /// </summary>
        /// <param name="data">Binary data that should be added.</param>
        void AppendData(byte[] data);

        /// <summary>
        /// Overwrites the file with the specified file.
        /// </summary>
        /// <param name="data">Binary data.</param>
        void Overwrite(byte[] data);

        /// <summary>
        /// Reads a bllock of data from the file.
        /// </summary>
        /// <param name="offset">Offset frrom the beginning of the file. If value is larger than file size then nnotning is read.</param>
        /// <param name="desiredSize">Required data size. If there is not enough data then actually read data size will be 
        /// lesser than this value.</param>
        /// <returns>Data block that was read from the file.</returns>
        byte[] ReadData(int offset = 0, int desiredSize = 0);

        /// <summary>
        /// Closes and deletes the file.
        /// </summary>
        void Delete();
    }
}
