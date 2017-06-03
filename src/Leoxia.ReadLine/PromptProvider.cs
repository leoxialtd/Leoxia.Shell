using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;

namespace Leoxia.ReadLine
{
    public class PromptProvider : IPromptProvider
    {
        private readonly IDirectory _directory;
        private readonly ITimeProvider _timeProvider;
        private readonly IConsole _console;

        public PromptProvider(
            IDirectory directory, 
            ITimeProvider timeProvider,
            IConsole console)
        {
            _directory = directory;
            _timeProvider = timeProvider;
            _console = console;
        }

        public void WritePrompt()
        {
            var emphasis = ConsoleColor.Yellow;
            var currentTime = _timeProvider.Now.ToString("ddd yy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            Write(emphasis, "[");
            Write(ConsoleColor.Cyan, currentTime);
            Write(emphasis, "]");
            var osInformation = 
                $"{RuntimeInformation.OSArchitecture} {RuntimeInformation.OSDescription}".Trim(' ');
            Write(emphasis, "[");
            Write(ConsoleColor.Cyan, osInformation);
            Write(emphasis, "]");
            _console.WriteLine();
            var account = Environment.GetEnvironmentVariable("USERNAME");
            var machine = GetMachineName();
            var curDir = _directory.GetCurrentDirectory();
            Write(ConsoleColor.Red, $" {account}");
            Write(ConsoleColor.White, "@");
            Write(ConsoleColor.Red, machine);
            Write(emphasis, ":");
            Write(ConsoleColor.Green, curDir);
            _console.WriteLine();
            Write(emphasis, ">");
        }

        private string GetMachineName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Environment.GetEnvironmentVariable("USERDOMAIN");
            }
            return Environment.GetEnvironmentVariable("HOSTNAME");
        }

        public void Write(ConsoleColor color, string text)
        {
            var savedColor = _console.ForegroundColor;
            _console.ForegroundColor = color;
            _console.Write(text);
            _console.ForegroundColor = savedColor;
        }
    }
}