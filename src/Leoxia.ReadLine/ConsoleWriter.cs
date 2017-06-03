using System;
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
            if (spaces > 0)
            {
                _console.Write(String.Concat(Enumerable.Repeat(' ', spaces)));
            }
            _console.SetCursorPosition(_initialLeft, _initialTop);
            _console.Write(history);
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
                var left = _console.CursorLeft;
                var top = _console.CursorTop;
                var str = buffer.ToString().Substring(_cursorPos);
                buffer.Insert(_cursorPos, c);
                _console.Write(c + str);
                _console.SetCursorPosition(left, top);
                MoveCursorRight();
            }
            _maxLeft = _console.CursorLeft;
            _maxTop = _console.CursorTop;
            _cursorLimit++;
        }

        public void Backspace(CommandLineBuffer buffer)
        {
            if (!IsStartOfLine())
            {
                buffer.Remove(_cursorPos - 1, 1);
                var isEndOfLine = IsEndOfLine();
                MoveCursorLeft();
                int left = _console.CursorLeft;
                int top = _console.CursorTop;
                if (!isEndOfLine)
                {
                    var str = buffer.ToString().Substring(_cursorPos - 1);
                    _console.Write(str);
                }
                _console.Write(' ');
                _console.SetCursorPosition(left, top);
                _cursorLimit--;
            }
        }

        public void Reset()
        {
            _initialLeft = _console.CursorLeft;
            _initialTop = _console.CursorTop;
            _maxLeft = _console.CursorLeft;
            _maxTop = _console.CursorTop;
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