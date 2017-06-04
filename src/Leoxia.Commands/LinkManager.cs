using System.IO;
using System.Runtime.InteropServices;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public class LinkManager : ILinkManager
    {
        private readonly IWin32LinkResolver _win32LinkResolver;
        private readonly IUnixLinkResolver _unixLinkResolver;

        public LinkManager(
            IWin32LinkResolver win32LinkResolver,
            IUnixLinkResolver unixLinkResolver)
        {
            _win32LinkResolver = win32LinkResolver;
            _unixLinkResolver = unixLinkResolver;
        }

        public bool TryGetLink(IFileSystemInfo path, out string target)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (IsWindowsLink(path))
                {
                    target = _win32LinkResolver.Resolve(path.FullName);
                    return true;
                }
            }
            // Can resolve Cygwin/Git/Msys bash links
            target = _unixLinkResolver.Resolve(path.FullName);
            return !string.IsNullOrEmpty(target);
        }

        private static bool IsWindowsLink(IFileSystemInfo path)
        {
            return path.Exists && path.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }
    }
}
