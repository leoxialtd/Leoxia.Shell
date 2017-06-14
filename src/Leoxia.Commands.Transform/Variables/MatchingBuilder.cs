using System;
using System.Collections.Generic;
using System.Linq;

namespace Leoxia.Commands
{
    public class MatchingBuilder
    {
        private readonly VariableDelimiter _delimiter;
        private readonly List<MatchingCandidate> _openCandidates =
            new List<MatchingCandidate>();

        public MatchingBuilder()
        {
            _delimiter = new VariableDelimiter();
            Candidates = new List<MatchingCandidate>();
        }

        public void CheckMatch(string str, int index)
        {
            var c = str[index];
            DelimiterType typeOfDelimiter;
            if (_delimiter.IsDelimiter(c, out typeOfDelimiter))
            {
                switch (typeOfDelimiter)
                {
                    case DelimiterType.Start:
                        var candidate = new MatchingCandidate(str, index);
                        _openCandidates.Add(candidate);
                        Candidates.Add(candidate);
                        break;
                    case DelimiterType.End:
                        var matchingCandidate = _openCandidates.Last();
                        matchingCandidate.AppendDelimiterEnd(c);
                        _openCandidates.Remove(matchingCandidate);
                        break;
                    case DelimiterType.StartContinue:
                        _openCandidates.Last().AppendDelimiterStart(c);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                foreach (var candidate in _openCandidates)
                {
                    candidate.Append(c);
                }
            }
        }

        public List<MatchingCandidate> Candidates { get; }
    }

    public enum DelimiterType
    {
        Start,
        End,
        StartContinue,
        No
    }
}