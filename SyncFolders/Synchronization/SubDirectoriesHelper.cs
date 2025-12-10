using System;
using System.Collections.Generic;
using System.Text;

namespace SyncFolders.Synchronization
{
    public static class SubDirectoriesHelper
    {
        public static Dictionary<string, string> CreateSubDirectoriesDictonary(List<string> subDirectoriesList, string mainFolderPath)
        {
            Dictionary<string, string> subDirectories = [];
            foreach (string subDirectory in subDirectoriesList)
            {
                string relativeDirectoryName = subDirectory.Replace(mainFolderPath, string.Empty).TrimStart('\\');
                subDirectories.Add(relativeDirectoryName, subDirectory);
            }

            return subDirectories;
        }
    }
}
