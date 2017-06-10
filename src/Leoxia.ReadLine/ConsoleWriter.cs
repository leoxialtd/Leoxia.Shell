using System;
using System.Diagnostics;
using System.Linq;
using Leoxia.Abstractions.IO;

namespace Leoxia.ReadLine
{
    public class ConsoleWriter : IConsoleWriter
    {
        private readonly IConsole _console;

        private int _cursorLimit;
        private int _cursorPos;
        private int _initialLeft;
        private int _initialTop;
        private int _maxLeft;
        private int _maxTop;

        public ConsoleWriter(IConsole console)
        {
            _console = console;
            Reset();
        }

        private bool IsStartOfLine()
        {
            return _cursorPos == 0;
        }

        private bool IsEndOfLine()
        {
            return _cursorPos == _cursorLimit;
        }

        private bool IsStartOfBuffer()
        {
            return _console.CursorLeft == 0;
        }

        private bool IsEndOfBuffer()
        {
            return _console.CursorLeft == _console.BufferWidth - 1;
        }

        public void Write(CommandLineBuffer history)
        {
            var spaces = _cursorLimit - history.Length;
            _console.SetCursorPosition(_initialLeft, _initialTop);
            _console.Write(history);
            if (spaces > 0)
            {
                WriteInPlace(String.Concat(Enumerable.Repeat(' ', spaces)));
            }
            _cursorPos = _cursorLimit = history.Length;
        }

        public void Write(CommandLineBuffer buffer, char c)
        {
            if (IsEndOfLine())
            {
                buffer.Append(c);
                _console.Write(c);
                _cursorPos++;
            }
            else
            {
                var str = buffer.ToString().Substring(_cursorPos);
                buffer.Insert(_cursorPos, c);
                WriteInPlace(c + str);
                MoveCursorRight();
            }
            IncrementCursorLimit();
        }

        private void IncrementCursorLimit()
        {
            if (_maxLeft == _console.BufferWidth - 1)
            {
                _maxLeft = 0;
                _maxTop++;
            }
            else
            {
                _maxLeft++;
            }
            _cursorLimit++;
        }

        public void WriteBelow(string text)
        {
            
        }

        private void WriteInPlace(string value)
        {
            var left = _console.CursorLeft;
            var top = _console.CursorTop;
            _console.Write(value);
            _console.SetCursorPosition(left, top);
        }

        public void Backspace(CommandLineBuffer buffer)
        {
            if (!IsStartOfLine())
            {
                buffer.Remove(_cursorPos - 1, 1);
                var isEndOfLine = IsEndOfLine();
                MoveCursorLeft();
                var value = " ";
                if (!isEndOfLine)
                {
                    var str = buffer.ToString().Substring(_cursorPos);
                    value = str + value;
                }
                WriteInPlace(value);
                DecrementCursorLimit();
            }
        }

        private void DecrementCursorLimit()
        {
            if (_maxLeft == 0)
            {
                _maxTop--;
                _maxLeft = _console.BufferWidth - 1;
            }
            else
            {
                _maxLeft--;
            }
            _cursorLimit--;
        }

        public void Reset()
        {
            _maxLeft = _initialLeft = _console.CursorLeft;
            _maxTop = _initialTop = _console.CursorTop;
            _cursorPos = 0;
            _cursorLimit = 0;
        }
        public void MoveCursorHome()
        {
            _cursorPos = 0;
            _console.SetCursorPosition(_initialLeft, _initialTop);
        }

        public void MoveCursorEnd()
        {
            _cursorPos = _cursorLimit;
            _console.SetCursorPosition(_maxLeft, _maxTop);
        }

        public void MoveCursorLeft()
        {
            if (!IsStartOfLine())
            {
                if (IsStartOfBuffer())
                {
                    _console.SetCursorPosition(_console.BufferWidth - 1, _console.CursorTop - 1);
                }
                else
                {
                    _console.SetCursorPosition(_console.CursorLeft - 1, _console.CursorTop);
                }
                _cursorPos--;
            }
        }

        public void MoveCursorRight()
        {
            if (IsEndOfLine())
            {
                return;
            }
            if (IsEndOfBuffer())
            {
                _console.SetCursorPosition(0, _console.CursorTop + 1);
            }
            else
            {
                _console.SetCursorPosition(_console.CursorLeft + 1, _console.CursorTop);
            }
            _cursorPos++;
        }
    }
}