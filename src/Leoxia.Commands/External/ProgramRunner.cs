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
    /// <summary>
    /// Runner of executable in non interactive mode.
    /// </summary>
    public sealed class ProgramRunner : BaseProgramRunner, IDisposable
    {
        private readonly IExecutableResolver _resolver;
        private readonly ISafeConsole _safeConsole;
        private readonly IDirectory _directory;
        private readonly string _commandLine;
        private ProcessStartInfo _startInfo;
        private readonly string _processName;
        private readonly Process _process;
        private StreamReaderBridge _outputBridge;
        private StreamReaderBridge _errorBridge;
        private StreamWriterBridge _inputBridge;
        private readonly string _arguments;
        private StreamWriter _input;
        private FlushingBuffer _flushingOutput;

        public ProgramRunner(
            IExecutableResolver resolver, 
            ISafeConsole safeConsole, 
            IDirectory directory,
            string commandLine) : 
            base(resolver,safeConsole, directory, commandLine)
        {
            _resolver = resolver;
            _safeConsole = safeConsole;
            _directory = directory;
            _commandLine = commandLine;
            var argumentList = GetArgumentList(_commandLine);
            _processName = argumentList[0];
            _startInfo = new ProcessStartInfo(_resolver.Resolve(_processName));
            argumentList.RemoveAt(0);
            _arguments = string.Empty;
            if (Enumerable.Any<string>(argumentList))
            {
                _arguments = AggregateArguments(argumentList);
                _startInfo.Arguments = _arguments;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // Not Supported on Unix
                //_startInfo.LoadUserProfile = true;                
            }
            _startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            _startInfo.UseShellExecute = false;
      
            _startInfo.WorkingDirectory = _directory.GetCurrentDirectory();
            _process = new Process();            
            _process.StartInfo = _startInfo;
        }

        public async Task<ProgramResult> AsyncRun()
        {
            if (!_process.Start())
            {
                _safeConsole.Error.WriteLine("Cannot start process " + _processName + " " + _arguments);
            }

            return await Task.Run(() =>
            {
                DateTime start = DateTime.Now;
                while (!_process.HasExited)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    _process.Refresh();
                }

                var exitCode = _process.ExitCode;
                Dispose();

                return new ProgramResult(String.Empty, string.Empty, exitCode);
            });
        }

        protected override void BeforeStart()
        {
            //_startInfo.CreateNoWindow = true;
            //_startInfo.RedirectStandardOutput = true;
            //_startInfo.RedirectStandardError = true;
            // TODO: Fix StandardInput Redirecton
            //_startInfo.RedirectStandardInput = true;  
        }

        protected override void AfterStart()
        {
            // TODO: Fix StandardInput Redirecton
            //_inputBridge = new StreamWriterBridge(_process);
            //_flushingOutput = new FlushingBuffer(_safeConsole.Out);
            //_outputBridge = new StreamReaderBridge(_process.StandardOutput, _flushingOutput);
            //_errorBridge = new StreamReaderBridge(_process.StandardError, new LineBuffer(_safeConsole.Error, _safeConsole));
            //_outputBridge.BeginRead();
            //_errorBridge.BeginRead();
        }

        protected override void OnExit()
        {
            //_outputBridge.EndRead();
            //_errorBridge.EndRead();
            _flushingOutput?.Dispose();
        }

        protected override ProgramResult GetResult(int exitCode)
        {
            return new ProgramResult(_outputBridge.ToString(), _errorBridge.ToString(), exitCode);
        }

        private static string AggregateArguments(List<string> argumentList)
        {
            return argumentList.Aggregate((x, y) => x + " " + y);
        }

        private static List<string> GetArgumentList(string commandLine)
        {
            return CommandLine.Split(commandLine).TrimMatchingQuotes().ToList();
        }

        public void WriteInInput(ConsoleKeyInfo key)
        {
            _inputBridge.Write(key);
        }

    }
}