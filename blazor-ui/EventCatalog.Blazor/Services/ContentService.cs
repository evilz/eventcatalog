using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using EventCatalog.Blazor.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace EventCatalog.Blazor.Services
{
    public class ContentService
    {
        private readonly string _basePath;

        public ContentService()
        {
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "../..", "examples/default");
        }

        public async Task<List<Collection<T>>> GetCollection<T>(string collectionName)
        {
            var collectionPath = Path.Combine(_basePath, collectionName);
            if (!Directory.Exists(collectionPath))
            {
                return new List<Collection<T>>();
            }

            var files = Directory.GetFiles(collectionPath, "*.md", SearchOption.AllDirectories);
            var result = new List<Collection<T>>();

            foreach (var file in files)
            {
                var content = await File.ReadAllTextAsync(file);
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .Build();

                var reader = new StringReader(content);
                // Skip the first line
                reader.ReadLine();
                var frontMatter = deserializer.Deserialize<T>(reader);

                result.Add(new Collection<T>
                {
                    collection = collectionName,
                    data = frontMatter
                });
            }

            return result;
        }
    }
}
