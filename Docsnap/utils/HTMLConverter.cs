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

            // Ajustar para verificar se está no nosso padrão de tags para salvar no lugar correto.
            // Verificar o conteúdo enquanto MD para que depois que for feita a conversão, possa ser salvo no lugar correto.
            foreach (string line in File.ReadAllLines(file))
            {
                string lineHTML = Markdown.ToHtml(line);

                if (ContainsTagHTML(lineHTML, "h2"))
                {
                    if (!string.IsNullOrEmpty(APIEndpoint.Endpoint))
                    {
                        APIController.MDJsonList.Add(APIEndpoint);
                        APIEndpoint = new();
                    }

                    APIEndpoint.Endpoint = lineHTML;
                }
                else
                {
                    APIEndpoint.ContentEndpoint.Add(lineHTML);
                }
            }

            APIController.MDJsonList.Add(APIEndpoint);
            APIContent.Add(APIController);
        }

        return APIContent;
    }

    private static bool ContainsTagHTML(string line, string tag)
    {
        Regex tagPattern = new($@"<\s*{tag}\b[^>]*>(.*?)<\/\s*{tag}\s*>");
        return tagPattern.IsMatch(line);
    }
}