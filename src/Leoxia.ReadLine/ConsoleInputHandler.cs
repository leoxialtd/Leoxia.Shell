using System;
using Leoxia.Abstractions.IO;

namespace Leoxia.ReadLine
{
    public class ConsoleInputHandler : IConsoleInputHandler
    {
        private readonly IConsole _console;
        private readonly IConsoleWriter _consoleWriter;
        private readonly IKeyHandler _keyHandler;
        private readonly IHistoryNavigator _historyNavigator;
        private readonly IPromptProvider _promptProvider;

        public ConsoleInputHandler(IConsole console, 
            IConsoleWriter consoleWriter,
            IKeyHandler keyHandler,
            IHistoryNavigator historyNavigator,
            IPromptProvider promptProvider)
        {
            _console = console;
            _consoleWriter = consoleWriter;
            _keyHandler = keyHandler;
            _historyNavigator = historyNavigator;
            _promptProvider = promptProvider;
        }

        public string ReadLine()
        {
            _promptProvider.WritePrompt();
            _consoleWriter.Reset();
            ConsoleKeyInfo keyInfo = _console.ReadKey(true);
            while (keyInfo.Key != ConsoleKey.Enter)
            {
                _keyHandler.Handle(keyInfo);
                keyInfo = _console.ReadKey(true);
            }
            _console.WriteLine();            
            return _historyNavigator.Validate();
        }
    }
}