using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using LeakBlocker.Libraries.Common;
using LeakBlocker.Libraries.Common.Collections;
using LeakBlocker.Libraries.Common.CommonInterfaces;

namespace LeakBlocker.Libraries.Common.Implementations
{
    internal sealed class CommandLine : ICommandLine
    {
        public bool Contains(string parameter)
        {
            Check.StringIsMeaningful(parameter, "parameter");

            ReadOnlyList<string> arguments = SharedObjects.Environment.CommandLineArguments;

            string result = arguments.FirstOrDefault(str1 => string.Equals(str1, parameter, StringComparison.OrdinalIgnoreCase));

            return (result != null);
        }

        public string GetValue(string keyParameter)
        {
            Check.StringIsMeaningful(keyParameter, "keyParameter");

            string result;

            bool hasValue = TryGetValue(keyParameter, out result);

            if (!hasValue)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Value was not found for parameter {0}", keyParameter));

            return result;
        }

        public bool HasValue(string keyParameter)
        {
            Check.StringIsMeaningful(keyParameter, "keyParameter");

            string result;

            return TryGetValue(keyParameter, out result);
        }

        public bool TryGetValue(string keyParameter, out string value)
        {
            Check.StringIsMeaningful(keyParameter, "keyParameter");

            ReadOnlyList<string> arguments = SharedObjects.Environment.CommandLineArguments;

            IEnumerable<string> afterKey = arguments.SkipWhile(str => !string.Equals(str, keyParameter, StringComparison.OrdinalIgnoreCase));

            value = afterKey.ElementAtOrDefault(1) ?? string.Empty; //следующий

            return !string.IsNullOrWhiteSpace(value);
        }

        public string Create(string executable, params string[] arguments)
        {
            Check.StringIsMeaningful(executable, "executable");
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            string result = "\"" + executable + "\"";
            if (arguments.Length > 0)
                result += (" " + CreateArguments(arguments));
            return result;
        }
        
        public string Create(string executable, IList<string> arguments)
        {
            Check.StringIsMeaningful(executable, "executable");
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            string result = "\"" + executable + "\"";
            if (arguments.Count > 0)
                result += (" " + CreateArguments(arguments));
            return result;
        }

        public string CreateArguments(params string[] arguments)
        {
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            return string.Join(" ", arguments);
        }

        public string CreateArguments(IList<string> arguments)
        {
            Check.CollectionHasNoDefaultItems(arguments, "arguments");

            return string.Join(" ", arguments);
        }

        public ReadOnlyList<string> Split(string commandLine)
        {
            bool quotes = false;

            var arguments = new List<string>();

            string currentArgument = string.Empty;

            foreach (char character in commandLine)
            {
                if (character == '\"')
                    quotes = !quotes;
                else if (!quotes && (character == ' '))
                {
                    arguments.Add(currentArgument);
                    currentArgument = string.Empty;
                }
                else
                    currentArgument += character;
            }
            if (!quotes)
                arguments.Add(currentArgument);
            else
                Exceptions.Throw(ErrorMessage.InvalidData, "Incorrect format.");

            return arguments.Where(argument => !string.IsNullOrEmpty(argument)).ToReadOnlyList();
        }
    }
}
