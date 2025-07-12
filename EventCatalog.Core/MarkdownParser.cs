using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using System;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace EventCatalog.Core
{
    public class MarkdownParser
    {
        public static (T? frontMatter, string content) Parse<T>(string markdown)
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseYamlFrontMatter()
                .Build();

            var document = Markdown.Parse(markdown, pipeline);
            var yamlBlock = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            if (yamlBlock == null)
            {
                return (default(T), Markdown.ToHtml(markdown, pipeline));
            }

            var yaml = yamlBlock.Lines.ToString();
            var deserializer = new DeserializerBuilder().Build();
            var frontMatter = deserializer.Deserialize<T>(yaml);

            var content = Markdown.ToHtml(document, pipeline);

            return (frontMatter, content);
        }
    }
}
