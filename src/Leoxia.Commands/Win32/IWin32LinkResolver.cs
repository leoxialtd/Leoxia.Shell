using Leoxia.Commands.Win32;

namespace Leoxia.Commands
{
    public interface IWin32LinkResolver
    {
        string Resolve(string path);
    }

    public class Win32LinkResolver : IWin32LinkResolver
    {
        public string Resolve(string path)
        {
            return NativeMethods.GetFinalPathName(path);
        }
    }
}