using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SyncFolders.Synchronization
{
    public class Synchronization
    {
        public void Synchronize(string sourceFolderPath, string replicaFolderPath)
        {
            // TODO: check each file and copy from source to replica if the file is nor there
            // TODO: or there is a new version available
        }
    }
}
