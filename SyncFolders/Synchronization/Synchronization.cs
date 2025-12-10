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

            Dictionary<string, (string FullFileName, string Hash)> sourceFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(sourceFiles, sourceFolderPath);
            Dictionary<string, (string FullFileName, string Hash)> replicaFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(replicaFiles, replicaFolderPath);

            foreach (var sourceFileWithHash in sourceFilesWithHashes)
            {
                if (replicaFilesWithHashes.ContainsKey(sourceFileWithHash.Key))
                {
                    // the file we're checking is present in the 'replica' folder
                    // we need to check if the content is the same (we'll do it using the MD5 checksum)
                    (string FullFileName, string Hash) = replicaFilesWithHashes[sourceFileWithHash.Key];
                    if (sourceFileWithHash.Value.Hash.Equals(Hash))
                    {
                        // file have the same checksum so we can just skip it
                        continue;
                    }

                    // check some is different so we want to override the version in 'replica' folder with the one from 'source'
                    File.Copy(sourceFileWithHash.Value.FullFileName, Path.Combine(replicaFolderPath, sourceFileWithHash.Key), true);
                }
                else
                {
                    // the file we're checking is not present in the 'replica' folder
                    // so we can just copy it without further checking
                    File.Copy(sourceFileWithHash.Value.FullFileName, Path.Combine(replicaFolderPath, sourceFileWithHash.Key));
                }
            }

            foreach (string replicaFile in replicaFiles)
            {
                // TODO: verify this - I think this will not work and we'll need to do the checking
                // TODO: on the 'sourceFilesWithHashes' and 'replicaFilesWithHashes'
                if (!sourceFiles.Contains(replicaFile))
                {
                    // if there is no coresponding file in the 'source' folder we need to remove this file from 'replica'
                    File.Delete(replicaFile);
                }
            }
        }
    }
}
