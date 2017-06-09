using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Leoxia.CommandTransform.Aliases;
using Xunit;

namespace Leoxia.CommandTransform.Test
{
    public class AliasExpanderTest
    {
        [Fact]
        public void Simple_Expand_Test()
        {
            var providerMock = new Mock<IAliasProvider>();
            providerMock.Setup(x => x.GetAliases()).Returns(new List<Alias>{ new Alias{ Key = "ll", Value = "ls -l"} });
            var provider = providerMock.Object;
            var expander = new AliasExpanderPipe(provider);
            var expanded = expander.Transform("ll");
            Assert.Equal("ls -l", expanded);
        }

        [Fact]
        public void Expand_With_SimilarName_Test()
        {
            var providerMock = new Mock<IAliasProvider>();
            providerMock.Setup(x => x.GetAliases()).Returns(new List<Alias> { new Alias { Key = "ll", Value = "ls -l" } });
            var provider = providerMock.Object;
            var expander = new AliasExpanderPipe(provider);
            var expanded = expander.Transform("lla");
            Assert.Equal("lla", expanded);
        }

        [Fact]
        public void Expand_With_SimpleQuotes_Test()
        {
            var providerMock = new Mock<IAliasProvider>();
            providerMock.Setup(x => x.GetAliases()).Returns(new List<Alias> { new Alias { Key = "ll", Value = "ls -l" } });
            var provider = providerMock.Object;
            var expander = new AliasExpanderPipe(provider);
            var expanded = expander.Transform("'ll'");
            Assert.Equal("'ll'", expanded);
        }

        [Fact]
        public void Expand_With_DoubleQuotes_Test()
        {
            var providerMock = new Mock<IAliasProvider>();
            providerMock.Setup(x => x.GetAliases()).Returns(new List<Alias> { new Alias { Key = "ll", Value = "ls -l" } });
            var provider = providerMock.Object;
            var expander = new AliasExpanderPipe(provider);
            var expanded = expander.Transform("\"ll\"");
            Assert.Equal("\"ll\"", expanded);
        }
    }
}
