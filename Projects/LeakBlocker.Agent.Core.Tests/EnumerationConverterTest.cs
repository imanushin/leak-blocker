using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.SystemTools.Drivers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LeakBlocker.Agent.Core.Tests
{
    [TestClass]
    public class EnumerationConverterTest
    {
        [TestMethod]
        public void EnumerationConverterTest7()
        {
            Dictionary<DeviceAccessType, FileAccessType> values2 = new Dictionary<DeviceAccessType, FileAccessType>
            {
                { default(DeviceAccessType), FileAccessType.Allow },
                { DeviceAccessType.TemporarilyAllowed, FileAccessType.Allow },
                { DeviceAccessType.Allowed, FileAccessType.Allow },
                { DeviceAccessType.TemporarilyAllowedReadOnly, FileAccessType.ReadOnly },
                { DeviceAccessType.ReadOnly, FileAccessType.ReadOnly },
                { DeviceAccessType.Blocked, FileAccessType.Block },
                { DeviceAccessType.AllowedNotLicensed, FileAccessType.Allow },
            };

            foreach (DeviceAccessType currentItem in EnumHelper<DeviceAccessType>.Values)
                Assert.AreEqual(values2[currentItem], EnumerationConverter.GetFileAccessType(currentItem));
            
            Dictionary<long, AgentFileActionType> values4 = new Dictionary<long, AgentFileActionType>()
            {
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.ReadAllowed },
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.ReadAllowed },
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.ReadAllowed },
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.ReadTemporarilyAllowed },
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.ReadTemporarilyAllowed },
                { (long)FileActionType.ReadAllowed | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.ReadAllowed },
                                  
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.ReadBlocked },
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.ReadBlocked },
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.ReadBlocked },
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.ReadBlocked },
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.ReadBlocked },
                { (long)FileActionType.ReadBlocked | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.ReadBlocked },
                 
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.ReadWriteAllowed },
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.ReadWriteAllowed },
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.ReadWriteAllowed },
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.ReadWriteTemporarilyAllowed },
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.ReadWriteTemporarilyAllowed },
                { (long)FileActionType.ReadWriteAllowed | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.ReadWriteAllowed },
                                                
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.ReadWriteBlocked },
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.ReadWriteBlocked },
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.ReadWriteBlocked },
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.ReadWriteBlocked },
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.ReadWriteBlocked },
                { (long)FileActionType.ReadWriteBlocked | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.ReadWriteBlocked },
             
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.WriteAllowed },
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.WriteAllowed },
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.WriteAllowed },
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.WriteTemporarilyAllowed },
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.WriteTemporarilyAllowed },
                { (long)FileActionType.WriteAllowed | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.WriteAllowed },
                                          
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.WriteBlocked },
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.WriteBlocked },
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.WriteBlocked },
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.WriteBlocked },
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.WriteBlocked },
                { (long)FileActionType.WriteBlocked | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.WriteBlocked },
            
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.Allowed << 32), AgentFileActionType.Unknown },
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.Blocked << 32), AgentFileActionType.Unknown },
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.ReadOnly << 32), AgentFileActionType.Unknown },
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.TemporarilyAllowed << 32), AgentFileActionType.Unknown },
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.TemporarilyAllowedReadOnly << 32), AgentFileActionType.Unknown }, 
                { (long)FileActionType.Unknown | ((long)DeviceAccessType.AllowedNotLicensed << 32), AgentFileActionType.Unknown },                
            };

            foreach (FileActionType currentItem1 in EnumHelper<FileActionType>.Values)
                foreach (DeviceAccessType currentItem2 in EnumHelper<DeviceAccessType>.Values)
                    Assert.AreEqual(values4[(uint)currentItem1 | ((long)currentItem2 << 32)], EnumerationConverter.GetAgentFileActionType(currentItem1, currentItem2));
        }
    }
}
