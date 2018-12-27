using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Devices.Management
{
    internal sealed class DeviceManagementHandle : Disposable
    {
        private readonly IntPtr deviceInformationSet;
        private readonly IntPtr machineHandle;

        internal IntPtr DeviceInformationSet
        {
            get
            {
                ThrowIfDisposed();
                return deviceInformationSet;
            }
        }

        internal IntPtr MachineHandle
        {
            get
            {
                ThrowIfDisposed();
                return machineHandle;
            }
        }
        
        internal DeviceManagementHandle(string machineName = null, bool empty = false)
        {
            string checkedMachineName = machineName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(checkedMachineName))
                checkedMachineName = @"\\" + checkedMachineName;

            if (empty)
            {
                deviceInformationSet = NativeMethods.SetupDiCreateDeviceInfoListEx(IntPtr.Zero, IntPtr.Zero, checkedMachineName, IntPtr.Zero);
                if (deviceInformationSet == NativeMethods.INVALID_HANDLE_VALUE)
                    NativeErrors.ThrowLastErrorException("SetupDiCreateDeviceInfoListEx", machineName);
            }
            else
            {
                uint flags = NativeMethods.DIGCF_ALLCLASSES;// | NativeMethods.DIGCF_DEVICEINTERFACE;

                deviceInformationSet = NativeMethods.SetupDiGetClassDevsEx(IntPtr.Zero, null, IntPtr.Zero, flags, IntPtr.Zero, machineName, IntPtr.Zero);
                if (deviceInformationSet == NativeMethods.INVALID_HANDLE_VALUE)
                    NativeErrors.ThrowLastErrorException("SetupDiGetClassDevsEx", machineName);
            }

            using (var handleBuffer = new UnmanagedPointer())
            {
                uint returnCode = NativeMethods.CM_Connect_Machine(checkedMachineName, +handleBuffer);
                if (returnCode != NativeMethods.CR_SUCCESS)
                    Exceptions.Throw(ErrorMessage.NetworkResourceNotAvailable, "CM_Connect_Machine failed with error {0}.".Combine(returnCode));

                machineHandle = handleBuffer;
            }
        }

        protected override void DisposeUnmanaged()
        {
            if ((deviceInformationSet != IntPtr.Zero) && (deviceInformationSet != NativeMethods.INVALID_HANDLE_VALUE) &&
                !NativeMethods.SetupDiDestroyDeviceInfoList(deviceInformationSet))
            {
                Log.Write(NativeErrors.GetLastErrorMessage("SetupDiDestroyDeviceInfoList"));
            }

            if (machineHandle != IntPtr.Zero)
            {
                uint returnCode = NativeMethods.CM_Disconnect_Machine(machineHandle);
                if (returnCode != NativeMethods.CR_SUCCESS)
                    Log.Write(NativeErrors.GetMessage("CM_Disconnect_Machine", returnCode));
            }
        }
    }
}
