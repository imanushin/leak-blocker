using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Интерфейс для сохранения и загрузки конфигурации
    /// </summary>
    [NetworkObject]
    public interface IConfigurationTools : IDisposable
    {
        /// <summary>
        /// Последняя конфигурация. null, если нет еще ни одной сохраненной.
        /// </summary>
        [CanBeNull]
        SimpleConfiguration LastConfiguration();

        /// <summary>
        /// Проверяет и сохраняет конфигурацию
        /// </summary>
        void SaveConfiguration(SimpleConfiguration configuration);

        /// <summary>
        /// true, если на сервер хоть раз сохранялась конфигурация
        /// </summary>
        bool HasConfiguration();
    }
}
