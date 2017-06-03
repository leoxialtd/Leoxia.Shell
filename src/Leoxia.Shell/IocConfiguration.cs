using DryIoc;
using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;
using Leoxia.Commands;
using Leoxia.Implementations;
using Leoxia.Implementations.IO;
using Leoxia.ReadLine;

namespace Leoxia.Shell
{
    public class IocConfiguration
    {
        public void Configure(Container container)
        {
            container.Register<IConsole, ConsoleAdapter>(Reuse.Singleton);
            container.Register<IKeyHandler, KeyHandler>(Reuse.Singleton);
            container.Register<IConsoleInputHandler, ConsoleInputHandler>(Reuse.Singleton);
            container.Register<ICommandExecutor, CommandExecutor>(Reuse.Singleton);
            container.Register<IHistoryNavigator, HistoryNavigator>(Reuse.Singleton);
            container.Register<ITimeProvider, TimeProvider>(Reuse.Singleton);
            container.Register<IDirectory, DirectoryAdapter>(Reuse.Singleton);
            container.Register<IPromptProvider, PromptProvider>(Reuse.Singleton);
            container.Register<IConsoleWriter, ConsoleWriter>(Reuse.Singleton);
        }
    }
}