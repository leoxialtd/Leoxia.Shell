using System;
using System.Collections.Generic;
using System.IO;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public class Cd : IBuiltin
    {
        private readonly IConsole _console;
        private readonly IDirectory _directory;

        public Cd(IConsole console, IDirectory directory)
        {
            _console = console;
            _directory = directory;
        }

        public void Execute(List<string> tokens)
        {
            try
            {
                if (tokens.Count >  1)
                {
                    _console.Error.WriteLine("Cd cannot have more than argument.");
                }
                else if (tokens.Count == 1)
                {
                    var newDir = tokens[0];
                    if (newDir != ".")
                    {
                        _directory.SetCurrentDirectory(newDir);
                    }
                }
            }
            catch (Exception e)
            {
                _console.Error.WriteLine(e.Message);   
            }
        }
    }
}