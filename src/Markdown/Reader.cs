using System.Text;

namespace Docsnap.Markdown
{
    public static class Reader
    {
        public static string Read(string path)
        {
            if (Path.GetExtension(path) is not ".md")
            {
                throw new ArgumentException("The file must be a markdown file");
            }

            StringBuilder sb = new StringBuilder();

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) is not null)
                {
                    sb.AppendLine(line);
                }
            }

            return sb.ToString();
        }
    }
}