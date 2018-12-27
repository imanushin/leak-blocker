using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.SystemTools.Win32;
using LeakBlocker.Libraries.SystemTools.Win32.UnmanagedTypes;

namespace LeakBlocker.Libraries.SystemTools.Implementations
{
    internal sealed class PrivateRegistryStorage : IPrivateRegistryStorage
    {
        #region SetupInformationFile

        private sealed class SetupInformationFile : Disposable
        {
            private const string template = @"
[Version]                                                          
Signature=""$Windows NT$""                                           
Class=ServiceSetupManagement                                            
ClassGuid={{{0}}}                                                                         
[ClassInstall32]                                                   
AddReg=SampleClass_RegistryAdd                                                                                 
[SampleClass_RegistryAdd]                                          
HKR,,,,""ServiceSetupManagement""                                               
HKR,,Icon,,""-10""    
HKR,,SilentInstall,,1
HKR,,NoInstallClass,,1
HKR,,NoDisplayClass,,1
HKR,,NoUseClass,,1
";

            private readonly string path;

            internal SetupInformationFile(Guid classIdentifier)
            {
                path = SharedObjects.Constants.TemporaryFolder + "ServiceSetupManagement_" + classIdentifier + ".inf";
                File.WriteAllText(path, template.Combine(classIdentifier), Encoding.ASCII);
            }

            protected override void DisposeManaged()
            {
                File.Delete(path);
            }

            public static implicit operator string(SetupInformationFile value)
            {
                Check.ObjectIsNotNull(value, "value");

                value.ThrowIfDisposed();
                return value.path;
            }
        }

        #endregion SetupInformationFile

