using System.Reflection;
using System.Text;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace docsnap;

public static class Docsnap
{
    public static string docsPath = Directory.GetCurrentDirectory() + "/docs";

    private static Assembly assembly = Assembly.GetExecutingAssembly();

    public static async Task<string> GetDocsnapHtml(string htmlContent)
    {
        // Lista de recursos e seus placeholders correspondentes
        Dictionary<string, string> resources = new()
        {
            { "docsnap.docs.index.html", "HTML_CONTENT" },
            { "docsnap.docs.js.script.js", "JS_STYLES_CONTENT" }
        };

        // Leia o HTML principal
        Stream htmlStream = assembly.GetManifestResourceStream("docsnap.docs.index.html") ?? throw new Exception("HTML resource not found.");
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

    public static IApplicationBuilder UseDocsnap(this IApplicationBuilder app)
    {
        IfNotExistsCreateDirectory();
        ScanControllers();

        string html = CreateHtml();

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/docsnap"))
            {
                string htmlContent = await GetDocsnapHtml(html);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlContent);
            }
            else
            {
                await next();
            }
        });

        return app;
    }

    private static void IfNotExistsCreateDirectory()
    {
        if (!Directory.Exists(docsPath))
        {
            Console.WriteLine("Docsnap: Making docs directory");
            Directory.CreateDirectory(docsPath);
        }
    }

    public static string CreateHtml()
    {
        string[] files = Directory.GetFiles(docsPath, "*.md");
        string htmlFiles = string.Empty;

        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            htmlFiles += Markdown.ToHtml(content);
        }

        return htmlFiles;
    }

    private static void ScanControllers()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        var controllers = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(ControllerBase).IsAssignableFrom(t) && !t.IsAbstract) // Filtra os que herdam de ControllerBase (usado pelo ASP.NET Core)
            .ToList();

        System.Console.WriteLine("Docsnap: Making md files");

        foreach (var controller in controllers)
        {
            string fileController = $"{docsPath}/{controller.Name}.md";

            if (!File.Exists(fileController))
            {
                StringBuilder content = new();

                var methods = controller.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                                           .Where(m => m.IsPublic && m.DeclaringType == controller);

                foreach (var method in methods)
                {
                    content.AppendLine($"## {method.Name}");
                }

                File.WriteAllText(fileController, content.ToString());
            }
        }
    }
}