using Markdig;

namespace docsnap.utils;

public class ConvertToHtml
{
    public static string CreateHtml(string Path)
    {
        string[] files = Directory.GetFiles(Path, "*.md");
        string htmlFiles = string.Empty;

        foreach (string file in files)
        {
            string content = File.ReadAllText(file);
            htmlFiles += Markdown.ToHtml(content);
        }

        return htmlFiles;
    }
}