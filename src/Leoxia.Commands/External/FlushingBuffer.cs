using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands.External
{
    public sealed class FlushingBuffer : IWriteBuffer, IDisposable
    {
        private readonly List<char> _buffer = new List<char>();
        private readonly ITextWriter _writer;
        private volatile bool _running;
        private readonly object _synchro = new object();
        private static readonly char[] _empty = new char[0];

        public FlushingBuffer(ITextWriter writer)
        {
            _writer = writer;
            _running = true;
            Task.Run((Action)Loop);
        }

        private void Loop()
        {
            while (_running)
            {
                Flush();
                Task.Delay(200).Wait();
            }
        }

        public void Write(char value)
        {
            lock (_synchro)
            {
                if (value != '\uffff')
                {
                    _buffer.Add(value);
                }
            }
        }

        public void Flush()
        {
            char[] copy = _empty;
            lock (_synchro)
            {
                if (_buffer.Count > 0)
                {
                    copy = _buffer.ToArray();
                    _buffer.Clear();
                }
            }
            if (copy.Length > 0)
            {
                _writer.Write(copy);
            }
        }

        public void Dispose()
        {
            _running = false;
        }
    }
}