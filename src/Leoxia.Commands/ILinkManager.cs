using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public interface ILinkManager
    {
        bool TryGetLink(IFileSystemInfo path, out string target);
    }
}