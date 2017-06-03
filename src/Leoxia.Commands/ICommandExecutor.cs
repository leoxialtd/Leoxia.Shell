using System.Net.Sockets;

namespace Leoxia.Commands
{
    public interface ICommandExecutor
    {
        CommandResult Executor(string rawLine);
    }

    public class CommandExecutor : ICommandExecutor
    {
        public CommandResult Executor(string rawLine)
        {
            var command = rawLine.Trim(' ');
            if (command == "exit" || command == "quit")
            {
                return CommandResult.Exit;
            }
            return CommandResult.Continue;
        }
    }

    public class CommandResult
    {
        public bool IsExit { get; }

        public static readonly CommandResult Continue = new CommandResult(false);

        public static readonly CommandResult Exit = new CommandResult(true);

        private CommandResult(bool isExit)
        {
            IsExit = isExit;
        }
    }
}
