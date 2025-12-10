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

            // TODO: we will need to:
            // TOOD: 1. Move new files
            // TODO: 2. Update existing files in the MD5 is different
            // TODO: 3. Remove files that no longer exist in the 'source' folder

            if (!Directory.Exists(sourceFolderPath))
            {
                // if source directory does not exist we can just check if the replica directory exist
                // and if that would be the case then just remove it
                if (Directory.Exists(replicaFolderPath))
                    Directory.Delete(replicaFolderPath, true);

                return;
            }

            List<string> sourceFiles = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories).ToList();

            if (sourceFiles.Count == 0)
            {
                // if the count of source files is zero we need to also make sure
                // that replica folder is empty
                if (!Directory.Exists(replicaFolderPath))
                {
                    // if the replica folder is not just we can just create it and finish
                    Directory.CreateDirectory(replicaFolderPath);
                    return;
                }

                // if the replica folder is there we just need to take all files and directories from it and remove them
                Directory.GetFiles(replicaFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(File.Delete);
                Directory.GetDirectories(replicaFolderPath, "*", SearchOption.AllDirectories).ToList().ForEach(Directory.Delete);
                return;
            }

            // we have some files in the 'source' folder so we need to make sure that it's replicated in the 'replica'
            if (!Directory.Exists(replicaFolderPath))
            {
                // create the 'replica' folder if needed
                Directory.CreateDirectory(replicaFolderPath);
            }

            // we have at least 1 file in the 'source' folder and we'll need to perform the actual synchronization
            List<string> replicaFiles = Directory.GetFiles(replicaFolderPath, "*", SearchOption.AllDirectories).ToList();

            Dictionary<string, string> sourceFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(sourceFiles);
            Dictionary<string, string> replicaFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(replicaFiles);

            foreach (var sourceFileWithHash in sourceFilesWithHashes)
            {
                if (replicaFilesWithHashes.ContainsKey(sourceFileWithHash.Key))
                {
                    // replica contain a file with the same name
                    // we need to compare the hashes and decide if we want to copy it
                    // TODO:
                }
                else
                {
                    // replica doesn't conatain the file with that name
                    // we can just copy if over
                    // TODO:
                }
            }

            foreach (string replicaFile in replicaFiles)
            {
                if (!sourceFiles.Contains(replicaFile))
                {
                    // if there is no coresponding file in the 'source' folder we need to remove this file from 'replica'
                    File.Delete(replicaFile);
                }
            }

        }
    }
}
