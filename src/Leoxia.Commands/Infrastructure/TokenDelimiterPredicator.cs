namespace Leoxia.Commands.Infrastructure
{
    public class TokenDelimiterPredicator
    {
        private bool _inDoubleQuotes;
        private bool _inSingleQuotes;
        private bool _isEscaped;

        public bool IsATokenDelimiter(char c)
        {
            if (_isEscaped)
            {
                _isEscaped = false;
                return false;
            }
            if (c == '\\')
            {
                _isEscaped = true;
                return false;
            }
            if (c == '\"')
            {
                if (!_inSingleQuotes)
                {
                    _inDoubleQuotes = !_inDoubleQuotes;
                }
                return false;
            }
            if (c == '\'')
            {
                if (!_inDoubleQuotes)
                {
                    _inSingleQuotes = !_inSingleQuotes;
                }
                return false;
            }
            return !_inDoubleQuotes && !_inSingleQuotes && c == ' ';
        }
    }
}