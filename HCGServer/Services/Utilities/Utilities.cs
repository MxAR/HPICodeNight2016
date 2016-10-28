using System.IO;
using System.Threading.Tasks;

namespace HCGServer.Services.Utilities
{
    public static class Utilities
    {
        public static long GetDirectorySize(string DIRP)
        {
            string[] DIR = Directory.GetFiles(DIRP, "*.*");
            long size = 0;
            
            Parallel.ForEach(DIR, e => {
                FileInfo INFO = new FileInfo(e);
                size += INFO.Length;

            });

            return size;
        }
    }
}
