using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Infrastructure;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    public abstract class BaseProgramRunner : IProgramRunner, IDisposable
    {
        private readonly ISafeConsole _safeConsole;
        private readonly string _processName;
        private readonly Process _process;
        private readonly string _arguments;

        protected BaseProgramRunner(
            IExecutableResolver resolver,
            ISafeConsole safeConsole,
            IDirectory directory,
            string commandLine)
        {
            _safeConsole = safeConsole;
            var argumentList = GetArgumentList(commandLine);
            _processName = argumentList[0];
            var startInfo = new ProcessStartInfo(resolver.Resolve(_processName));
            argumentList.RemoveAt(0);
            _arguments = string.Empty;
            if (Enumerable.Any<string>(argumentList))
            {
                _arguments = AggregateArguments(argumentList);
                startInfo.Arguments = _arguments;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Not Supported on Unix
                //_startInfo.LoadUserProfile = true;                
            }
            //_startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;  
            startInfo.WorkingDirectory = directory.GetCurrentDirectory();
            _process = new Process();
            _process.StartInfo = startInfo;
        }

        public async Task<ProgramResult> AsyncRun()
        {
            BeforeStart();
            if (!_process.Start())
            {
                _safeConsole.Error.WriteLine("Cannot start process " + _processName + " " + _arguments);
            }
            AfterStart();
            return await Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                while (!_process.HasExited)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    _process.Refresh();
                }
                OnExit();
                var exitCode = _process.ExitCode;
                Dispose();
                return GetResult(exitCode);
            });
        }

        protected abstract void BeforeStart();
        protected abstract void AfterStart();
        protected abstract void OnExit();
        protected abstract ProgramResult GetResult(int exitCode);

        private static string AggregateArguments(List<string> argumentList)
        {
            return argumentList.Aggregate((x, y) => x + " " + y);
        }

        private static List<string> GetArgumentList(string commandLine)
        {
            return CommandLine.Split(commandLine).TrimMatchingQuotes().ToList();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {            
            _process.Dispose();
        }
    }
}