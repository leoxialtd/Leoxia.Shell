using System;

namespace Leoxia.Commands
{
    public interface IExecutableResolver
    {
        string Resolve(string path);
    }
}