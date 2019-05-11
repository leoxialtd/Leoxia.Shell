using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.External;
using Leoxia.Commands.Infrastructure;

namespace Leoxia.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IConsole _console;
        private readonly IExecutableResolver _resolver;
        private readonly IProgramRunnerFactory _runnerFactory;

        private readonly IDictionary<string, IBuiltin> _builtins = 
            new Dictionary<string, IBuiltin>();

        public CommandExecutor(
            IConsole console, 
            IExecutableResolver resolver,
            IProgramRunnerFactory runnerFactory,
            IBuiltin[] builtins)
        {
            _console = console;
            _resolver = resolver;
            _runnerFactory = runnerFactory;
            foreach (var builtin in builtins)
            {
                _builtins.Add(builtin.Command, builtin);
            }
        }


        public CommandResult Execute(string rawLine)
        {
            try
            {
                var command = rawLine.Trim(' ');
                if (string.IsNullOrEmpty(command))
                {
                    return CommandResult.Continue;
                }
                if (command == "exit" || command == "quit")
                {
                    return CommandResult.Exit;
                }
                var tokens = CommandLine.Split(rawLine).ToList();
                var first = tokens[0];
                tokens = tokens.GetRange(1, tokens.Count - 1);
                IBuiltin builtin;
                if (_builtins.TryGetValue(first, out builtin))
                {
                    builtin.Execute(tokens);
                }
                else
                {
                    if (string.IsNullOrEmpty(_resolver.Resolve(first)))
                    {
                        _console.Error.WriteLine($"lxsh: {first}: command not found");
                    }
                    else
                    {
                        var runner = _runnerFactory.CreateRunner(command, true);
                        var task = runner.AsyncRun();
                        while (!task.IsCompleted)
                        {
                            Task.Delay(50).Wait();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _console.Error.WriteLine(e.Message);
            }
            return CommandResult.Continue;
        }
    }
}