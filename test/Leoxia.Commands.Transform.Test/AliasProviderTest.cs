using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Leoxia.Abstractions.IO;
using Xunit;
using Leoxia.Commands.Transform.Aliases;
using Moq;
using System.Linq;

namespace Leoxia.Commands.Transform.Test
{   
    public class AliasProviderTest
    {
        private string _saved;

        [Fact]
        public void Check_Empty_Alias_List_Is_Provided_On_NonExisting_File_Test()
        {
            var directorySystemMock = new Mock<IDirectory>();
            var directorySystem = directorySystemMock.Object;
            var fileSystemMock = new Mock<IFile>();
            var fileSystem = fileSystemMock.Object;
            var provider = new AliasProvider(directorySystem, fileSystem);
            List<Alias> aliases = provider.GetAliases().ToList();
            Assert.NotNull(aliases);
            Assert.Equal(0, aliases.Count);
        }

        [Fact]
        public void Check_NotEmpty_Alias_List_Is_Provided_On_Existing_File_Test()
        {
            var readerMock = new Mock<IStreamReader>();
            readerMock.Setup(x => x.ReadToEnd()).Returns("{ \"Key\": \"Value\" }");
            var reader = readerMock.Object;
            var directorySystemMock = new Mock<IDirectory>();
            var directorySystem = directorySystemMock.Object;
            var fileSystemMock = new Mock<IFile>();
            fileSystemMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            fileSystemMock.Setup(x => x.OpenText(It.IsAny<string>())).Returns(reader);
            var fileSystem = fileSystemMock.Object;
            var provider = new AliasProvider(directorySystem, fileSystem);
            List<Alias> aliases = provider.GetAliases().ToList();
            Assert.NotNull(aliases);
            Assert.Equal(1, aliases.Count);
            Assert.Equal("Key", aliases[0].Key);
            Assert.Equal("Value", aliases[0].Value);
        }

        [Fact]
        public void Check_Alias_Save_Test()
        {
            _saved = string.Empty;
            var writerMock = new Mock<IStreamWriter>();
            writerMock.Setup(x => x.Write(It.IsAny<string>())).Callback<string>(x =>
            {
                _saved = x;
            });
            var writer = writerMock.Object;
            var directorySystemMock = new Mock<IDirectory>();
            var directorySystem = directorySystemMock.Object;
            var fileSystemMock = new Mock<IFile>();
            fileSystemMock.Setup(x => x.CreateText(It.IsAny<string>())).Returns(writer);
            var fileSystem = fileSystemMock.Object;
            var provider = new AliasProvider(directorySystem, fileSystem);
            var aliases = provider.GetAliases();
            provider.Save();
            Assert.Equal("{}", _saved);
            aliases.Add(new Alias {Key = "Key", Value = "Value"});
            provider.Save();
            Assert.Equal("{\"Key\":\"Value\"}", _saved);
        }
    }
}
