using System;
using System.Collections.Generic;
using System.Linq;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.External;
using Leoxia.Commands.Infrastructure;

namespace Leoxia.Commands
{
    public class CommandExecutor : ICommandExecutor
    {
        private readonly IConsole _console;
        private readonly IExecutableResolver _resolver;
        private readonly IProgramRunner _runner;

        private readonly IDictionary<string, IBuiltin> _builtins = 
            new Dictionary<string, IBuiltin>();

        public CommandExecutor(
            IConsole console, 
            IDirectory directory, 
            IFileSystemInfoFactory fileSystemFactory,
            IExecutableResolver resolver,
            IProgramRunner runner,
            IEnvironmentVariablesExpander expander,
            ILinkManager linkManager)
        {
            _console = console;
            _resolver = resolver;
            _runner = runner;
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
                        _runner.Run(command);
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