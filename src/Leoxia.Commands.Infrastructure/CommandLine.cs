using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Leoxia.Commands.Infrastructure
{
    public static class CommandLine
    {
        /// <summary>
        /// Splits the specified command line with spaces as delimiters.
        /// </summary>
        /// <param name="commandLine">The command line.</param>
        /// <returns>The list of tokens</returns>
        public static IEnumerable<string> Split(string commandLine)
        {
            var tokenStateChecker = new TokenDelimiterPredicator();
            return Split(commandLine.Trim(' ', '\t', '\r', '\n'), tokenStateChecker.IsATokenDelimiter);
        }

        private static IEnumerable<string> Split(string str,
            Predicate<char> isATokenDelimiter)
        {
            int nextToken = 0;
            var lastCharacterIsDelimiter = false;
            for (int c = 0; c < str.Length; c++)
            {
                var isDelimiter = isATokenDelimiter(str[c]);
                if (isDelimiter)
                {
                    if (!lastCharacterIsDelimiter)
                    {
                        yield return str.Substring(nextToken, c - nextToken);
                    }
                    nextToken = c + 1;
                }
                lastCharacterIsDelimiter = isDelimiter;
            }
            yield return str.Substring(nextToken);
        }

        /// <summary>
        /// Trims the matching quotes from the start and the end. Only removes the first and last character
        /// if they match.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns></returns>
        public static IEnumerable<string> TrimMatchingQuotes(this IEnumerable<string> enumerable)
        {
            return enumerable.Select(RemoveMatchingQuotes);
        }

        public static string RemoveMatchingQuotes(string input)
        {
            if (input.Length > 1 && input[0] == input.Last())
            {
                if (input[0] == '"' || input[0] == '\'')
                {
                    return input.Substring(1, input.Length - 2);
                }
            }
            return input;
        }

        public static string RemoveEscapedSpaces(string path)
        {
            var predicator = new EscapedSpacePredicator();
            return RemoveEscapedSpaces(path.Trim(' ', '\t', '\r', '\n'), predicator.IsEscapedSpace);
        }

        private static string RemoveEscapedSpaces(string str, Func<char, char, bool> isEscapedSpace)
        {
            List<char> result = new List<char>();
            if (str.Length < 2)
            {
                return str;
            }
            for (int i = 0; i < str.Length - 1; i++)
            {
                char c1 = str[i];
                char c2 = str[i + 1];
                if (!isEscapedSpace(c1, c2))
                {
                    result.Add(c1);
                }
            }
            result.Add(str[str.Length - 1]);
            return String.Concat(result);
        }
    }

    public class EscapedSpacePredicator
    {
        private bool _isEscaped;
        private bool _inSingleQuotes;
        private bool _inDoubleQuotes;

        public bool IsEscapedSpace(char arg1, char arg2)
        {
            if (_isEscaped)
            {
                _isEscaped = false;
                return false;
            }
            if (arg1 == '\"')
            {
                if (!_inSingleQuotes)
                {
                    _inDoubleQuotes = !_inDoubleQuotes;
                }
                return false;
            }
            if (arg1 == '\'')
            {
                if (!_inDoubleQuotes)
                {
                    _inSingleQuotes = !_inSingleQuotes;
                }
                return false;
            }
            if (_inDoubleQuotes || _inSingleQuotes)
            {
                return false;
            }
            if (arg1 == '\\')
            {
                _isEscaped = true;
                if (arg2 == ' ')
                {
                    return true;
                }
            }
            return false;
        }
    }
}
