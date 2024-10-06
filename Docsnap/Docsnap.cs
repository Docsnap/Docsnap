using System.Diagnostics;
using docsnap.utils;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace docsnap;

public static class Docsnap
{
    private static Stopwatch Timer = new();
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
        Watcher.ScanAllControllers(docsPath);

        string html = HTMLConverter.CreateHtml(docsPath);

        app.Use(async (context, next) =>
        {
            if (context.Request.Path.StartsWithSegments("/docsnap"))
            {
                string htmlContent = await WriteHtml.WriteToHtml(html);
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(htmlContent);
            }

            await next();
        });

        Timer.Stop();
        Console.WriteLine("Process time: " + Timer.ElapsedMilliseconds);
        return app;
    }

    //! todo: Change this name because can confuse the users (Path to access in API or path to put the documents)
    public static IApplicationBuilder ChangeDocsnapPath(this IApplicationBuilder app, string path)
    {
        docsPath = Directory.GetCurrentDirectory() + path;
        return app;
    }
}