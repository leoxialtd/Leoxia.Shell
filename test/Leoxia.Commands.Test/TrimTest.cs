using System;
using System.Collections.Generic;
using System.Text;
using Leoxia.Commands.Infrastructure;
using Xunit;

namespace Leoxia.Commands.Test
{
    public class TrimTest
    {
        [Fact]
        public void TrimSimpleTest()
        {
            var res = CommandLine.RemoveMatchingQuotes("\"foo\"");
            Assert.Equal("foo", res);
        }

        [Fact]
        public void TrimQuoteEmptyTest()
        {
            var res = CommandLine.RemoveMatchingQuotes("\"\"");
            Assert.Equal(String.Empty, res);
        }

        [Fact]
        public void TrimStringEmptyTest()
        {
            var res = CommandLine.RemoveMatchingQuotes(string.Empty);
            Assert.Equal(String.Empty, res);
        }

        [Fact]
        public void TrimQuoteTest()
        {
            var res = CommandLine.RemoveMatchingQuotes("'Bar'");
            Assert.Equal("Bar", res);
        }

        [Fact]
        public void TrimSingleDoubleQuoteTest()
        {
            var res = CommandLine.RemoveMatchingQuotes("\"");
            Assert.Equal("\"", res);
        }

        [Fact]
        public void TrimUnbalancedTest()
        {
            var res = CommandLine.RemoveMatchingQuotes("\"Foo");
            Assert.Equal("\"Foo", res);
        }
    }
}
