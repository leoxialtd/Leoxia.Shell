using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;
using Moq;
using Xunit;

namespace Leoxia.Commands.Test
{
    public class ExecutableResolverTest
    {
        [Fact]
        public void ResolveEasyTest()
        {
            var factory = SetupFactory();
            var environmentMock = new Mock<IEnvironment>();
            var environment = environmentMock.Object;
            var resolver = new ExecutableResolver(factory, environment);
            string result = resolver.Resolve("C:\\Program\\ Files\\Git\\bin\\git.exe");
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("C:\\Program Files\\Git\\bin\\git.exe", result);
        }

        [Fact]
        public void ResolveQuoteTest()
        {
            var factory = SetupFactory();
            var environmentMock = new Mock<IEnvironment>();
            var environment = environmentMock.Object;
            var resolver = new ExecutableResolver(factory, environment);
            string result = resolver.Resolve("'C:\\Program Files\\Git\\bin\\git.exe'");
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal("C:\\Program Files\\Git\\bin\\git.exe", result);
        }

        private static IFileInfoFactory SetupFactory()
        {
            var fileName = "C:\\Program Files\\Git\\bin\\git.exe";
            var adapterMock = new Mock<IFileInfo>();
            adapterMock.SetupGet(x => x.FullName).Returns(fileName);
            adapterMock.SetupGet(x => x.Exists).Returns(true);
            var adapter = adapterMock.Object;
            var factoryMock = new Mock<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(fileName)).Returns(adapter);
            var factory = factoryMock.Object;
            return factory;
        }
    }
}
