using System;
using System.Collections.Generic;
using System.Text;
using Leoxia.Commands;

namespace Leoxia.Commands.Transform.Variables
{
    public class VariableExpanderPipe : ICommandTransformPipe
    {
        private readonly IEnvironmentVariablesExpander _expander;

        public VariableExpanderPipe(IEnvironmentVariablesExpander expander)
        {
            _expander = expander;
        }

        public string Transform(string commandLine)
        {
            return _expander.Expand(commandLine);
        }
    }
}
