using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Entities.Security;

namespace LeakBlocker.Server.Service.InternalTools
{
    internal interface IAgentManager
    {
        /// <summary>
        /// Проверяет, включен ли компьютер. Эта функция не зависит от того, установлен агент на <paramref name="computer"/> или нет.
        /// </summary>
        bool IsComputerTurnedOn(BaseComputerAccount computer);

        /// <summary>
        /// Устанавливает агента и отписывается в базу.
        /// Исключения при установке агента подавляются, так как они записываются в базу
        /// </summary>
        void InstallAgent(BaseComputerAccount computer);
    }
}
