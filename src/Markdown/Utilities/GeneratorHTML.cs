namespace Docsnap.Markdown.Utilities
{
    public static class GeneratorHTML
    {
        public static void Generate(string path, string content)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(content);
            }
        }
    }
}