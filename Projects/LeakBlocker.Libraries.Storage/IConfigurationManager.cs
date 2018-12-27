using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Settings;

namespace LeakBlocker.Libraries.Storage
{
    /// <summary>
    /// Позволяет сохранять и загружать конфигурацию
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// Сохраняет простую конфигурацию. Побочный эффект: сохраняется прикрепленная к ней полная конфишурация
        /// </summary>
        void SaveConfiguration(ProgramConfiguration configuration);

        /// <summary>
        /// Выдает актуальную конфигурацию.
        /// null, если еще ни одна конфигурация не была сохранена в базе
        /// </summary>
        [CanBeNull]
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]//Метод выполняет тредоемкие действия и нелогично ему быть свойством
        ProgramConfiguration GetLastConfiguration();
    }
}
