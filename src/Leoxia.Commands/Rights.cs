namespace Leoxia.Commands
{
    public class Rights
    {
        public bool IsExecutable { get; set; }
        public bool IsReadable { get; set; }
        public bool IsWritable { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return (IsReadable ? "r" : "-") + (IsWritable ? "w" : "-") + (IsExecutable ? "x" : "-");
        }
    }
}