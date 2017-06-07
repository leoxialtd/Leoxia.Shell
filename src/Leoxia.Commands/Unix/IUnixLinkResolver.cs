using Leoxia.Commands.External;

namespace Leoxia.Commands
{
    public interface IUnixLinkResolver
    {
        string Resolve(string path);
    }

    public class UnixLinkResolver : IUnixLinkResolver
    {
        private readonly IExecutableResolver _resolver;
        private readonly IProgramRunnerFactory _runnerFactory;

        public UnixLinkResolver(IExecutableResolver resolver, IProgramRunnerFactory runnerFactory)
        {
            _resolver = resolver;
            _runnerFactory = runnerFactory;
        }

        public string Resolve(string path)
        {
            if (!string.IsNullOrEmpty(_resolver.Resolve("readlink")))
            {
                var runner = _runnerFactory.CreateRunner("readlink " + path);
                return runner.AsyncRun().Result;
            }
            return null;
        }
    }
}