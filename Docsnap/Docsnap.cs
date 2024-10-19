using System.Diagnostics;
using Docsnap.utils;
using Markdig;
using Docsnap.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Docsnap;

public static class Docsnap
{
    private static readonly Stopwatch Timer = new();
    private static string docsPath = Directory.GetCurrentDirectory() + "/docs";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDocsnap(this IApplicationBuilder app)
    {
        Timer.Start();

        CheckDirectory.IfNotExistsCreateDirectory(docsPath);
        MethodsAndController.ScanAllControllers(docsPath);

        List<DocumentationController> APIHtml = HTMLConverter.ConvertMDToHTML(docsPath);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/docsnap"))
            {
                string htmlContent = await WriteHtml.WriteToHtml(APIHtml);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlContent);
            }

            await next();
        });

        Timer.Stop();
        Console.WriteLine("Process Docsnap time: " + Timer.ElapsedMilliseconds);
        return app;
    }

    public static IApplicationBuilder ChangeDocsnapPath(this IApplicationBuilder app, string path)
    {
        docsPath = Directory.GetCurrentDirectory() + path;
        return app;
    }
}