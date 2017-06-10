using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Leoxia.ReadLine.Test
{
    public class CompletionTest
    {
        [Fact]
        public void AutoCompleteTest()
        {
            var completionNavigator = new CompletionNavigator();
            var results = completionNavigator.NextAutoComplete(new CommandLineBuffer("cm"));
            Assert.Equal(1, results.Length);
            Assert.Equal("cmd", results[0].ToString());
        }
    }
}
