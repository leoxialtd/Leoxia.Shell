using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leoxia.Commands.Infrastructure;

namespace Leoxia.CommandTransform.Aliases
{
    public class AliasExpanderPipe : ICommandTransformPipe
    {
        private readonly IAliasProvider _provider;
        private Dictionary<string, string> _aliases;

        public AliasExpanderPipe(IAliasProvider provider)
        {
            _provider = provider;
            _aliases = provider.GetAliases().ToDictionary(x => x.Key, y => y.Value);
        }

        public string Transform(string commandLine)
        {
            var tokens = CommandLine.Split(commandLine);
            var expanded = new List<string>();
            foreach (var token in tokens)
            {
                string value;
                if (_aliases.TryGetValue(token, out value))
                {
                    expanded.Add(value);
                }
                else
                {
                    expanded.Add(token);
                }
            }
            return string.Join(" ", expanded);
        }
    }
}
