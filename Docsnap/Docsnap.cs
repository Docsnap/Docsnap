using docsnap.utils;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace docsnap;

public static class Docsnap
{
    private static string docsPath = Directory.GetCurrentDirectory() + "/docs";

    public static IApplicationBuilder UseDocsnap(this IApplicationBuilder app)
    {
        CheckDirectory.IfNotExistsCreateDirectory(docsPath);
        ScanControllers.ScanAllControllers(docsPath);

        string html = ConvertToHtml.CreateHtml(docsPath);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/docsnap"))
            {
                string htmlContent = await WriteHtml.WriteToHtml(html);
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

    public static IApplicationBuilder AlterDocsnapPath(this IApplicationBuilder app, string path)
    {
        docsPath = Directory.GetCurrentDirectory() + path;
        return app;
    }
}