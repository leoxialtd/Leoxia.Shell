using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Leoxia.Commands
{
    public interface IEnvironmentVariablesExpander
    {
        string Expand(string str);
    }

    public class EnvironmentVariablesExpander : IEnvironmentVariablesExpander
    {
        private readonly Dictionary<string, string> _variables;

        public EnvironmentVariablesExpander(IEnvironmentVariablesProvider provider)
        {
            _variables = provider.GetVariables().ToDictionary(x => x.Key, y => y.Value, GetEqualityComparer());
        }

        private IEqualityComparer<string> GetEqualityComparer()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return StringComparer.OrdinalIgnoreCase;
            }
            return StringComparer.Ordinal;
        }

        public string Expand(string str)
        {
            var builder = new StringBuilder(str);
            var matchingBuilder = new MatchingBuilder();
            for (int i = 0; i < str.Length; ++i)
            {
                matchingBuilder.CheckMatch(str, i);
            }
            var validCandidates = matchingBuilder.Candidates.Where(IsValid);
            foreach (var candidate in validCandidates)
            {
                candidate.Apply(builder);
            }
            return builder.ToString();
        }

        private bool IsValid(MatchingCandidate matchingCandidate)
        {
            if (matchingCandidate.Validate())
            {
                string value;
                if (_variables.TryGetValue(matchingCandidate.Key, out value))
                {
                    matchingCandidate.Value = value;
                    return true;
                }
            }
            return false;
        }
    }
}