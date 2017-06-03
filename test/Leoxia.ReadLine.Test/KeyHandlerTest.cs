using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using Leoxia.Implementations.IO;
using Xunit;

namespace Leoxia.ReadLine.Test
{
    public class KeyHandlerTest
    {
        [Fact]
        public void TestKeyBuild()
        {
            var key = new ControlSequence(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, false)).GetHashCode();
            Assert.Equal(65, key);
            key = new ControlSequence(new ConsoleKeyInfo('A', ConsoleKey.A, false, false, true)).GetHashCode();
            Assert.Equal(1024 + 65, key);
            key = new ControlSequence(new ConsoleKeyInfo('A', ConsoleKey.OemClear, true, false, false)).GetHashCode();
            Assert.Equal(512 + 254, key);
            key = new ControlSequence(new ConsoleKeyInfo('A', ConsoleKey.OemClear, false, true, false)).GetHashCode();
            Assert.Equal(256 + 254, key);
        }
    }
}
