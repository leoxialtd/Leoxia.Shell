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

        public Echo(IConsole console)
        {
            _console = console;
        }

        public void Execute(List<string> tokens)
        {
            var unquotedTokens = tokens.Select(CommandLine.RemoveMatchingQuotes);
            _console.WriteLine(String.Join(" ", unquotedTokens));
        }

        public string Command => "echo";
    }
}