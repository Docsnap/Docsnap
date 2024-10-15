using System.Text.RegularExpressions;
using Docsnap.Models;
using Markdig;

namespace Docsnap.utils;

public partial class HTMLConverter
{
    public static List<ListMDJson> ConvertMDToHTML(string Path)
    {
        string[] files = Directory.GetFiles(Path, "*.md");
        Console.WriteLine("Quantidade de Arquivos Encontrados: " + files.Length);
        List<ListMDJson> MDObject = [];

        foreach (string file in files)
        {
            ListMDJson ListMd = new();
            MDJson md = new();

            foreach (string line in File.ReadAllLines(file))
            {
                string lineMd = Markdown.ToHtml(line);

                if (ContainsTagHTML(lineMd, "h2"))
                {
                    if (!string.IsNullOrEmpty(md.TitleMD))
                    {
                        ListMd.MDJsonList.Add(md);
                        md = new();
                    }

                    md.TitleMD = lineMd;
                }
                else
                {
                    md.BodyMD.Add(lineMd);
                }
            }

            ListMd.MDJsonList.Add(md);
            MDObject.Add(ListMd);
        }

        return MDObject;
    }

    private static bool ContainsTagHTML(string line, string tag)
    {
        Regex tagPattern = new($@"<\s*{tag}\b[^>]*>(.*?)<\/\s*{tag}\s*>");
        return tagPattern.IsMatch(line);
    }
}