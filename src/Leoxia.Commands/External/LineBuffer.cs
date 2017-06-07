using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Threading;

namespace Leoxia.Commands.External
{
    public class LineBuffer : IWriteBuffer
    {
        private readonly ITextWriter _writer;
        private readonly ISafeConsole _console;
        private readonly StringBuilder _buffer = new StringBuilder();

        public LineBuffer(ITextWriter writer, ISafeConsole console)
        {
            _writer = writer;
            _console = console;
        }

        /// <summary>Writes a character to the text string or stream.</summary>
        /// <param name="value">The character to write to the text stream. </param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public void Write(char value)
        {
            _console.SafeCall(x =>
            {
                if (value != '\n')
                {
                    _buffer.Append(value);
                }
                else
                {
                    WriteLine(_buffer.ToString());
                    _buffer.Clear();
                }
            });
        }
        

        /// <summary>Writes a string followed by a line terminator to the text string or stream.</summary>
        /// <param name="value">The string to write. If <paramref name="value" /> is null, only the line terminator is written. </param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public void WriteLine(string value)
        {
            _console.SafeCall(console =>
            {
                var foreground = _console.ForegroundColor;
                console.ForegroundColor = ConsoleColor.Red;
                _writer.WriteLine(value);
                console.ForegroundColor = foreground;
            });
        }
    }

    public interface IWriteBuffer
    {
        void Write(char value);
    }
}