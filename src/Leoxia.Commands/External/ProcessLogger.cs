using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    internal class ProcessLogger
    {
        private readonly string _processName;
        private readonly StringBuilder _errorBuilder;
        private readonly StringBuilder _outputBuilder;
        private readonly ISafeConsole _console;

        public ProcessLogger(string processName, ISafeConsole console)
        {
            _console = console;
            _processName = processName;
            _outputBuilder = new StringBuilder();
            _errorBuilder = new StringBuilder();
        }

        public string Output => _outputBuilder.ToString();
        public string Error => _errorBuilder.ToString();

        public void OnError(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _errorBuilder.AppendLine(e.Data);
                _console.SafeCall(console =>
                {
                    var foreground = console.ForegroundColor;
                    console.ForegroundColor = ConsoleColor.Red;
                    console.Error.WriteLine(e.Data);
                    console.ForegroundColor = foreground;
                });
            }
        }

        public void OnOutput(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                _outputBuilder.AppendLine(e.Data);
                _console.WriteLine(e.Data);
            }
        }
    }
}
