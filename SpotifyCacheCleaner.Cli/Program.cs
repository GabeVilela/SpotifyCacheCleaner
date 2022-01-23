using SpotifyCacheCleaner.Core;
using System.Configuration;

namespace SpotifyCacheCleaner.Cli
{
    public class Program
    {
        private static bool flagQuietMode = false;

        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    switch (arg)
                    {
                        case "--quiet": flagQuietMode = true; break;
                        default: break;
                    }
                }
            }

            Console.WriteLine("INFO: Starting...");
            if (!SettingsHandler.CanStart())
            {
                if (flagQuietMode) Finish();
                else FirstSetup();
            }

            string[] cacheFolders = DirectoryNavigator.GetCacheFolders(
                SettingsHandler.Read(SettingsHandler.Ekeys.InstallPath) ?? "",
                SettingsHandler.Read(SettingsHandler.Ekeys.Version) == "Win32" ? false : true
                );

            if (cacheFolders.Length == 0)
            {
                Console.WriteLine("ERROR: The cache folder is already empty, or it doesn't exists!");
                Finish();
            }
            Cleaner engine = new Cleaner(cacheFolders);

            Console.WriteLine("WARN: Please, close Spotify if it's running...");

            if (!flagQuietMode)
            {
                string confirmation = GetInput($"{engine.GetRemaining()} cache folders found!\n" +
                    $"Are you sure you want to delete them?\n(Y/N)", "[yYnN]");

                if (confirmation.ToUpper() == "N")
                {
                    Console.WriteLine("Process aborted!");
                    Finish();
                }
            }


            while (engine.GetRemaining() > 0)
            {
                try
                {
                    string folderDeleted = engine.Next();
                    Console.WriteLine($"LOG: Folder {folderDeleted} has been sucefully deleted!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR: Something went wrong...");
                    Console.WriteLine(ex.ToString());
                    Finish();
                }
            }

            Console.WriteLine("\n\nINFO: Process finished!");
            Finish();
        }

        public static void FirstSetup()
        {
            Console.WriteLine("FIRST TIME SETUP\n" +
                "In order for this app to work properly, you need to setup a few things.");

            string path = GetInput("Where's your Spotify installation?\n" +
                @"Usually, it will be on C:\Users\<Your UserName>\AppData\Local\Spotify" + "\n" +
                @"or C:\Users\<Your UserName>\AppData\Local\Packages\SpotifyAB.SpotifyMusic_zpdnekdrzrea0" +
                "\nDon't forget to include the name of Spotify's root folder", @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$");

            string isStoreVersion = GetInput("Ok! Is your Spotify the Microsoft Store version?\n(Y/N)", "[yYnN]");

            Console.WriteLine("Got it! That's was all.");
            SettingsHandler.AddUpdate("InstallPath", path.EndsWith(@"\") ? path : path + @"\");
            SettingsHandler.AddUpdate("Version", isStoreVersion.ToUpper() == "Y" ? "UWP" : "Win32");

            Console.WriteLine("INFO: Your settings were sucessfully saved!");
        }

        public static string GetInput(string message, string regexMask)
        {
            RegexStringValidator validator = new RegexStringValidator(regexMask);
            bool clearConsole = false;

            string result;
            while (true)
            {
                if (clearConsole)
                    Console.Clear();
                else
                    Console.WriteLine("\n\n");
                Console.WriteLine(message);
                string input = Console.ReadLine() ?? "";
                try
                {
                    validator.Validate(input);

                    result = input;
                    break;
                }
                catch (Exception ex)
                {
                    clearConsole = true;
                }
            }

            return result;
        }

        public static void Finish()
        {
            if (!flagQuietMode)
            {
                Console.WriteLine("\nPress any key to close this window.");
                Console.ReadKey();
            }
            Environment.Exit(0);
        }
    }
}