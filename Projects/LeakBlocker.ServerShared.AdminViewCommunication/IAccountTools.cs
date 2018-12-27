using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Network;

namespace LeakBlocker.ServerShared.AdminViewCommunication
{
    /// <summary>
    /// Содержит методы для работы с аккаунтами: поиск компьютеров, проверка логина/пароля и пр.
    /// </summary>
    [NetworkObject]
    public interface IAccountTools : IDisposable
    {
        /// <summary>
        /// Ищет домены, которые видны серверу. В список должны включаться:
        /// 1. Текущий домен (если есть)
        /// 2. Домены, с которыми когда-либо ранее велась работа
        /// 3. Домены в одном Forest'е с объектами из двух перечисленных выше групп
        /// </summary>
        ReadOnlySet<string> FindDnsDomains();

        /// <summary>
        /// Проверяет и сохраняет логин/пароль для домена (или компьютера)
        /// На выходе возвращается созданный домен.
        /// Если произошла ошибка, то будет брошено исключение
        /// </summary>
        /// <returns>Домен и идентификатор запроса обновления</returns>
        DomainUpdateRequest CheckAndSetCredentials(DomainCredentials credentials);

        /// <summary>
        /// Выдает список <see cref="Scope"/>, для которых уже есть логин и пароль и которые следует отображать в UI
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<Scope> GetAvailableComputerScopes();

        /// <summary>
        /// Выдает список <see cref="Scope"/>, которые можно использовать для списков пользователей
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<Scope> GetAvailableUserScopes();

        /// <summary>
        /// Выдает все компьютеры, которые содержатся в наборе Scope.
        /// </summary>
        ReadOnlySet<ResultComputer> GetComputers(ReadOnlySet<Scope> scope);

        /// <summary>
        /// Выдает список доступных компьютеров: то есть тех, до которых можно добраться из любых добавленных scope'ов + тех, которые сохранены в базе данных
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<BaseComputerAccount> GetAvailableComputers();

        /// <summary>
        /// Выдает список доступных пользователей: то есть тех, до которых можно добраться из любых добавленных scope'ов + тех, которые сохранены в базе данных
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        ReadOnlySet<BaseUserAccount> GetAvailableUsers();
        
        /// <summary>
        /// Выдает предпочтительный домен для работы продукта. Имеет смысл только при первых запусках
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        string GetPreferableDomain();

        /// <summary>
        /// Проверяет: обновился ли кеш для заданного запроса. Если метод вернул true то НЕЛЬЗЯ вызывать его второй раз с тем же параметром.
        /// </summary>
        bool IsRequestCompleted(DomainUpdateRequest request);

        /// <summary>
        /// Информация о текущем пользователе
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        UserContactInformation GetCurrentUserInformation();
    }
}
