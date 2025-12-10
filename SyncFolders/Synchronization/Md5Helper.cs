using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SyncFolders.Synchronization
{
    public static class Md5Helper
    {
        public static Dictionary<string, (string fullFileName, string hash)> CalculateMd5HashesForFiles(List<string> filesList, string pathBase)
        {
            Dictionary<string, (string fullFileName, string hash)> filesWithMd5s = [];
            foreach (string file in filesList)
            {
                string fileMd5 = GetFileMd5(file);
                string relativeFileName = file.Replace(pathBase, string.Empty).TrimStart('\\');
                filesWithMd5s.Add(relativeFileName, (file, fileMd5));
            }

            return filesWithMd5s;
        }

        private static string GetFileMd5(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash);
        }
    }
}
