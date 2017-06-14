using Xunit;

namespace Leoxia.Commands.Infrastructure.Test
{
    public class EscapeSpaceTest
    {
        [Fact]
        public void SimpleEscapeTest()
        {
            var res = CommandLine.RemoveEscapedSpaces("C:\\Test\\ Foo\\");
            Assert.Equal("C:\\Test Foo\\", res);
        }

        [Fact]
        public void SimpleEscapeWithQuotesTest()
        {
            var res = CommandLine.RemoveEscapedSpaces("'C:\\Test\\ Foo\\'");
            Assert.Equal("'C:\\Test\\ Foo\\'", res);
        }

        [Fact]
        public void SimpleEscapeWithDoublesQuotesTest()
        {
            var res = CommandLine.RemoveEscapedSpaces("\"C:\\Test\\ Foo\\\"");
            Assert.Equal("\"C:\\Test\\ Foo\\\"", res);
        }

        [Fact]
        public void DoubleEscapeTest()
        {
            var res = CommandLine.RemoveEscapedSpaces("C:\\Test\\\\ Foo\\");
            Assert.Equal("C:\\Test\\\\ Foo\\", res);
        }
    }
}
