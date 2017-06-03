using System.Collections.Generic;
using System.Text;

namespace Leoxia.ReadLine
{
    /// <summary>
    /// interface for line reading (with control special sequences) Unix-like.
    /// </summary>
    public interface IConsoleInputHandler
    {
        string ReadLine();
    }
}
