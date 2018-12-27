using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.ResourceWrappers;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Entities.Management
{
    internal static class NameConversion
    {
        internal static string SimplifyUserName(string fullName)
        {
            Check.StringIsMeaningful(fullName, "fullName");

            int startIndex = 0;
            int endIndex = fullName.IndexOf('@');
            if (endIndex == -1)
            {
                startIndex = fullName.LastIndexOf('\\') + 1;
                endIndex = fullName.Length;
            }
            int length = endIndex - startIndex;

            if ((startIndex >= 0) && (startIndex < (fullName.Length - 1)) && ((startIndex + length) <= fullName.Length))
                return fullName.Substring(startIndex, length);
            return fullName;
        }

        internal static string GetUserDomainName(string fullName)
        {
            Check.StringIsMeaningful(fullName, "fullName");

            int startIndex = fullName.LastIndexOf('@') + 1;
            int endIndex = fullName.Length;
            if (startIndex == 0)
            {
                startIndex = 0;
                endIndex = fullName.IndexOf('\\');
                if (endIndex == -1)
                    endIndex = fullName.Length;
            }
            int length = endIndex - startIndex;

            if ((startIndex >= 0) && (startIndex < (fullName.Length - 1)) && ((startIndex + length) <= fullName.Length))
                return fullName.Substring(startIndex, length);
            return string.Empty;
        }

        internal static string ConvertToCanonicalName(string distinguishedName)
        {
            Check.StringIsMeaningful(distinguishedName, "distinguishedName");

            return ConvertName(IntPtr.Zero, distinguishedName, NativeMethods.DS_NAME_FORMAT.DS_FQDN_1779_NAME,
                NativeMethods.DS_NAME_FORMAT.DS_CANONICAL_NAME, NativeMethods.DS_NAME_FLAGS.DS_NAME_FLAG_SYNTACTICAL_ONLY);
        }

        private static string ConvertName(IntPtr handle, string source, NativeMethods.DS_NAME_FORMAT sourceFormat, NativeMethods.DS_NAME_FORMAT targetFormat, NativeMethods.DS_NAME_FLAGS flags)
        {
            string result = null;

            using (var sourceNameBuffer = new UnmanagedUnicodeString(source))
            using (var sourcePointersBuffer = new UnmanagedArray<IntPtr>(new[] { +sourceNameBuffer }))
            using (var resultPointerBuffer = new UnmanagedPointer())
            using (new DirectoryServiceNameMemoryWrapper(resultPointerBuffer))
            {
                uint error = NativeMethods.DsCrackNames(handle, flags, sourceFormat, targetFormat,
                    sourcePointersBuffer.ULength, +sourcePointersBuffer, +resultPointerBuffer);

                if (error != 0)
                    NativeErrors.ThrowException("DsCrackNames", error, source);

                using (var conversionResult = new UnmanagedStructure<NativeMethods.DS_NAME_RESULT>(resultPointerBuffer))
                using (var resultArray = new UnmanagedArray<NativeMethods.DS_NAME_RESULT_ITEM>(conversionResult.Value.rItems, conversionResult.Value.cItems))
                {
                    foreach (NativeMethods.DS_NAME_RESULT_ITEM currentItem in resultArray)
                    {
                        if (currentItem.status != (uint)NativeMethods.DS_NAME_ERROR.DS_NAME_NO_ERROR)
                            Log.Write("Name conversion error: {0}.", currentItem.status);
                        else
                            result = StringTools.FromPointer(currentItem.pName);
                    }
                }
            }

            Check.ObjectIsNotNull(result);

            return result;
        }
    }
}
