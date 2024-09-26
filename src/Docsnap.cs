using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Docsnap;

public static class Docsnap
{
    public static string route = "/docsnap";

    public static void UseDocsnap(IApplicationBuilder app)
    {
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet(route, async context =>
            {
                string htmlContent = await GetDocsnapHtml();
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlContent);
            });
        });
    }

    public static async Task<string> GetDocsnapHtml()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

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
}
