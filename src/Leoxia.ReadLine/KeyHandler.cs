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

        private readonly IDictionary<ControlSequence, Action> _handlers =
            new Dictionary<ControlSequence, Action>();

        public KeyHandler(IConsole console,
            IConsoleWriter writer,
            IPromptProvider promptProvider,
            IHistoryNavigator historyNavigator)
        {
            _console = console;
            _writer = writer;
            _promptProvider = promptProvider;
            _historyNavigator = historyNavigator;
            AddHandler(_writer.MoveCursorLeft, Seqs.ControlB, Seqs.LeftArrow);
            AddHandler(_writer.MoveCursorRight, Seqs.ControlF, Seqs.RightArrow);

            AddHandler(_writer.MoveCursorHome, Seqs.ControlA, Seqs.Home);

            AddHandler(_writer.MoveCursorEnd, Seqs.ControlE, Seqs.End);

            AddHandler(Backspace, Seqs.ControlH, Seqs.Backspace);

            AddHandler(ClearScreen, Seqs.ControlL);

            AddHandler(PreviousHistory, Seqs.ControlP, Seqs.UpArrow);
            AddHandler(NextHistory, Seqs.ControlN, Seqs.DownArrow);

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
            // If in auto complete mode and Tab wasn't pressed
            if (IsInAutoCompleteMode() && keyInfo.Key != ConsoleKey.Tab)
            {
                ResetAutoComplete();
            }
            var key = new ControlSequence(keyInfo);
            Action handler;
            if (_handlers.TryGetValue(key, out handler))
            {
                handler();
            }
            else
            {   
                if (!keyInfo.Modifiers.HasFlag(ConsoleModifiers.Alt) && 
                    !keyInfo.Modifiers.HasFlag(ConsoleModifiers.Control))
                {
                    _writer.Write(_historyNavigator.Current, keyInfo.KeyChar);
                }
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

        private void ResetAutoComplete()
        {
        }

        private bool IsInAutoCompleteMode()
        {
            return false;
        }
    }
}