#region Copyright (c) 2017 Leoxia Ltd

// Copyright © 2011 - 2017 Leoxia Ltd, https://www.leoxia.com
// 
// All rights reserved.
// 
// No permission is hereby granted to copy, modify, or share the content of 
// the following files and any dependencies. Any violation of the current notice
// will be prosecuted.

#endregion

#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Leoxia.Abstractions.IO;
using Seqs = Leoxia.ReadLine.ControlSequences;

#endregion

namespace Leoxia.ReadLine
{
    public class KeyHandler : IKeyHandler
    {
        private readonly IConsole _console;
        private readonly IConsoleWriter _writer;
        private readonly IPromptProvider _promptProvider;
        private readonly IHistoryNavigator _historyNavigator;
        private readonly ICompletionNavigator _completionNavigator;

        private readonly IDictionary<ControlSequence, Action> _handlers =
            new Dictionary<ControlSequence, Action>();

        private readonly ICompletionWriter _completionWriter;

        public KeyHandler(IConsole console,
            IConsoleWriter writer,
            IPromptProvider promptProvider,
            IHistoryNavigator historyNavigator,
            ICompletionNavigator completionNavigator,
            ICompletionWriter completionWriter)
        {
            _console = console;
            _writer = writer;
            _promptProvider = promptProvider;
            _historyNavigator = historyNavigator;
            _completionNavigator = completionNavigator;
            _completionWriter = completionWriter;
            AddHandler(_writer.MoveCursorLeft, Seqs.ControlB, Seqs.LeftArrow);
            AddHandler(_writer.MoveCursorRight, Seqs.ControlF, Seqs.RightArrow);

            AddHandler(_writer.MoveCursorHome, Seqs.ControlA, Seqs.Home);

            AddHandler(_writer.MoveCursorEnd, Seqs.ControlE, Seqs.End);

            AddHandler(Backspace, Seqs.ControlH, Seqs.Backspace);

            AddHandler(ClearScreen, Seqs.ControlL);

            AddHandler(PreviousHistory, Seqs.ControlP, Seqs.UpArrow);
            AddHandler(NextHistory, Seqs.ControlN, Seqs.DownArrow);

            AddHandler(BreakProcess, Seqs.ControlC);

            AddHandler(NextAutoComplete, Seqs.Tab);
            AddHandler(PrevAutoComplete, Seqs.ShiftTab);

            // Cmder specific features
            //AddHandler(TraverseUpDirectory, Seqs.ControlAltU);

            // Gnu Readline https://en.wikipedia.org/wiki/GNU_Readline
            //AddHandler(DeleteNextChar, Seqs.ControlD);

            //AddHandler(SearchHistoryUp, Seqs.ControlR);
            //AddHandler(SearchHistoryDown, Seqs.ControlS);

            // Undo
            //AddHandler(Undo, Seqs.ControlUnderscore, Seqs.ControlXControlU);
            //_handlers.Add(BuildKey(ConsoleKey.Underscore, Console.Control), Undo);
        }

        private void PrevAutoComplete()
        {
            var results = _completionNavigator.PreviousAutoComplete(_historyNavigator.Current);
            _writer.Write(_historyNavigator.Current);
            _completionWriter.Write(results);
        }

        private void NextAutoComplete()
        {
            var results = _completionNavigator.NextAutoComplete(_historyNavigator.Current);
            _writer.Write(_historyNavigator.Current);
            _completionWriter.Write(results);
        }

        private void BreakProcess()
        {
            
        }

        private void NextHistory()
        {
            if (_historyNavigator.HasNext)
            {
                _historyNavigator.GoNext();
                _writer.Write(_historyNavigator.Current);
            }
        }

        private void PreviousHistory()
        {
            if (_historyNavigator.HasPrevious)
            {
                _historyNavigator.GoPrevious();
                _writer.Write(_historyNavigator.Current);
            }
        }

        public void Handle(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key != ConsoleKey.Tab)
            {
                _completionNavigator.ExitCompletion();
            }
            var key = new ControlSequence(keyInfo);
            Action handler;
            if (_handlers.TryGetValue(key, out handler))
            {
                handler();
            }
            else
            {   
                _writer.Write(_historyNavigator.Current, keyInfo.KeyChar);               
            }
        }

        private void ClearScreen()
        {
            _console.Clear();
            _promptProvider.WritePrompt();
            _writer.Reset();
        }

        private void Backspace()
        {
            _writer.Backspace(_historyNavigator.Current);                              
        }

        private void AddHandler(Action action, params ControlSequence[] controlSequences)
        {
            foreach (var sequence in controlSequences)
            {
                _handlers.Add(sequence, action);
            }
        }

    }
}