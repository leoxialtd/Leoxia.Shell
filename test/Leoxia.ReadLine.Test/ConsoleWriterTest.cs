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
        private readonly char[] _builder = new char[80];

        [Fact]
        public void WriteAndBackspaceTest()
        {
            var console = BuildConsole();
            var writer = new ConsoleWriter(console);
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
            consoleMock.SetupGet(x => x.BufferWidth).Returns(() => 80);
            consoleMock.SetupGet(x => x.CursorLeft).Returns(() => _currentIndex);
            consoleMock.Setup(x => x.SetCursorPosition(It.IsAny<int>(), It.IsAny<int>()))
                .Callback<int, int>(SetCursor);
            consoleMock.Setup(x => x.Write(It.IsAny<char>())).Callback<char>(Write);
            consoleMock.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(Write);
            var console = consoleMock.Object;
            return console;
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
            _currentIndex = x;
        }

        [Fact]
        public void WriteAndBackspaceWithRemainingCharactersTest()
        {
            var console = BuildConsole();
            var writer = new ConsoleWriter(console);
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
            var writer = new ConsoleWriter(console);
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
            var writer = new ConsoleWriter(console);
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
            var writer = new ConsoleWriter(console);
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
    }
}
