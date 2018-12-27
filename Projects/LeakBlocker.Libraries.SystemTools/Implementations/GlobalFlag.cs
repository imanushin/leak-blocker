using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class GlobalFlag : IGlobalFlag
    {
        private readonly string name;

        bool IGlobalFlag.Exists
        {
            get
            {
                if (NativeMethods.GlobalFindAtom(name) == 0)
                {
                    if (NativeMethods.GetLastError() == NativeMethods.ERROR_FILE_NOT_FOUND)
                        return false;
                    NativeErrors.ThrowLastErrorException("GlobalFindAtom", name);
                }
                return true;
            }
        }

        internal GlobalFlag(string name)
        {
            Check.ObjectIsNotNull(name, "name");

            this.name = name;
        }

        void IGlobalFlag.Create()
        {
            if (NativeMethods.GlobalAddAtom(name) == 0)
                NativeErrors.ThrowLastErrorException("GlobalAddAtom", name);
        }

        void IGlobalFlag.Delete()
        {
            ushort atom = NativeMethods.GlobalFindAtom(name);
            
            if (atom == 0)
                NativeErrors.ThrowLastErrorException("GlobalFindAtom", name);

            NativeMethods.SetLastError(NativeMethods.ERROR_SUCCESS);
            NativeMethods.GlobalDeleteAtom(atom);
            uint error = NativeMethods.GetLastError();
            if(error != NativeMethods.ERROR_SUCCESS)
                NativeErrors.ThrowException("GlobalDeleteAtom", error, name);
        }
    }
}
