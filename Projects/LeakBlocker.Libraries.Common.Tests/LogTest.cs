using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LeakBlocker.Libraries.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.Common.Tests
{
    [TestClass]
    public sealed class LogTest
    {
        [TestMethod]
        public void Write_Text()
        {
            Log.Write(new Exception());
        }

        [TestMethod]
        public void Write_Exception()
        {
            Log.Write("some text");
        }

        [TestMethod]
        public void Write_stringFormat()
        {
            Log.Write("{0} {1}", 123, "542");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            try
            {
                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\Delta Corvi\Logs", true);
            }
            catch
            {
            }
        }
    }
}
