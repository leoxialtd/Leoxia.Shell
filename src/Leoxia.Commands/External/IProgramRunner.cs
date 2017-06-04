using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Leoxia.Commands.Infrastructure;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    public interface IProgramRunner
    {
        ProgramResult Run(string command);
    }

    public class ProgramRunner : IProgramRunner
    {
        private readonly IEnvironmentVariablesExpander _expander;
        private readonly IExecutableResolver _resolver;
        private readonly ISafeConsole _safeConsole;

        public ProgramRunner(IEnvironmentVariablesExpander expander,
            IExecutableResolver resolver, 
            ISafeConsole safeConsole)
        {
            _expander = expander;
            _resolver = resolver;
            _safeConsole = safeConsole;
        }

        public ProgramResult Run(string commandLine)
        {
            try
            {
                return AsyncRun(commandLine).Result;
            }
            catch (Exception e)
            {
                _safeConsole.WriteLine(e.Message);
                return new ProgramResult(string.Empty, e.Message, -2);
            }
        }

        private static string AggregateArguments(List<string> argumentList)
        {
            return argumentList.Aggregate((x, y) => x + " " + y);
        }

        private static List<string> GetArgumentList(string commandLine)
        {
            return CommandLine.Split(commandLine).TrimMatchingQuotes().ToList();
        }

        public async Task<ProgramResult> AsyncRun(string commandLine)
        {
            return await Task.Run(() =>
            {
                // Ensure proper display of some characters
                commandLine = _expander.Expand(commandLine);
                var argumentList = GetArgumentList(commandLine);
                var processName = argumentList[0];
                ProcessStartInfo startInfo = new ProcessStartInfo(_resolver.Resolve(processName));
                argumentList.RemoveAt(0);
                string arguments = string.Empty;
                if (argumentList.Any())
                {
                    arguments = AggregateArguments(argumentList);
                    startInfo.Arguments = arguments;
                }
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                // Not Supported on Unix
                // startInfo.LoadUserProfile = true;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.RedirectStandardInput = true;
                startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    var logger = new ProcessLogger(Path.GetFileName(startInfo.FileName), _safeConsole);
                    process.OutputDataReceived += logger.OnOutput;
                    process.ErrorDataReceived += logger.OnError;
                    if (!process.Start())
                    {
                        _safeConsole.Error.WriteLine("Cannot start process " + processName + " " + arguments);
                    }
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();
                    DateTime start = DateTime.Now;
                    while (!process.HasExited)
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }
                    process.OutputDataReceived -= logger.OnOutput;
                        process.ErrorDataReceived -= logger.OnError;
                        return new ProgramResult(logger.Output, logger.Error, process.ExitCode);                    
                }
            });
        }
    }
}