using Docsnap.Markdown;


namespace Docsnap
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = Reader.Read("teste.md");
            Console.WriteLine(content);
        }
    }
}