using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.CommonInterfaces
{
    /// <summary>
    /// Image type corresponding to the processor architecture.
    /// </summary>
    public enum NativeImageType
    {
        /// <summary>
        /// CA1008
        /// </summary>
        [ForbiddenToUse]
        [Obsolete("Incorrect value.", true)]
        None = 0,

        /// <summary>
        /// 32-bit.
        /// </summary>
        X86 = 1,

        /// <summary>
        /// 64-bit.
        /// </summary>
        X64,
    }

    /// <summary>
    /// Common constants.
    /// </summary>
    public interface IConstants
    {
        /// <summary>
        /// Tcp порт по-умолчанию
        /// </summary>
        int DefaultTcpPort
        {
            get;
        }

        /// <summary>
        /// Максимальное время работы метода на сервере
        /// </summary>
        TimeSpan DefaultTcpTimeout
        {
            get;
        }

        /// <summary>
        /// Returns a product subfolder path in the ProgramData folder.
        /// </summary>
        string UserDataFolder
        {
            get;
        }
        
        /// <summary>
        /// Путь к текущему продукту с добавлением версии продукта
        /// </summary>
        string CurrentVersionProgramFilesFolder
        {
            get;
        }

        /// <summary>
        /// Folder where main module is located.
        /// </summary>
        string MainModuleFolder
        {
            get;
        }

        /// <summary>
        /// Full path to the main module.
        /// </summary>
        string MainModulePath
        {
            get;
        }

        /// <summary>
        /// Main module file name.
        /// </summary>
        string MainModuleFile
        {
            get;
        }

        /// <summary>
        /// Returns temporary folder path.
        /// </summary>
        string TemporaryFolder
        {
            get;
        }

        /// <summary>
        /// Returns the version of the current executable.
        /// </summary>
        Version Version
        {
            get;
        }

        /// <summary>
        /// String that identifies version. Must meet any object name requirements, thus should constain only numbers and underscores.
        /// </summary>
        string VersionString
        {
            get;
        }

        /// <summary>
        /// Returns current process image architecture.
        /// </summary>
        NativeImageType CurrentProcessImageType
        {
            get;
        }
    }
}
