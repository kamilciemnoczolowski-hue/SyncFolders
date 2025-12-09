using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SyncFolders.Synchronization
{
    public static class Md5Helper
    {
        public static string GetFileMd5(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            byte[] hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash);
        }
    }
}
