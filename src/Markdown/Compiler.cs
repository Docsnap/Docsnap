using System.Text;
using System.IO;


namespace Docsnap.Markdown
{
    public static class Compiler
    {
        public static string Compile(string inPath, string outPath)
        {
            string[] markdown = Reader.Read(inPath).Split(new[] { Environment.NewLine }, StringSplitOptions.None); ;

            StringBuilder html = new StringBuilder();

            foreach (string line in markdown)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    foreach (char character in line)
                    {
                        Console.WriteLine($"Character: {character}");
                    }
                }
                Console.WriteLine("new line");
            }

            return html.ToString();
        }
    }
}