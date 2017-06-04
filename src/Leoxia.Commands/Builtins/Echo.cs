using System;
using System.Collections.Generic;
using System.Linq;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Infrastructure;

namespace Leoxia.Commands
{
    public class Echo : IBuiltin
    {
        private readonly IConsole _console;
        private readonly IEnvironmentVariablesExpander _expander;

        public Echo(IConsole console, IEnvironmentVariablesExpander expander)
        {
            _console = console;
            _expander = expander;
        }

        public void Execute(List<string> tokens)
        {
            var expandedTokens = tokens.Select(_expander.Expand);
            var unquotedTokens = expandedTokens.Select(CommandLine.RemoveMatchingQuotes);
            _console.WriteLine(String.Join(" ", unquotedTokens));
        }
    }
}