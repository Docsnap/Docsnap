namespace docsnap.utils
{
    public class CheckDirectory
    {
        public static void IfNotExistsCreateDirectory(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
    }
}