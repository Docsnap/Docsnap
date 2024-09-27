using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace docsnap;

public class Docsnap
{
    public static string docsPath = Directory.GetCurrentDirectory() + "/docs";

    private static Assembly assembly = Assembly.GetExecutingAssembly();

    public static async Task<string> GetDocsnapHtml()
    {
        // Lista de recursos e seus placeholders correspondentes
        Dictionary<string, string> resources = new()
        {
            { "docsnapLib.docs.index.html", "HTML_CONTENT" },
            { "docsnapLib.docs.js.script.js", "JS_STYLES_CONTENT" }
        };

        // Leia o HTML principal
        Stream htmlStream = assembly.GetManifestResourceStream("docsnapLib.docs.index.html") ?? throw new Exception("HTML resource not found.");
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

        return fileAssembly;
    }

    public static void ConfigureDocsnap(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/docsnap", async context =>
            {
                string htmlContent = await GetDocsnapHtml();
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlContent);
            });
        });

        ScanControllers();
    }

    private static void IfNotExistsCreateDirectory()
    {
        if (!Directory.Exists(docsPath))
        {
            System.Console.WriteLine("Docsnap: Making docs directory");
            Directory.CreateDirectory(docsPath);
        }
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
            IfNotExistsCreateDirectory();

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