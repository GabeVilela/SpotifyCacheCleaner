using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyCacheCleaner.Core
{
    public class Cleaner
    {
        private List<string> queue { get; set; }
        private int foldersAmmount { get; set; }

        public Cleaner(string[] CacheFolders)
        {
            queue = new List<string>(CacheFolders);
            foldersAmmount = CacheFolders.Length;
        }

        public string Next()
        {
            string folderPath = queue.First();
            System.IO.Directory.Delete(folderPath, true);
            queue.RemoveAt(0);
            foldersAmmount--;
            return folderPath.Split(@"\").Last();
        }

        public int GetRemaining()
        {
            return foldersAmmount;
        }
    }
}
