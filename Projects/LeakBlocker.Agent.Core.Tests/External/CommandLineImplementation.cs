using System;
using System.Collections.Generic;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.CommonInterfaces;
using LeakBlocker.Libraries.Common.Implementations;

namespace LeakBlocker.Agent.Core.Tests.External
{
    class CommandLineImplementation : ICommandLine
    {
        readonly ICommandLine cmd = new CommandLine();

        public CommandLineImplementation(string exe, params string[] arguments)
        {
            //cmd = exe;
            //args = arguments ?? new string[0];
        }

        public string Create(string executable, params string[] arguments)
        {
            return cmd.Create(executable, arguments);
        }

        public bool Contains(string parameter)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public string GetValue(string keyParameter)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public bool HasValue(string keyParameter)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public bool TryGetValue(string parameter, out string value)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public string Create(string executable, IList<string> arguments)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public string CreateArguments(params string[] arguments)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public string CreateArguments(IList<string> arguments)
        {
            throw new NotImplementedException("Should not be called.");
        }

        public ReadOnlyList<string> Split(string commandLine)
        {
            throw new NotImplementedException("Should not be called.");
        }
    }
}
