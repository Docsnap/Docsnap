using Docsnap.Markdown;


namespace Docsnap
{
    class Program
    {
        static void Main(string[] args)
        {
            Compiler.Compile("teste.md", "teste.html");
        }
    }
}