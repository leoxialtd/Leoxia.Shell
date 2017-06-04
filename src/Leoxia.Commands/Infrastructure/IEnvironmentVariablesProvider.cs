using System.Collections.Generic;

namespace Leoxia.Commands
{
    public interface IEnvironmentVariablesProvider
    {
        IEnumerable<EnvironmentVariable> GetVariables();
    }
}