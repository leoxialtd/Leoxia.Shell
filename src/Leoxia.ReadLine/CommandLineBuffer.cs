using System.Text;

namespace Leoxia.ReadLine
{
    public class CommandLineBuffer
    {
        private readonly StringBuilder _builder;

        public CommandLineBuffer()
        {
            _builder  = new StringBuilder();
        }

        public CommandLineBuffer(string value)
        {
            _builder = new StringBuilder(value);
        }

        public int Length => _builder.Length;

        public static readonly CommandLineBuffer Empty = new CommandLineBuffer();

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return _builder.ToString();
        }

        public void Remove(int index, int length)
        {
            _builder.Remove(index, length);
        }

        public void Insert(int index, char value)
        {
            _builder.Insert(index, value);
        }

        public void Append(char value)
        {
            _builder.Append(value);
        }
    }
}