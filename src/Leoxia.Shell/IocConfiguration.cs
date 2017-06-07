using DryIoc;
using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;
using Leoxia.Commands;
using Leoxia.Commands.External;
using Leoxia.Commands.Threading;
using Leoxia.Implementations;
using Leoxia.Implementations.IO;
using Leoxia.ReadLine;
using ExecutableResolver = Leoxia.Commands.ExecutableResolver;

namespace Leoxia.Shell
{
    public class IocConfiguration
    {
        public void Configure(Container container)
        {
            container.Register<IConsole, ConsoleAdapter>(Reuse.Singleton);
            container.RegisterInstance<ISafeConsole>(SafeConsoleAdapter.Instance, Reuse.Singleton);
            container.Register<IExecutableResolver, ExecutableResolver>(Reuse.Singleton);
            container.Register<IProgramRunnerFactory, ProgramRunnerFactory>(Reuse.Singleton);
            container.Register<IEnvironmentVariablesExpander, EnvironmentVariablesExpander>(Reuse.Singleton);
            container.Register<IEnvironmentVariablesProvider, EnvironmentVariablesProvider>(Reuse.Singleton);
            container.Register<ILinkManager, LinkManager>(Reuse.Singleton);
            container.Register<IWin32LinkResolver, Win32LinkResolver>(Reuse.Singleton);
            container.Register<IUnixLinkResolver, UnixLinkResolver>(Reuse.Singleton);
            container.Register<IFileInfoFactory, FileInfoFactory>(Reuse.Singleton);
            container.Register<IKeyHandler, KeyHandler>(Reuse.Singleton);
            container.Register<IConsoleInputHandler, ConsoleInputHandler>(Reuse.Singleton);
            container.Register<ICommandExecutor, CommandExecutor>(Reuse.Singleton);
            container.Register<IHistoryNavigator, HistoryNavigator>(Reuse.Singleton);
            container.Register<ITimeProvider, TimeProvider>(Reuse.Singleton);
            container.Register<IDirectory, DirectoryAdapter>(Reuse.Singleton);
            container.Register<IPromptProvider, PromptProvider>(Reuse.Singleton);
            container.Register<IConsoleWriter, ConsoleWriter>(Reuse.Singleton);
            container.Register<IConsoleConfigurator, ConsoleConfigurator>(Reuse.Singleton);
            container.Register<IFileSystemInfoFactory, FileSystemInfoFactory>(Reuse.Singleton, made: Made.Of(() => new FileSystemInfoFactory()));
        }
    }
}