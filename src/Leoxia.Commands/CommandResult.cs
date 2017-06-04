namespace Leoxia.Commands
{
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