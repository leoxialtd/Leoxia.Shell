using System;
using System.Collections.Generic;
using System.Text;

namespace Leoxia.Commands
{
    public class MatchingCandidate
    {
        private string _startDelimiter;
        private string _endDelimiter = string.Empty;

        private readonly List<char> _chars = new List<char>();
        private bool _requireEndDelimiter;

        public MatchingCandidate(string str, int index)
        {
            StartIndex = index;
            var c = str[index];
            _requireEndDelimiter = c == '%';
            _startDelimiter = c.ToString();
        }

        public void Apply(StringBuilder str)
        {
            str.Replace(Pattern, Value, StartIndex, Pattern.Length);
        }

        public int StartIndex { get; }

        public string Pattern { get; private set; }

        public string Value { get; set; }

        public string Key { get; private set; }

        public void AppendDelimiterEnd(char c)
        {
            _endDelimiter += c;
        }

        public void Append(char c)
        {
            _chars.Add(c);
        }

        public void AppendDelimiterStart(char c)
        {
            _requireEndDelimiter = c == '{';
            _startDelimiter += c;
        }

        public bool Validate()
        {
            if (_requireEndDelimiter && _endDelimiter.Length == 0)
            {
                return false;
            }
            Key = String.Concat(_chars);
            Pattern = _startDelimiter + Key + _endDelimiter;
            return true;
        }
    }
}