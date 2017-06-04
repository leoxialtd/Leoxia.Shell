using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Leoxia.Abstractions.IO;

namespace Leoxia.Commands
{
    public interface IExecutableResolver
    {
        string Resolve(string path);
    }

    public class ExecutableResolver : IExecutableResolver
    {
        private readonly IFileInfoFactory _factory;
        private readonly string[] _directories;

        public ExecutableResolver(IFileInfoFactory factory)
        {
            _factory = factory;
            // Replace this with a Lucene.net index
            _directories = SplitPath();
        }

        public string Resolve(string processName)
        {            
            var localFile = _factory.Build(processName);
            if (localFile.Exists)
            {
                return localFile.FullName;
            }
            StringBuilder builder = new StringBuilder();
            foreach (var directory in _directories)
            {
                builder.AppendFormat("Try to find {0} in {1}", processName, directory);
                builder.AppendLine();
                string s = Resolve(processName, directory);
                if (s != null)
                {
                    return s;
                }
                if (!processName.EndsWith(".exe"))
                    s = Resolve(processName + ".exe", directory);
                if (s != null)
                {
                    return s;
                }
            }
            return null;
        }

        public string Resolve(string processName, string directory)
        {
            var absolutePath = Path.Combine(directory, processName);
            string fullName = CheckPath(absolutePath);
            if (fullName != null)
            {
                return fullName;
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && !absolutePath.EndsWith(".exe"))
            {
                absolutePath = absolutePath + ".exe";
                fullName = CheckPath(absolutePath);
                if (fullName != null)
                {
                    return fullName;
                }
            }
            return null;
        }

        private string CheckPath(string absolutePath)
        {
            var absoluteFile = _factory.Build(absolutePath);
            if (absoluteFile.Exists)
            {
                return absoluteFile.FullName;
            }
            return null;
        }

        private static string[] SplitPath()
        {

            char splitCharacter;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                splitCharacter = ';';
            }
            else
            {
                splitCharacter = ':';
            }
            return Environment.GetEnvironmentVariable("PATH").Split(splitCharacter);
        }
    }
}