        private readonly Guid identifier;
        private readonly Dictionary<string, string> data = new Dictionary<string, string>();
        private readonly DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, string>));
        private static readonly object synchronization = new object();

        internal PrivateRegistryStorage(Guid identifier)
        {
            this.identifier = identifier;
        }

        Time IPrivateRegistryStorage.InstallDate
        {
            get
            {
                return GetInstallDate(identifier);
            }
        }

        string IPrivateRegistryStorage.GetValue(string key)
        {
            Check.StringIsMeaningful(key, "key");

            using (new ExceptionNotifier("Requesting property {0} from private storage", key))
            {
                lock (synchronization)
                {
                    Load();
                    return !data.ContainsKey(key) ? string.Empty : data[key];
                }
            }
        }

        void IPrivateRegistryStorage.SetValue(string key, string value)
        {
            Check.StringIsMeaningful(key, "key");

            using (new ExceptionNotifier("Updating property {0} in private storage", key))
            {
                lock (synchronization)
                {
                    if (value == null)
                        data.Remove(key);
                    else
                        data[key] = value;
                    Save();
                }
            }
        }

        void IPrivateRegistryStorage.DeleteValue(string key)
        {
            Check.StringIsMeaningful(key, "key");

            using (new ExceptionNotifier("Removing property {0} from private storage", key))
            {
                lock (synchronization)
                {
                    data.Remove(key);
                    Save();
                }
            }
        }

        private void Save()
        {
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, data);

                var binaryData = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(binaryData, 0, (int)stream.Length);

                string stringData = Convert.ToBase64String(binaryData);

                SaveData(identifier, stringData);
            }
        }

        private void Load()
        {
            string stringData = LoadData(identifier);

            if (string.IsNullOrWhiteSpace(stringData))
                return;

            try
            {
                using (var stream = new MemoryStream(Convert.FromBase64String(stringData)))
                {
                    var newData = (Dictionary<string, string>)serializer.ReadObject(stream);

                    data.Clear();
                    data.AddRange(newData);
                }
            }
            catch (Exception exception)
            {
                Log.Write(exception);
            }
        }

        private static string LoadData(Guid identifier)
        {
            CreateIfRequired(identifier);

            using (var classGuid = new UnmanagedStructure<Guid>(identifier))
            using (var requiredSize = new UnmanagedInteger())
            {
                if (!NativeMethods.SetupDiGetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_SECURITY_SDS,
                    IntPtr.Zero, IntPtr.Zero, 0, +requiredSize, null, IntPtr.Zero))
                {
                    uint error = NativeMethods.GetLastError();

                    if (error == NativeMethods.ERROR_INVALID_DATA)
                        return string.Empty;

                    if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                        NativeErrors.ThrowLastErrorException("SetupDiGetClassRegistryProperty", identifier);
                }

                if (requiredSize == 0)
                    return string.Empty;

                using (var buffer = new UnmanagedMemory(requiredSize))
                using (var result = new UnmanagedUnicodeString(+buffer, buffer.USize / 2))
                {
                    if (!NativeMethods.SetupDiGetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_SECURITY_SDS,
                        IntPtr.Zero, +buffer, buffer.USize, +requiredSize, null, IntPtr.Zero))
                    {
                        NativeErrors.ThrowLastErrorException("SetupDiGetClassRegistryProperty", identifier);
                    }

                    return UnpackData(result.Value);
                }
            }
        }

        private static void SaveData(Guid identifier, string data)
        {
            Check.ObjectIsNotNull(data, "data");

            string packedData = PackData(data);

            CreateIfRequired(identifier);

            using (var classGuid = new UnmanagedStructure<Guid>(identifier))
            using (var value = new UnmanagedUnicodeString(packedData))
            {
                if (!NativeMethods.SetupDiSetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_SECURITY_SDS, +value, value.USize, null, IntPtr.Zero))
                    NativeErrors.ThrowLastErrorException("SetupDiSetClassRegistryProperty", identifier);
            }
        }

        private static void CreateIfRequired(Guid identifier)
        {
            using (var classGuid = new UnmanagedStructure<Guid>(identifier))
            {
                IntPtr key = NativeMethods.SetupDiOpenClassRegKey(+classGuid, NativeMethods.KEY_ENUMERATE_SUB_KEYS);
                if (key == NativeMethods.INVALID_HANDLE_VALUE)
                {
                    uint error = NativeMethods.GetLastError();
                    if (error != NativeMethods.ERROR_INVALID_CLASS)
                        NativeErrors.ThrowException("SetupDiOpenClassRegKey", error, identifier);

                    using (var file = new SetupInformationFile(identifier))
                    {
                        if (!NativeMethods.SetupDiInstallClass(IntPtr.Zero, file, 0, IntPtr.Zero))
                            NativeErrors.ThrowLastErrorException("SetupDiInstallClass", identifier);

                        SetInstallDateIfRequired(identifier);
                    }
                }
                else
                {
                    int error = NativeMethods.RegCloseKey(key);
                    if (error != NativeMethods.ERROR_SUCCESS)
                        Log.Write(NativeErrors.GetMessage("RegCloseKey", unchecked((uint)error)));
                }
            }
        }

        private static Time GetInstallDate(Guid identifier)
        {
            using (var classGuid = new UnmanagedStructure<Guid>(identifier))
            using (var value = new UnmanagedInteger())
            {
                if (!NativeMethods.SetupDiGetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_DEVTYPE,
                    IntPtr.Zero, +value, value.USize, IntPtr.Zero, null, IntPtr.Zero))
                {
                    NativeErrors.ThrowLastErrorException("SetupDiGetClassRegistryProperty", identifier);
                }

                int day = ((value - 32768) >> 10) & 31;
                int month = ((value - 32768) >> 6) & 15;
                int year = ((value - 32768) & 63) + 2000;

                return new Time(new DateTime(year, month, day));
            }
        }

        private static void SetInstallDateIfRequired(Guid identifier)
        {
            using (var classGuid = new UnmanagedStructure<Guid>(identifier))
            using (var value = new UnmanagedInteger())
            using (var requiredSize = new UnmanagedInteger())
            {
                if (!NativeMethods.SetupDiGetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_DEVTYPE,
                    IntPtr.Zero, IntPtr.Zero, 0, +requiredSize, null, IntPtr.Zero))
                {
                    uint error = NativeMethods.GetLastError();
                    if (error == NativeMethods.ERROR_INVALID_DATA)
                    {
                        var day = unchecked((byte)DateTime.UtcNow.Day);
                        var month = unchecked((byte)DateTime.UtcNow.Month);
                        var year = unchecked((byte)(DateTime.UtcNow.Year - 2000));

                        value.SValue = 32768 + ((day & 31) << 10) | ((month & 15) << 6) | (year & 63);

                        if (!NativeMethods.SetupDiSetClassRegistryProperty(+classGuid, NativeMethods.SPCRP_DEVTYPE, +value, value.USize, null, IntPtr.Zero))
                            NativeErrors.ThrowLastErrorException("SetupDiSetClassRegistryProperty", identifier);
                    }
                    else if ((error != NativeMethods.ERROR_SUCCESS) && (error != NativeMethods.ERROR_INSUFFICIENT_BUFFER))
                        NativeErrors.ThrowLastErrorException("SetupDiGetClassRegistryProperty", identifier);
                }
            }
        }

        private static string PackData(string data)
        {
            Check.ObjectIsNotNull(data, "data");

            byte[] stringData = Encoding.Unicode.GetBytes(data);

            var binary = new byte[4 + stringData.Length + (4 - (stringData.Length % 4))];

            Check.IntegerIsNotLessThanZero(-(binary.Length % 4));

            BitConverter.GetBytes(stringData.Length).CopyTo(binary, 0);
            stringData.CopyTo(binary, 4);

            var parts = new List<string>();
            for (int i = 0; i < binary.Length / 4; i++)
            {
                uint value = BitConverter.ToUInt32(binary, i * 4);
                parts.Add("-" + value.ToString(CultureInfo.InvariantCulture));
            }
            int padding = (13 - (parts.Count % 13));
            for (int i = 0; i < padding; i++)
                parts.Add("-0");

            Check.IntegerIsNotLessThanZero(-(parts.Count % 13));

            string result = "D:";

            for (int i = 0; i < (parts.Count / 13); i++)
            {
                string currentEntry = "(D;;;;;S-1-281474976710655-" + i.ToString(CultureInfo.InvariantCulture);

                for (int j = 0; j < 13; j++)
                    currentEntry += parts[i * 13 + j];

                currentEntry += ")";
                result += currentEntry;
            }

            return result;
        }

        private static string UnpackData(string data)
        {
            Check.StringIsMeaningful(data, "data");

            string[] parts = data.Substring(2).Split(new[] { "(D;;;;;S-", ")" }, StringSplitOptions.RemoveEmptyEntries);

            var binaryParts = new Dictionary<int, byte[]>();

            int lastIndex = 0;
            foreach (string[] items in parts.Select(currentPart => currentPart.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)))
            {
                if (items.Length != 16)
                    Exceptions.Throw(ErrorMessage.InvalidData);

                var binaryPart = new byte[52];

                for (int i = 3; i < 16; i++)
                    BitConverter.GetBytes(uint.Parse(items[i], CultureInfo.InvariantCulture)).CopyTo(binaryPart, (i - 3) * 4);

                int currentIndex = int.Parse(items[2], CultureInfo.InvariantCulture);
                binaryParts.Add(currentIndex, binaryPart);

                lastIndex = Math.Max(lastIndex, currentIndex);
            }

            var allData = new byte[52 * (lastIndex + 1)];

            foreach (KeyValuePair<int, byte[]> currentPart in binaryParts)
                currentPart.Value.CopyTo(allData, currentPart.Key * 52);

            int actualSize = BitConverter.ToInt32(allData, 0);
            return Encoding.Unicode.GetString(allData, 4, actualSize);
        }
    }
}
