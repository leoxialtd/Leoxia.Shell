using System;

namespace Leoxia.ReadLine
{
    public class CompletionWriter : ICompletionWriter
    {
        private readonly IConsoleWriter _consoleWriter;

        public CompletionWriter(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
        }

        public void Write(string[] results)
        {
            if (results  != null && results.Length > 0)
            {
                if (results.Length > 1)
                {
                    var concatened = Concat(results);
                    _consoleWriter.WriteBelow(concatened);
                }
            }
        }

        private string Concat(string[] results)
        {
            return String.Join("  ", results);
        }
    }
}