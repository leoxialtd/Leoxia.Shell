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
            IDirectory directory, 
            IFileSystemInfoFactory fileSystemFactory,
            IExecutableResolver resolver,
            IProgramRunnerFactory runnerFactory,
            IEnvironmentVariablesExpander expander,
            ILinkManager linkManager)
        {
            _console = console;
            _resolver = resolver;
            _runnerFactory = runnerFactory;
            _builtins.Add("echo", new Echo(console, expander));
            _builtins.Add("cd", new Cd(console, directory));
            _builtins.Add("mkdir", new Mkdir(console, directory));
            _builtins.Add("ls", new Ls(console, directory, fileSystemFactory, linkManager));
            //            _builtins.Add("rm", new Rm());
            //            _builtins.Add("del", new Del());
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
                        var runner = _runnerFactory.CreateRunner(command);
                        var task = runner.AsyncRun();
                        while (!task.IsCompleted)
                        {
                            Task.Delay(100).Wait();
                            // TODO: Fix the StandardInput redirection
                            //var key = _console.ReadKey(true);
                            //runner.WriteInInput(key);
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