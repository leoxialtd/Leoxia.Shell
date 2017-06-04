using System;
using System.Collections.Generic;
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
            return Split(commandLine.Trim(' '), tokenStateChecker.IsATokenDelimiter);
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
    }
}
