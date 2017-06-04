using System.Collections.Generic;

namespace Leoxia.Commands
{
    public interface IBuiltin
    {
        void Execute(List<string> tokens);
    }
}