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
        private readonly IProgramRunner _runner;

        public UnixLinkResolver(IExecutableResolver resolver, IProgramRunner runner)
        {
            _resolver = resolver;
            _runner = runner;
        }

        public string Resolve(string path)
        {
            if (!string.IsNullOrEmpty(_resolver.Resolve("readlink")))
            {
                return _runner.Run("readlink " + path);
            }
            return null;
        }
    }
}