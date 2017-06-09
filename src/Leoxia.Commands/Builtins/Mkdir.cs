using System;
using System.Collections.Generic;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public class Mkdir : IBuiltin
    {
        private readonly IConsole _console;
        private readonly IDirectory _directory;

        public Mkdir(IConsole console, IDirectory directory)
        {
            _console = console;
            _directory = directory;
        }

        public void Execute(List<string> tokens)
        {
            try
            {
                foreach (var token in tokens)
                {
                    _directory.CreateDirectory(token);
                }
            }
            catch (Exception e)
            {
                _console.Error.WriteLine(e.Message);
            }
        }

        public string Command => "mkdir";
    }
}