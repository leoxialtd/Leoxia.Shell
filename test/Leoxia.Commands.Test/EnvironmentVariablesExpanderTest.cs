using System;
using System.Collections.Generic;
using System.Diagnostics;
using Leoxia.Commands;
using Moq;
using Xunit;

namespace Leoxia.ReadLine.Test
{
    public class EnvironmentVariablesExpanderTest
    {
        private static IEnvironmentVariablesProvider BuildProvider()
        {
            var providerMock = new Mock<IEnvironmentVariablesProvider>();
            providerMock.Setup(x => x.GetVariables()).Returns(new List<EnvironmentVariable>
            {
                new EnvironmentVariable {Key = "USERNAME", Value = "Bar"}
            });
            var provider = providerMock.Object;
            return provider;
        }

        private static EnvironmentVariablesExpander BuildExpander()
        {
            return new EnvironmentVariablesExpander(BuildProvider());
        }


        [Fact]
        public void ExpandVariablesCmdStyles()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello %USERNAME%");
            Assert.Equal("Hello Bar", res);
        }

        [Fact]
        public void ExpandVariablesBashStyle()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello $USERNAME");
            Assert.Equal("Hello Bar", res);
        }

        [Fact]
        public void ExpandVariablesDoubleDollar()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello $$USERNAME");
            Assert.Equal($"Hello {Process.GetCurrentProcess().Id}USERNAME", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyle()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello ${USERNAME}");
            Assert.Equal("Hello Bar", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyleInsideDoubleQuotes()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello \"${USERNAME}\"");
            Assert.Equal("Hello \"Bar\"", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyleInsideSingleQuotes()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello '${USERNAME}'");
            Assert.Equal("Hello '${USERNAME}'", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyleInsideDeactivatedSingleQuotes()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello \\'${USERNAME}\\'");
            Assert.Equal("Hello \\'Bar\\'", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyleInsideDoubleSingleQuotes()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello \"'${USERNAME}'\"");
            Assert.Equal("Hello \"'Bar'\"", res);
        }

        [Fact]
        public void ExpandVariablesBashOtherStyleInsideSingleDoubleQuotes()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello '\"${USERNAME}\"'");
            Assert.Equal("Hello '\"${USERNAME}\"'", res);
        }



        [Fact]
        public void ExpandVariablesBashOtherStyleInsideBraces()
        {
            var expander = BuildExpander();
            var res = expander.Expand("Hello \"${Foo${USERNAME}}\"");
            Assert.Equal("Hello \"${FooBar}\"", res);
        }
    }
}
