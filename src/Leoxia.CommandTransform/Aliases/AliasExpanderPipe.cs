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
            var expansionMarks = new HashSet<string>();
            bool hasBeenExpanded;
            string result = commandLine;
            do
            {
                result = Expand(result, expansionMarks, out hasBeenExpanded);
            } while (hasBeenExpanded);
            return result;
        }

        private string Expand(string commandLine, HashSet<string> expansionMarks, out bool hasBeenExpanded)
        {
            hasBeenExpanded = false;
            var tokens = CommandLine.Split(commandLine);
            var expanded = new List<string>();
            var marks = new List<string>();
            foreach (var token in tokens)
            {
                string value;
                if (!expansionMarks.Contains(token) && _aliases.TryGetValue(token, out value))
                {
                    marks.Add(token);
                    expanded.Add(value);
                    hasBeenExpanded = true;
                }
                else
                {
                    expanded.Add(token);
                }
            }
            foreach (var mark in marks)
            {
                expansionMarks.Add(mark);
            }
            return string.Join(" ", expanded);
        }
    }
}
