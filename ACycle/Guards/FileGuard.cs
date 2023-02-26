namespace ACycle.Guards
{
    public static class FileGuard
    {
        public static void Exists(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(message: null, fileName: path);
            }
        }
    }
}
