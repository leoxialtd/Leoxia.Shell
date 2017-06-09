using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Leoxia.Abstractions.IO;
using Newtonsoft.Json;

namespace Leoxia.CommandTransform.Aliases
{
    public class AliasProvider : IAliasProvider, IAliasRepository
    {
        private readonly IDirectory _directorySystem;
        private readonly IFile _fileSystem;
        private List<Alias> _aliases;

        public AliasProvider(IDirectory directorySystem, IFile fileSystem)
        {
            _directorySystem = directorySystem;
            _fileSystem = fileSystem;
            _aliases = InnerLoad(directorySystem, fileSystem);
        }

        private List<Alias> InnerLoad(IDirectory directorySystem, IFile fileSystem)
        {
            string aliasFilePath = GetAliasFilePath(directorySystem);
            if (fileSystem.Exists(aliasFilePath))
            {
                string json;
                using (var reader = fileSystem.OpenText(aliasFilePath))
                {
                    json = reader.ReadToEnd();
                }
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                return values.Select(x => new Alias { Key = x.Key, Value = x.Value }).ToList();
            }
            return new List<Alias>();
        }

        private string GetAliasFilePath(IDirectory directorySystem)
        {
            return Path.Combine(GetDirectory(directorySystem), ".lxaliases");
        }

        public string GetDirectory(IDirectory directorySystem)
        {
            var location = Assembly.GetEntryAssembly().Location;
            if (!string.IsNullOrEmpty(location))
            {
                return Path.GetDirectoryName(location);
            }
            return directorySystem.GetCurrentDirectory();
        }

        public List<Alias> GetAliases()
        {
            return _aliases;
        }

        public void Load()
        {
            _aliases = InnerLoad(_directorySystem, _fileSystem);
        }

        public void Save()
        {
            var dictionary = _aliases.ToDictionary(x => x.Key, x => x.Value);
            var json = JsonConvert.SerializeObject(dictionary);
            var aliasFilePath = GetAliasFilePath(_directorySystem);            
            using (var writer = _fileSystem.CreateText(aliasFilePath))
            {
                writer.Write(json);
            }
        }
    }

    public interface IAliasRepository
    {
        void Load();
        void Save();
    }

    public interface IAliasProvider
    {
        List<Alias> GetAliases();
    }

    public class Alias
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
