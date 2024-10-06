using System.Reflection;

namespace docsnap.utils;

public class WriteHtml
{
    private readonly static Assembly assembly = Assembly.GetExecutingAssembly();

    public static async Task<string> WriteToHtml(string htmlContent)
    {
        // Lista de recursos e seus placeholders correspondentes
        Dictionary<string, string> resources = new()
        {
            { "Docsnap.web.index.html", "HTML_CONTENT" },
            { "Docsnap.web.js.script.js", "JS_STYLES_CONTENT" }
        };

        // Leia o HTML principal
        Stream htmlStream = assembly.GetManifestResourceStream("Docsnap.web.index.html") ?? throw new Exception("HTML resource not found." + assembly);
        StreamReader htmlReader = new(htmlStream);

        string fileAssembly = await htmlReader.ReadToEndAsync();

        // Substitua os placeholders pelos conteúdos dos recursos
        foreach (KeyValuePair<string, string> resource in resources)
        {
            string resourceName = resource.Key;
            string placeholder = $"{{{{{resource.Value}}}}}";


            Stream stream = assembly.GetManifestResourceStream(resourceName) ?? throw new Exception($"Resource '{resourceName}' not found.");
            StreamReader reader = new(stream);

            // Substitua o placeholder pelo conteúdo do recurso
            fileAssembly = fileAssembly.Replace(placeholder, await reader.ReadToEndAsync());
        }

        fileAssembly = fileAssembly.Replace("{{MARKDOWN_CONTENT}}", htmlContent);

        return fileAssembly;
    }
}