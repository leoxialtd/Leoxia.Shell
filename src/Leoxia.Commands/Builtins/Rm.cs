using System;
using System.Collections.Generic;
using System.Text;

namespace Leoxia.Commands.Builtins
{
    public class Rm : IBuiltin
    {
        public void Execute(List<string> arguments, List<Option> options)
        {
            
        }

        public string Command => "rm";
    }
}
