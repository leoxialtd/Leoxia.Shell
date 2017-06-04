using System.Collections.Generic;

namespace Leoxia.Commands
{
    public class VariableDelimiter
    {
        private bool _previousIsDollar;
        private bool _ancestorIsCurlyBracket;
        private bool _ancestorIsPercent;
        private bool _isInSingleQuote;
        private bool _isInDoubleQuote;
        private bool _isEscaping;

        public bool IsDelimiter(char c, out DelimiterType type)
        {
            type = DelimiterType.No;
            if (c == '\\' && !_isEscaping)
            {
                _previousIsDollar = false;
                _isEscaping = true;
                return false;
            }
            if (c == '\'' && !_isInDoubleQuote && !_isEscaping)
            {
                _isInSingleQuote = !_isInSingleQuote;
            }
            else if (c == '"' && !_isInSingleQuote && !_isEscaping)
            {
                _isInDoubleQuote = !_isInDoubleQuote;
            }
            else if (c == '$' && !_isInSingleQuote && !_isEscaping)
            {
                _previousIsDollar = true;
                type = DelimiterType.Start;
                return true;
            }
            else if (c == '%' && !_isEscaping)
            {
                if (_ancestorIsPercent)
                {
                    _ancestorIsPercent = false;
                    type = DelimiterType.End;
                }
                else
                {
                    _ancestorIsPercent = true;
                    type = DelimiterType.Start;
                }
            }            
            else if (c == '{' && _previousIsDollar)
            {
                _ancestorIsCurlyBracket = true;
                type = DelimiterType.StartContinue;
            }
            else if (c == '}' && _ancestorIsCurlyBracket && !_isEscaping)
            {
                type = DelimiterType.End;
            }
            _isEscaping = false;
            _previousIsDollar = false;
            return type != DelimiterType.No;
        }
    }
}