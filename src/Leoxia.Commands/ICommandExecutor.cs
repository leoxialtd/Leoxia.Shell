using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace Leoxia.Commands
{
    public interface ICommandExecutor
    {
        CommandResult Execute(string rawLine);
    }
}
