using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Libraries.Common.Entities.Settings.Rules
{
    /// <summary>
    /// Действия, которые применятся, если условие Rule будет выполнено.
    /// </summary>
    [DataContract(IsReference = true)]
    [Serializable]
    public sealed class ActionData : BaseEntity
    {
        private static readonly ActionData skipAll = new ActionData(BlockActionType.Skip,AuditActionType.Skip);

        /// <summary>
        /// Skip для обоих действий.
        /// </summary>
        public static ActionData SkipAll
        {
            get
            {
                return skipAll;
            }
        }

        /// <summary>
        /// Настройки блокировки
        /// </summary>
        [DataMember]
        [Required]
        public BlockActionType BlockAction
        {
            get;
            private set;
        }

        /// <summary>
        /// Настройки аудита.
        /// </summary>
        [DataMember]
        [Required]
        public AuditActionType AuditAction
        {
            get;
            private set;
        }

        /// <summary>
        /// Создает набор действий для блокировки.
        /// </summary>
        public ActionData(BlockActionType block, AuditActionType audit)
        {
            Check.EnumerationValueIsDefined(block, "block");
            Check.EnumerationValueIsDefined(audit, "audit");

            BlockAction = block;
            AuditAction = audit;
        }

        #region Equality

        /// <summary>
        /// Возвращает все вложенные объекты, которые имеют смысл для вычисления хеш-кода и метода Equals
        /// </summary>
        protected override IEnumerable<object> GetInnerObjects()
        {
            yield return BlockAction;
            yield return AuditAction;
        }

        #endregion

        /// <summary>
        /// Создает новое действие на основе текущего + действий, полученных из правил с бОльшим приоритетом.
        /// Если в агрументе не Skip, то учитывается настройка из аргумента
        /// </summary>
        /// <returns></returns>
        public ActionData UpdateActions(ActionData actions)
        {
            Check.ObjectIsNotNull(actions);

            BlockActionType block = actions.BlockAction == BlockActionType.Skip ? BlockAction : actions.BlockAction;
            AuditActionType audit = actions.AuditAction == AuditActionType.Skip ? AuditAction : actions.AuditAction;

            return new ActionData(block,audit);
        }
    }
}
