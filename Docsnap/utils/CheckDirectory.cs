namespace Docsnap.utils;

internal class CheckDirectory
{
    internal static void IfNotExistsCreateDirectory(string Path)
    {
        if (!Directory.Exists(Path))
        {
            Directory.CreateDirectory(Path);
        }
    }
}
