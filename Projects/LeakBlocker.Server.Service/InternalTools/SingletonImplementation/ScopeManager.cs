using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.Entities.Security;
using LeakBlocker.Libraries.Common.Entities.Settings;
using LeakBlocker.Libraries.Common.Entities.Settings.Rules;
using LeakBlocker.Libraries.SystemTools;
using LeakBlocker.Server.Service.InternalTools.Extensions;

namespace LeakBlocker.Server.Service.InternalTools.SingletonImplementation
{
    internal sealed class ScopeManager : IScopeManager
    {
        private ReadOnlySet<BaseComputerAccount> currentScope = ReadOnlySet<BaseComputerAccount>.Empty;

        private readonly object updateSyncRoot = new object();

        public ScopeManager()
        {
            InternalObjects.SecurityObjectCache.Updated += ForceUpdateScope;

            ForceUpdateScope();
        }

        public event Action ScopeChanged;

        public ReadOnlySet<BaseComputerAccount> CurrentScope()
        {
            return currentScope;
        }

        public void ForceUpdateScope()
        {
            using (new TimeMeasurement("ForceUpdateScope", true))
            {
                lock (updateSyncRoot)
                {
                    ReadOnlySet<BaseComputerAccount> computers = GetComputers().ToReadOnlySet();

                    Log.Add("ForceUpdateScope::computers in scope - {0}".Combine(computers));

                    if (computers.Equals(currentScope))
                        return;

                    currentScope = computers;

                    if (ScopeChanged != null)
                        ScopeChanged();
                }
            }
        }

        private static IEnumerable<BaseComputerAccount> GetComputers()
        {
            ProgramConfiguration configuration = InternalObjects.ConfigurationStorage.CurrentFullConfiguration;

            List<Rule> rules = configuration.Rules.ToList();

            rules.Sort(Rule.CompareRulesUsingPriority);

            var resultComputers = new HashSet<BaseComputerAccount>();

            foreach (Rule rule in rules)
            {
                resultComputers.AddRange(rule.RootCondition.GetAllComputersUsedInCondition());//Добавляем используемые...

                rule.GetExcludedComputers().ForEach(computer => resultComputers.Remove(computer));//... и удаляем те, которые 100% будут разблокированы и вне аудита
            }

            return resultComputers;
        }

    }
}
