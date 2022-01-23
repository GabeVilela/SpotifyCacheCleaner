namespace SpotifyCacheCleaner.Core
{
    public class DirectoryNavigator
    {
        public static string[] GetCacheFolders(string installPath, bool isUWP)
        {
            string[] foldersToDelete = Array.Empty<string>();

            string path;

            if (isUWP)
                path = Path.Combine(installPath, @"LocalCache\Spotify\Data");
            else
                path = Path.Combine(installPath, "Storage");

            if (Directory.Exists(path))
            {
                foldersToDelete = Directory.GetDirectories(path);
            }
            return foldersToDelete;
        }
    }
}