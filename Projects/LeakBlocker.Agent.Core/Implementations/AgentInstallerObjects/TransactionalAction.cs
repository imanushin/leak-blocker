using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;

namespace LeakBlocker.Agent.Core.Implementations.AgentInstallerObjects
{
    /// <summary>
    /// Class for performing transacted operations.
    /// </summary>
    public sealed class TransactionalAction
    {
        private readonly bool suppressErrors;
        private readonly Action mainAction;
        private readonly Action rollbackAction;

        /// <summary>
        /// Creates an of TransactedAction class.
        /// </summary>
        /// <param name="mainAction">Main action. Can be null.</param>
        /// <param name="rollbackAction">Rollback action. Can be null.</param>
        /// <param name="suppressErrors">If true then any errors during executing the current action will be ignored. 
        /// Indicates non-critical actions within the action sequence (such action is always treated as successfull, but if sequence rollback
        /// is initiated by one of the subsequent actions then current action's rollback action is also executed.</param>
        public TransactionalAction(Action mainAction, Action rollbackAction, bool suppressErrors = false)
        {
            this.mainAction = mainAction;
            this.rollbackAction = rollbackAction;
            this.suppressErrors = suppressErrors;
        }

        /// <summary>
        /// Executes the specified action sequence. If one of the actions fails then rollback actions are called in the inverse order.
        /// </summary>
        /// <param name="sequence">Action sequence.</param>
        /// <param name="force">True means that no rollback should be performed. In other words all actions will be treated as successful.</param>
        public static void RunSequence(IList<TransactionalAction> sequence, bool force = false)
        {
            Check.CollectionHasOnlyMeaningfulData(sequence, "sequence");

            var exceptions = new List<Exception>();

            for (int i = 0; i < sequence.Count; i++)
            {
                try
                {
                    sequence[i].Run();
                }
                catch (Exception exception)
                {
                    exceptions.Add(exception);

                    if (force) 
                        continue;

                    for (int j = i; j >= 0; j--)
                    {
                        try
                        {
                            sequence[j].Rollback();
                        }
                        catch (Exception rollbackException)
                        {
                            exceptions.Add(rollbackException);
                        }
                    }
                    break;
                }
            }

            if (exceptions.Count <= 0) 
                return;

            var resultException = new AggregateException(exceptions);
            if (force)
                Log.Write(resultException);
            else
                throw resultException;
        }

        private void Run()
        {
            if (mainAction == null)
                return;

            try
            {
                mainAction();
            }
            catch (Exception exception)
            {
                if (suppressErrors)
                    Log.Write(exception);
                else
                    throw;
            }
        }

        private void Rollback()
        {
            if (rollbackAction == null)
                return;

            try
            {
                rollbackAction();
            }
            catch (Exception exception)
            {
                if (suppressErrors)
                    Log.Write(exception);
                else
                    throw;
            }
        }
    }
}
