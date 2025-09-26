namespace Paftax.Pafta.Revit2026.Utilities
{
    public static class FileUtilities
    {
        public static string MakeValidFileName(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        public static bool IsFileOpen(string filePath)
        {
            if (!File.Exists(filePath))
                return false;
            try
            {
                using FileStream stream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}
