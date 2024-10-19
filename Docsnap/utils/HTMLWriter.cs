using System.Reflection;
using System.Text.Json;
using Docsnap.Models;

namespace Docsnap.utils;

internal class WriteHtml
{
    private readonly static JsonSerializerOptions serializerOptions = new() { WriteIndented = true };
    private readonly static Assembly assembly = Assembly.GetExecutingAssembly();

    internal static async Task<string> WriteToHtml(List<DocumentationAPI> APIContent)
    {
        Dictionary<string, string> resources = new()
        {
            { "Docsnap.web.index.html", "HTML_CONTENT" },
            { "Docsnap.web.js.script.js", "JS_STYLES_CONTENT" }
        };

        Stream htmlStream = assembly.GetManifestResourceStream("Docsnap.web.index.html") ?? throw new Exception("HTML resource not found." + assembly);
        StreamReader htmlReader = new(htmlStream);

        string fileAssembly = await htmlReader.ReadToEndAsync();

        foreach (KeyValuePair<string, string> resource in resources)
        {
            Stream stream = assembly.GetManifestResourceStream(resource.Key) ?? throw new Exception($"Resource '{resource.Key}' not found.");
            StreamReader reader = new(stream);

            fileAssembly = fileAssembly.Replace($"{{{{{resource.Value}}}}}", await reader.ReadToEndAsync());
        }

        string content = JsonSerializer.Serialize(APIContent, serializerOptions);
        fileAssembly = fileAssembly.Replace("{{MARKDOWN_CONTENT}}", content);

        return fileAssembly;
    }
}