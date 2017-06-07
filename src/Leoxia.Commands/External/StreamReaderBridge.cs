using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Leoxia.Commands.External
{
    public class StreamReaderBridge
    {
        private readonly StreamReader _reader;
        private readonly IWriteBuffer _writer;
        private readonly StringBuilder _buffer = new StringBuilder();
        private volatile bool _reading;

        public StreamReaderBridge(StreamReader reader, IWriteBuffer writer)
        {
            _reader = reader;
            _writer = writer;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _buffer.ToString();
        }

        public void BeginRead()
        {
            _reading = true;
            Task.Run(() =>
            {
                while (_reading)
                {
                    var i = _reader.Read();
                    var c = (char) i;
                    _buffer.Append(c);
                    _writer.Write(c);
                }
            });
        }

        public void EndRead()
        {
            _reading = false;
        }
    }
}