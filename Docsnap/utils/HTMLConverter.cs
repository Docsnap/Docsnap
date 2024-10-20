using System.Text.RegularExpressions;
using Docsnap.Models;
using Markdig;

namespace Docsnap.utils;

internal partial class HTMLConverter
{
    internal static List<DocumentationController> ConvertMDToHTML(string Path)
    {
        string[] files = Directory.GetFiles(Path, "*.md");
        Console.WriteLine("Quantidade de Arquivos Encontrados: " + files.Length);
        List<DocumentationController> APIContent = [];

        foreach (string file in files)
        {
            DocumentationController APIController = new();
            DocumentationEndpoint APIEndpoint = new();

            string[] fileLines = File.ReadAllLines(file);

            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains("# &"))
                {
                    string lineHTML = Markdown.ToHtml(fileLines[i][3..]);
                    APIController.ControllerName = lineHTML;

                    lineHTML = Markdown.ToHtml(fileLines[i + 1].Split(":")[1].Trim());
                    APIController.ControllerRoute = lineHTML;
                    i++;
                }
                else if (fileLines[i].Contains("## @@"))
                {
                    if (!string.IsNullOrEmpty(APIEndpoint.EndpointName))
                    {
                        APIController.EndpointsCollection.Add(APIEndpoint);
                        APIEndpoint = new();
                    }

                    string lineHTML = Markdown.ToHtml(fileLines[i][5..]);
                    APIEndpoint.EndpointName = lineHTML;

                    lineHTML = Markdown.ToHtml(fileLines[i + 1][5..]);
                    APIEndpoint.EndpointMethod = lineHTML;

                    lineHTML = Markdown.ToHtml(fileLines[i + 2].Trim());
                    APIEndpoint.EndpointRoute = lineHTML;
                    i += 2;
                }
                else
                {
                    string lineHTML = Markdown.ToHtml(fileLines[i]);
                    APIEndpoint.ContentEndpoint.Add(lineHTML);
                }
            }

            APIController.EndpointsCollection.Add(APIEndpoint);
            APIContent.Add(APIController);
        }

        return APIContent;
    }

    // private static bool ContainsTagHTML(string line, string tag)
    // {
    //     Regex tagPattern = new($@"<\s*{tag}\b[^>]*>(.*?)<\/\s*{tag}\s*>");
    //     return tagPattern.IsMatch(line);
    // }
}