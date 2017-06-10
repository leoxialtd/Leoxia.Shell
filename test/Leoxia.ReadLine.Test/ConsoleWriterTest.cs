using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leoxia.Abstractions.IO;
using Moq;
using Xunit;

namespace Leoxia.ReadLine.Test
{
    public class ConsoleWriterTest
    {
        private int _currentIndex;
        private readonly char[] _builder = new char[200];
        private int _bufferWidth = 80;

        [Fact]
        public void WriteAndBackspaceTest()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'a');
            writer.Write(buffer, 'b');
            writer.Write(buffer, 'c');
            writer.Write(buffer, 'd');
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            Assert.Equal(String.Empty, buffer.ToString());
            Assert.Equal(String.Empty, GetBuffer());
        }

        private IConsole BuildConsole()
        {
            var consoleMock = new Mock<IConsole>();
            consoleMock.SetupGet(x => x.BufferWidth).Returns(() => _bufferWidth);
            consoleMock.SetupGet(x => x.CursorLeft).Returns(() => _currentIndex % _bufferWidth);
            consoleMock.SetupGet(x => x.CursorTop).Returns(() => _currentIndex / _bufferWidth);
            consoleMock.Setup(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()))
                .Callback<int, int>(SetCursor);
            consoleMock.Setup(x => x.Write(It.IsAny<char>())).Callback<char>(Write);
            consoleMock.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(Write);
            var console = consoleMock.Object;
            return console;
        }

        private static ConsoleWriter BuildWriter(IConsole console)
        {
            var writer = new ConsoleWriter(console);
            return writer;
        }

        private void Write(char obj)
        {
            _builder[_currentIndex] = obj;
            _currentIndex++;
        }

        private void Write(string obj)
        {
            foreach (var c in obj)
            {
                Write(c);
            }
        }

        private void SetCursor(int x, int y)
        {
            _currentIndex = x + y * _bufferWidth;
        }

        [Fact]
        public void WriteAndBackspaceWithRemainingCharactersTest()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'a');
            writer.Write(buffer, 'b');
            writer.Write(buffer, 'c');
            writer.Write(buffer, 'd');
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            Assert.Equal("a", buffer.ToString());
            Assert.Equal("a", GetBuffer());
        }

        private IEnumerable<char> GetBuffer()
        {
            return String.Concat(_builder.Where(x => x != '\0')).TrimEnd(' ');
        }

        [Fact]
        public void WriteMoveAndBackspace()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'a');
            writer.Write(buffer, 'b');
            writer.Write(buffer, 'c');
            writer.Write(buffer, 'd');
            writer.MoveCursorLeft();
            writer.MoveCursorLeft();
            writer.Backspace(buffer);
            writer.Write(buffer, 'x');
            Assert.Equal("axcd", buffer.ToString());
            Assert.Equal("axcd", GetBuffer());
        }

        [Fact]
        public void WriteMoveAndWrite()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'a');
            writer.Write(buffer, 'b');
            writer.Write(buffer, 'c');
            writer.Write(buffer, 'd');
            writer.MoveCursorLeft();
            writer.MoveCursorLeft();
            writer.MoveCursorRight();
            writer.MoveCursorLeft();
            writer.Write(buffer, '_');
            Assert.Equal("ab_cd", buffer.ToString());
            Assert.Equal("ab_cd", GetBuffer());
        }


        [Fact]
        public void WriteMoveBackspaceMixed()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'f');
            writer.Write(buffer, 'b');
            writer.Write(buffer, 'a');
            writer.Write(buffer, 'r');
            writer.MoveCursorLeft();
            writer.MoveCursorLeft();
            writer.MoveCursorLeft();
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'o');
            writer.MoveCursorRight();
            writer.MoveCursorRight();
            writer.Backspace(buffer);
            writer.Backspace(buffer);
            writer.Write(buffer, 'B');
            writer.Write(buffer, '@');
            writer.MoveCursorLeft();
            writer.Write(buffer, 'A');
            Assert.Equal("fooBA@r", buffer.ToString());
            Assert.Equal("fooBA@r", GetBuffer());
        }

        [Fact]
        public void WriteMoveHomeEndBackspaceMixed()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'f');
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'o');
            for (int i = 0; i < 10; i++)
            {
                writer.Write(buffer, 'x');
            }
            writer.MoveCursorHome();
            for (int i = 0; i < 12; i++)
            {
                writer.MoveCursorRight();
            }
            for (int i = 0; i < 9; i++)
            {
                writer.Backspace(buffer);
            }
            writer.MoveCursorEnd();
            writer.Backspace(buffer);
            Assert.Equal("foo", buffer.ToString());
            Assert.Equal("foo", GetBuffer());
        }

        [Fact]
        public void WriteMoveLeftAndBackspaceMoveEnd()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'f');
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'b');
            Assert.Equal(4, _currentIndex);
            writer.MoveCursorLeft();
            Assert.Equal(3, _currentIndex);
            writer.Backspace(buffer);
            Assert.Equal(2, _currentIndex);
            writer.Write(buffer, 'o');
            Assert.Equal(3, _currentIndex);
            writer.MoveCursorEnd();
            Assert.Equal(4, _currentIndex);
            Assert.Equal("foob", buffer.ToString());
            Assert.Equal("foob", GetBuffer());
        }

        [Fact]
        public void WriteMoveHomeEndBackspaceMixedIntoTheEndOfBuffer()
        {
            var console = BuildConsole();
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'f');
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'o');
            Assert.Equal(3, _currentIndex);
            for (int i = 0; i < 90; i++)
            {
                writer.Write(buffer, 'x');
            }
            Assert.Equal(93, _currentIndex);
            writer.MoveCursorHome();
            Assert.Equal(0, _currentIndex);
            for (int i = 0; i < 92; i++)
            {
                writer.MoveCursorRight();
            }
            Assert.Equal(92, _currentIndex);
            for (int i = 0; i < 89; i++)
            {
                writer.Backspace(buffer);
            }
            Assert.Equal(3, _currentIndex);
            writer.MoveCursorEnd();
            Assert.Equal(4, _currentIndex);
            writer.Backspace(buffer);
            Assert.Equal(3, _currentIndex);
            Assert.Equal("foo", buffer.ToString());
            Assert.Equal("foo", GetBuffer());
        }


        [Fact]
        public void WriteMoveHomeEndBackspaceMixedAroundTheEndOfBufferWithPrompt()
        {
            var console = BuildConsole();
            Write('>');
            var writer = BuildWriter(console);
            var buffer = new CommandLineBuffer();
            writer.Write(buffer, 'f');
            writer.Write(buffer, 'o');
            writer.Write(buffer, 'o');
            Assert.Equal(4, _currentIndex);
            for (int i = 0; i < 90; i++)
            {
                writer.Write(buffer, 'x');
            }
            Assert.Equal(94, _currentIndex);
            writer.MoveCursorHome();
            Assert.Equal(1, _currentIndex);
            for (int i = 0; i < 92; i++)
            {
                writer.MoveCursorRight();
            }
            Assert.Equal(93, _currentIndex);
            for (int i = 0; i < 89; i++)
            {
                writer.Backspace(buffer);
            }
            Assert.Equal(4, _currentIndex);
            writer.MoveCursorEnd();
            Assert.Equal(5, _currentIndex);
            writer.Backspace(buffer);
            Assert.Equal(4, _currentIndex);
            Assert.Equal("foo", buffer.ToString());
            Assert.Equal(">foo", GetBuffer());
        }

    }
}