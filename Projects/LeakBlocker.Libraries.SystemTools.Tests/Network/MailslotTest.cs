using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LeakBlocker.Libraries.Common.Tests;
using LeakBlocker.Libraries.SystemTools.Network;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Libraries.SystemTools.Tests.Network
{
    [TestClass]
    public class MailslotTest : BaseTest
    {
        [TestMethod]
        public void MailslotTest1()
        {
            using (MailslotServer server = new MailslotServer("test_QWERTY", 123))
            using (MailslotClient client = new MailslotClient("test_QWERTY", 123))
            {
                byte[] received = null;
                byte[] sent = new byte[123];
                Encoding.Unicode.GetBytes("test").CopyTo(sent, 0);

                server.MessageReceived += data => received = data;
                client.SendMessage(sent);

                Thread.Sleep(1000);

                for (int i = 0; i < 123; i++)
                    Assert.AreEqual(sent[i], received[i]);
            }
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MailslotTest2()
        {
            new MailslotServer(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest3()
        {
            new MailslotServer("", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest4()
        {
            new MailslotServer("   ", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest5()
        {
            new MailslotServer("qwe", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest6()
        {
            new MailslotServer("qwe", -1);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MailslotTest7()
        {
            new MailslotClient(null, 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest8()
        {
            new MailslotClient("", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest9()
        {
            new MailslotClient("   ", 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest10()
        {
            new MailslotClient("qwe", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailslotTest11()
        {
            new MailslotClient("qwe", -1);
        }
    }
}
