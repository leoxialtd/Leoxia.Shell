using System.IO;
using System.Runtime.InteropServices;
using Leoxia.Abstractions.IO;
using Leoxia.Commands.Win32;
using Leoxia.Scripting.Commands;

namespace Leoxia.Commands
{
    public class LinkTools
    {
        public static bool IsLink(IFileSystemInfo path, out string target)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (IsWindowsLink(path))
                {
                    target = NativeMethods.GetFinalPathName(path.FullName);
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(ExecutableResolver.Resolve("readlink", false)))
            {
                target = Builtins.ReadLink(path.FullName);
                return !string.IsNullOrEmpty(target);
            }
            target = null;
            return false;
        }

        private static bool IsWindowsLink(IFileSystemInfo path)
        {
            return path.Exists && path.Attributes.HasFlag(FileAttributes.ReparsePoint);
        }
    }
}
