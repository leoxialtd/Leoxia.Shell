using System.Text;
using Leoxia.Abstractions.IO;

namespace Leoxia.Shell
{
    public interface IConsoleConfigurator
    {
        void Configure();
    }

    public class ConsoleConfigurator : IConsoleConfigurator
    {
        private readonly IConsole _console;

        public ConsoleConfigurator(IConsole console)
        {
            _console = console;
        }

        public void Configure()
        {
            _console.OutputEncoding = Encoding.Unicode;
            _console.TreatControlCAsInput = true;
            _console.OutputEncoding = Encoding.Unicode;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        }
    }
}