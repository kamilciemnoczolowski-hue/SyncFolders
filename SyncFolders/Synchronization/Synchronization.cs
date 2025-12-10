using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
                SynchronizeWhenSourceFolderDoesntExist(replicaFolderPath);
                return;
            }

            List<string> sourceFiles = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories).ToList();
            List<string> sourceSubDirectoriesList = Directory.GetDirectories(sourceFolderPath, "*", SearchOption.AllDirectories).ToList();

            if (sourceFiles.Count == 0 && sourceSubDirectoriesList.Count == 0)
            {
                SynchronizeWhenNoFilesOrSubDirectoriesInSource(replicaFolderPath);
            }

            // we have some files in the 'source' folder so we need to make sure that it's replicated in the 'replica'
            if (!Directory.Exists(replicaFolderPath))
            {
                // create the 'replica' folder if needed
                Directory.CreateDirectory(replicaFolderPath);
            }

            // handle file synchronization
            List<string> replicaFiles = Directory.GetFiles(replicaFolderPath, "*", SearchOption.AllDirectories).ToList();

            Dictionary<string, (string FullFileName, string Hash)> sourceFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(sourceFiles, sourceFolderPath);
            Dictionary<string, (string FullFileName, string Hash)> replicaFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(replicaFiles, replicaFolderPath);

            CopyAndUpdateFilesInReplica(sourceFilesWithHashes, replicaFilesWithHashes, replicaFolderPath);
            RemoveFilesFromReplica(sourceFilesWithHashes, replicaFilesWithHashes);

            // handle subdirectories synchronization
            List<string> replicaSubDirectoriesList = Directory.GetDirectories(replicaFolderPath, "*", SearchOption.AllDirectories).ToList();

            Dictionary<string, string> sourceSubDirectories = SubDirectoriesHelper.CreateSubDirectoriesDictonary(sourceSubDirectoriesList, sourceFolderPath);
            Dictionary<string, string> replicaSubDirectories = SubDirectoriesHelper.CreateSubDirectoriesDictonary(replicaSubDirectoriesList, replicaFolderPath);

            CopyAndUpdateSubDirectoriesInReplica(sourceSubDirectories, replicaSubDirectories, replicaFolderPath);
            RemoveSubDirectories(sourceSubDirectories, replicaSubDirectories);
        }

        private void SynchronizeWhenSourceFolderDoesntExist(string replicaFolderPath)
        {
            // if source directory does not exist we can just check if the replica directory exist
            // and if that would be the case then just remove it
            if (Directory.Exists(replicaFolderPath))
                Directory.Delete(replicaFolderPath, true);
        }

        private void SynchronizeWhenNoFilesOrSubDirectoriesInSource(string replicaFolderPath)
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
            List<string> subDirectoriesToBeRemoved = Directory.GetDirectories(replicaFolderPath, "*", SearchOption.AllDirectories).ToList();
            foreach (string subDirectory in subDirectoriesToBeRemoved)
            {
                if (Directory.Exists(subDirectory))
                    Directory.Delete(subDirectory, true);
            }

            return;
        }

        private void CopyAndUpdateFilesInReplica(Dictionary<string, (string FullFileName, string Hash)> sourceFilesWithHashes, Dictionary<string, (string FullFileName, string Hash)> replicaFilesWithHashes, string replicaFolderPath)
        {
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
                    string replicatedFilePath = Path.Combine(replicaFolderPath, sourceFileWithHash.Key);
                    string? replicatedFileDirectoryPath = Path.GetDirectoryName(replicatedFilePath);

                    if (replicatedFileDirectoryPath is not null && !Directory.Exists(replicatedFileDirectoryPath))
                    {
                        Directory.CreateDirectory(replicatedFileDirectoryPath);
                    }

                    File.Copy(sourceFileWithHash.Value.FullFileName, replicatedFilePath);
                }
            }
        }

        private void RemoveFilesFromReplica(Dictionary<string, (string FullFileName, string Hash)> sourceFilesWithHashes, Dictionary<string, (string FullFileName, string Hash)> replicaFilesWithHashes)
        {
            foreach (KeyValuePair<string, (string FullFileName, string Hash)> replicaFileWithHash in replicaFilesWithHashes)
            {
                if (sourceFilesWithHashes.ContainsKey(replicaFileWithHash.Key))
                {
                    // this file is in the 'source' folder and was already updated if necessary - we can just continue
                    continue;
                }

                // the file is not present in the 'source' folder so we will remove it from 'replica'
                File.Delete(replicaFileWithHash.Value.FullFileName);
            }
        }

        private void CopyAndUpdateSubDirectoriesInReplica(Dictionary<string, string> sourceSubDirectories, Dictionary<string, string> replicaSubDirectories, string replicaFolderPath)
        {
            foreach (KeyValuePair<string, string> sourceSubDirectory in sourceSubDirectories)
            {
                if (replicaSubDirectories.ContainsKey(sourceSubDirectory.Key))
                {
                    // the subdirectory we're checking is present in the 'replica' folder
                    // no need to do anything
                    continue;
                }

                // subdirectory is not present in the 'replica' folder so we'll need to create it
                string newSubDirectory = Path.Combine(replicaFolderPath, sourceSubDirectory.Key);

                if (!Directory.Exists(newSubDirectory))
                    Directory.CreateDirectory(newSubDirectory);
            }
        }

        private void RemoveSubDirectories(Dictionary<string, string> sourceSubDirectories, Dictionary<string, string> replicaSubDirectories)
        {
            foreach (KeyValuePair<string, string> replicaSubDirectory in replicaSubDirectories)
            {
                if (sourceSubDirectories.ContainsKey(replicaSubDirectory.Key))
                {
                    // if checked subdirectory is present in the 'source' folder we don't need to do anything
                    continue;
                }

                // subdirectory is not present in the 'source' folder so we need to remove it from 'replica'
                if (Directory.Exists(replicaSubDirectory.Value))
                    Directory.Delete(replicaSubDirectory.Value, true);
            }
        }
    }
}
