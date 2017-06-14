using Leoxia.Collections;
using Leoxia.Commands.Infrastructure;
using Xunit;

namespace Leoxia.Text.Extensions.Test
{
    public class CommandLineTest
    {

        [Fact]
        public void SimpleCaseTest()
        {
            var tokens = CommandLine.Split("my.exe wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("my.exe", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void BackSlashSpaceCaseTest()
        {
            var tokens = CommandLine.Split("C:\\Program\\ Files\\my.exe wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("C:\\Program\\ Files\\my.exe", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void DoubleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("\"C:\\Program Files\\my.exe\" wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("\"C:\\Program Files\\my.exe\"", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void FollowingDoubleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("\"C:\\Program Files\\my\"\".exe\" wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("\"C:\\Program Files\\my\"\".exe\"", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void UnbalancedDoubleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("\"C:\\Program Files\\my.exe wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("\"C:\\Program Files\\my.exe wants to run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void MixedQuoteCaseTest()
        {
            var tokens = CommandLine.Split("\"'C:\\Program Files\\my.exe\" wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("\"'C:\\Program Files\\my.exe\"", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void AnotherMixedQuoteCaseTest()
        {
            var tokens = CommandLine.Split("\"'C:\\Program Files\\my.exe\" wants' to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("\"'C:\\Program Files\\my.exe\"", cursor.Next());
                Assert.Equal("wants' to run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void EscapedQuoteCaseTest()
        {
            var tokens = CommandLine.Split("C:\\Program Files\\my.exe\\\" wants\" to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("C:\\Program", cursor.Next());
                Assert.Equal("Files\\my.exe\\\"", cursor.Next());
                Assert.Equal("wants\" to run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void EscapedEscapeQuoteCaseTest()
        {
            var tokens = CommandLine.Split("C:\\Program Files\\my.exe\\\\\" wants\" to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("C:\\Program", cursor.Next());
                Assert.Equal("Files\\my.exe\\\\\" wants\"", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void DoubleQuoteInsideSingleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("'\"C:\\Program Files\\my.exe\"' wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("'\"C:\\Program Files\\my.exe\"'", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void SeveralWhiteSpaceTest()
        {
            var tokens = CommandLine.Split("my.exe    wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("my.exe", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void SeveralWhiteSpaceAtTheBegginningTest()
        {
            var tokens = CommandLine.Split("   my.exe    wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("my.exe", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }


        [Fact]
        public void SingleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("'C:\\Program Files\\my.exe' wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("'C:\\Program Files\\my.exe'", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void FollowingSingleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("'C:\\Program Files\\my.''exe' wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("'C:\\Program Files\\my.''exe'", cursor.Next());
                Assert.Equal("wants", cursor.Next());
                Assert.Equal("to", cursor.Next());
                Assert.Equal("run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void UnbalancedSingleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("'C:\\Program Files\\my.exe wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("'C:\\Program Files\\my.exe wants to run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }

        [Fact]
        public void BalancedUnbalancedSingleQuoteCaseTest()
        {
            var tokens = CommandLine.Split("'C:\\Program Files\\my.exe' 'wants to run");
            using (var cursor = tokens.GetCursor())
            {
                Assert.Equal("'C:\\Program Files\\my.exe'", cursor.Next());
                Assert.Equal("'wants to run", cursor.Next());
                Assert.Equal(null, cursor.Next());
            }
        }
    }
}