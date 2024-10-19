using System.Text.RegularExpressions;
using Docsnap.Models;
using Markdig;

namespace Docsnap.utils;

internal partial class HTMLConverter
{
    internal static List<DocumentationAPI> ConvertMDToHTML(string Path)
    {
        string[] files = Directory.GetFiles(Path, "*.md");
        Console.WriteLine("Quantidade de Arquivos Encontrados: " + files.Length);
        List<DocumentationAPI> APIContent = [];

        foreach (string file in files)
        {
            DocumentationAPI APIController = new();
            DocumentationEndpoint Endpoint = new();

            // Ajustar para verificar se está no nosso padrão de tags para salvar no lugar correto.
            // Verificar o conteúdo enquanto MD para que depois que for feita a conversão, possa ser salvo no lugar correto.
            foreach (string line in File.ReadAllLines(file))
            {
                string lineHTML = Markdown.ToHtml(line);

                if (ContainsTagHTML(lineHTML, "h2"))
                {
                    if (!string.IsNullOrEmpty(Endpoint.Endpoint))
                    {
                        APIController.MDJsonList.Add(Endpoint);
                        Endpoint = new();
                    }

                    Endpoint.Endpoint = lineHTML;
                }
                else
                {
                    Endpoint.ContentEndpoint.Add(lineHTML);
                }
            }

            APIController.MDJsonList.Add(Endpoint);
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