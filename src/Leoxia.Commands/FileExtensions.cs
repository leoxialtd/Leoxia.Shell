using System.Collections.Generic;

namespace Leoxia.Commands
{
    internal class FileExtensions
    {
        public static readonly HashSet<string> Compressed =
            new HashSet<string>
            {
                "tar.gz", "bz2", "zip", "rar",
                "7z", "tgz", "arj", "tar", "cab",
                "gz", "lz", "lzma", "lzo", "deb",
                "rz", "z", "ace", "lzh", "apk",
                "lha", "jar", "nupkg"
            };
    }
}