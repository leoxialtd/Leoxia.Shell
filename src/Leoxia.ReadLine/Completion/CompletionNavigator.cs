using System;
using System.Collections.Generic;
using Leoxia.Commands.Infrastructure;

namespace Leoxia.ReadLine
{
    public class CompletionNavigator : ICompletionNavigator
    {
        private bool _isCompleting;
        private CompletionListNavigator _navigator;

        public string[] PreviousAutoComplete(CommandLineBuffer currentCommandLine)
        {
            if (!_isCompleting)
            {
                return InitCompletion(currentCommandLine);
            }
            return new []{ _navigator.GoPrevious() };
        }

        private string[] InitCompletion(CommandLineBuffer currentCommandLine)
        {
            _isCompleting = true;
            var completionList = BuildCompletionList(currentCommandLine);
            _navigator = new CompletionListNavigator(completionList);
            return completionList;
        }

        private string[] BuildCompletionList(CommandLineBuffer currentCommandLine)
        {
            var commandLine = currentCommandLine.ToString();
            var tokens = CommandLine.Split(commandLine);
            var completionType = GetCompletionType(tokens);
            switch (completionType)
            {
                case CompletionType.Command:
                    break;
                case CompletionType.Directory:
                    break;
                case CompletionType.File:
                    break;
                case CompletionType.FileAndDirectory:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return null;
        }

        private CompletionType GetCompletionType(IEnumerable<string> tokens)
        {
            return CompletionType.Command;
        }

        public string[] NextAutoComplete(CommandLineBuffer currentCommandLine)
        {
            if (!_isCompleting)
            {
                return InitCompletion(currentCommandLine);
            }
            return new[] { _navigator.GoNext() };
        }

        public void ExitCompletion()
        {
            _isCompleting = false;
        }
    }

    internal enum CompletionType
    {
        Command,
        Directory,
        File,
        FileAndDirectory
    }
}