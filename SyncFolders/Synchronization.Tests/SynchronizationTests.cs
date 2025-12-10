using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace SyncFolders.Synchronization.Tests
{
    [TestFixture]
    public class SynchronizationTests
    {
        [SetUp]
        public void SetUp()
        {
            #region Create 'source' with some files
            string sourceWithContent = Path.Combine(BasePath, SourceWithContent);
            Directory.CreateDirectory(sourceWithContent);
            
            File.WriteAllText(Path.Combine(sourceWithContent, "file1.txt"), "Some file content.");

            string subDir = Path.Combine(sourceWithContent, "subfolder");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "Another file with some different content.");
            #endregion

            #region Create 'source' with some updated files
            string sourceWithContentUpdated = Path.Combine(BasePath, SourceWithContentUpdated);
            Directory.CreateDirectory(sourceWithContentUpdated);

            File.WriteAllText(Path.Combine(sourceWithContentUpdated, "file1.txt"), "Some file content updated.");

            string subDir2 = Path.Combine(sourceWithContentUpdated, "subfolder");
            Directory.CreateDirectory(subDir2);
            File.WriteAllText(Path.Combine(subDir2, "file2.txt"), "Another file with some different content updated.");
            #endregion

            #region Create 'source' with more files
            string sourceWithMoreContent = Path.Combine(BasePath, SourceWithMoreContent);
            Directory.CreateDirectory(sourceWithMoreContent);

            File.WriteAllText(Path.Combine(sourceWithMoreContent, "file1.txt"), "Some file content.");

            string subDir3 = Path.Combine(sourceWithMoreContent, "subfolder");
            Directory.CreateDirectory(subDir3);
            File.WriteAllText(Path.Combine(subDir3, "file2.txt"), "Another file with some different content.");
            File.WriteAllText(Path.Combine(subDir3, "file3.txt"), "Yet another file with some different content.");
            File.WriteAllText(Path.Combine(subDir3, "file4.txt"), "Last file with some different content.");
            #endregion

            #region Create 'source' with more updated files
            string sourceWithMoreContentUpdated = Path.Combine(BasePath, SourceWithMoreContentUpdated);
            Directory.CreateDirectory(sourceWithMoreContentUpdated);

            File.WriteAllText(Path.Combine(sourceWithMoreContentUpdated, "file1.txt"), "Some file content updated.");

            string subDir4 = Path.Combine(sourceWithMoreContentUpdated, "subfolder");
            Directory.CreateDirectory(subDir4);
            File.WriteAllText(Path.Combine(subDir4, "file2.txt"), "Another file with some different content updated.");
            File.WriteAllText(Path.Combine(subDir4, "file3.txt"), "Yet another file with some different content updated.");
            File.WriteAllText(Path.Combine(subDir4, "file4.txt"), "Last file with some different content updated.");
            #endregion

            #region Create 'source' with some files but also empty directories
            string sourceWithContentAndEmptyDirectories = Path.Combine(BasePath, SourceWithContentAndEmptyDirectories);
            Directory.CreateDirectory(sourceWithContentAndEmptyDirectories);

            File.WriteAllText(Path.Combine(sourceWithContentAndEmptyDirectories, "file1.txt"), "Some file content.");

            string subDir5 = Path.Combine(sourceWithContentAndEmptyDirectories, "subfolder");
            Directory.CreateDirectory(subDir5);
            File.WriteAllText(Path.Combine(subDir5, "file2.txt"), "Another file with some different content.");

            string subDir6 = Path.Combine(sourceWithContentAndEmptyDirectories, "subfolder2");
            Directory.CreateDirectory(subDir6);

            string subDir7 = Path.Combine(subDir6, "subfolder3");
            Directory.CreateDirectory(subDir7);
            #endregion

            #region Create 'source' with only empty subdirectories
            string sourceWithOnlyEmptySubDirectories = Path.Combine(BasePath, SourceWithOnlyEmptySubDirectories);
            Directory.CreateDirectory(sourceWithOnlyEmptySubDirectories);

            string subDir8 = Path.Combine(sourceWithOnlyEmptySubDirectories, "subfolder");
            Directory.CreateDirectory(subDir8);

            string subDir9 = Path.Combine(sourceWithOnlyEmptySubDirectories, "subfolder2");
            Directory.CreateDirectory(subDir9);

            string subDir10 = Path.Combine(subDir9, "subfolder3");
            Directory.CreateDirectory(subDir10);
            #endregion

            #region Create an empty 'source' folder
            string sourceEmpty = Path.Combine(BasePath, SourceEmpty);
            Directory.CreateDirectory(sourceEmpty);
            #endregion

            #region Create 'replica' with some files
            string replicaWithContent = Path.Combine(BasePath, ReplicaWithContent);
            Directory.CreateDirectory(replicaWithContent);

            File.WriteAllText(Path.Combine(replicaWithContent, "file1.txt"), "Some file content.");

            string subDirReplica = Path.Combine(replicaWithContent, "subfolder");
            Directory.CreateDirectory(subDirReplica);
            File.WriteAllText(Path.Combine(subDirReplica, "file2.txt"), "Another file with some different content.");
            #endregion

            #region Create 'replica' with some more files
            string replicaWithMoreContent = Path.Combine(BasePath, ReplicaWithMoreContent);
            Directory.CreateDirectory(replicaWithMoreContent);

            File.WriteAllText(Path.Combine(replicaWithMoreContent, "file1.txt"), "Some file content.");

            string subDirReplica2 = Path.Combine(replicaWithMoreContent, "subfolder");
            Directory.CreateDirectory(subDirReplica2);
            File.WriteAllText(Path.Combine(subDirReplica2, "file2.txt"), "Another file with some different content.");
            File.WriteAllText(Path.Combine(subDirReplica2, "file3.txt"), "Yet another file with some different content.");
            File.WriteAllText(Path.Combine(subDirReplica2, "file4.txt"), "Last file with some different content.");
            #endregion

            #region Create 'replica' with some files but also empty directories
            string replicaWithContentAndEmptyDirectories = Path.Combine(BasePath, ReplicaWithContentAndEmptyDirectories);
            Directory.CreateDirectory(replicaWithContentAndEmptyDirectories);

            File.WriteAllText(Path.Combine(replicaWithContentAndEmptyDirectories, "file1.txt"), "Some file content.");

            string subDirReplica3 = Path.Combine(replicaWithContentAndEmptyDirectories, "subfolder");
            Directory.CreateDirectory(subDirReplica3);
            File.WriteAllText(Path.Combine(subDirReplica3, "file2.txt"), "Another file with some different content.");

            string subDirReplica4 = Path.Combine(subDirReplica3, "subfolder2");
            Directory.CreateDirectory(subDirReplica4);

            string subDirReplica5 = Path.Combine(subDirReplica4, "subfolder3");
            Directory.CreateDirectory(subDirReplica5);
            #endregion

            #region Create 'replica' with only empty subdirectories
            string replicaWithOnlyEmptySubdirectories = Path.Combine(BasePath, ReplicaWithOnlyEmptySubDirectories);
            Directory.CreateDirectory(replicaWithOnlyEmptySubdirectories);

            string subDirReplica6 = Path.Combine(replicaWithOnlyEmptySubdirectories, "subfolder");
            Directory.CreateDirectory(subDirReplica6);

            string subDirReplica7 = Path.Combine(subDirReplica6, "subfolder2");
            Directory.CreateDirectory(subDirReplica7);

            string subDirReplica8 = Path.Combine(subDirReplica7, "subfolder3");
            Directory.CreateDirectory(subDirReplica8);
            #endregion

            #region Create an empty 'replica' folder
            string replicaEmpty = Path.Combine(BasePath, ReplicaEmpty);
            Directory.CreateDirectory(replicaEmpty);
            #endregion

            // create a synchronization object
            m_synchronization = new Synchronization();
        }

        [TearDown]
        public void TearDown()
        {
            #region Clean up after tests
            string sourceWitContentPath = Path.Combine(BasePath, SourceWithContent);
            if (Directory.Exists(sourceWitContentPath))
                Directory.Delete(sourceWitContentPath, true);

            string sourceWitContentUpdatedPath = Path.Combine(BasePath, SourceWithContentUpdated);
            if (Directory.Exists(sourceWitContentUpdatedPath))
                Directory.Delete(sourceWitContentUpdatedPath, true);

            string sourceWitMoreContentPath = Path.Combine(BasePath, SourceWithMoreContent);
            if (Directory.Exists(sourceWitMoreContentPath))
                Directory.Delete(sourceWitMoreContentPath, true);

            string sourceWitMoreContentUpdatedPath = Path.Combine(BasePath, SourceWithMoreContentUpdated);
            if (Directory.Exists(sourceWitMoreContentUpdatedPath))
                Directory.Delete(sourceWitMoreContentUpdatedPath, true);

            string sourceNotExistingPath = Path.Combine(BasePath, SourceNotExisting);
            if (Directory.Exists(sourceNotExistingPath))
                Directory.Delete(sourceNotExistingPath, true);

            string sourceEmptyPath = Path.Combine(BasePath, SourceEmpty);
            if (Directory.Exists(sourceEmptyPath))
                Directory.Delete(sourceEmptyPath, true);

            string replicaWithContentPath = Path.Combine(BasePath, ReplicaWithContent);
            if (Directory.Exists(replicaWithContentPath))
                Directory.Delete(replicaWithContentPath, true);

            string replicaWithMoreContentPath = Path.Combine(BasePath, ReplicaWithMoreContent);
            if (Directory.Exists(replicaWithMoreContentPath))
                Directory.Delete(replicaWithMoreContentPath, true);

            string replicaNotExistingPath = Path.Combine(BasePath, ReplicaNotExisting);
            if (Directory.Exists(replicaNotExistingPath))
                Directory.Delete(replicaNotExistingPath, true);

            string replicaEmptyPath = Path.Combine(BasePath, ReplicaEmpty);
            if (Directory.Exists(replicaEmptyPath))
                Directory.Delete(replicaEmptyPath, true);
            #endregion
        }

        [TestCaseSource(nameof(TestCasesWhenSourceFolderExist))]
        public void Test_Synchronization_SourceWithContent(string sourceWithContent, string replica)
        {
            // setup
            string sourcePath = Path.Combine(BasePath, sourceWithContent);
            string replicaPath = Path.Combine(BasePath, replica);

            // act
            m_synchronization.Synchronize(sourcePath, replicaPath);

            List<string> sourceFiles = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).ToList();
            List<string> replicaFiles = Directory.Exists(replicaPath) ? Directory.GetFiles(replicaPath, "*", SearchOption.AllDirectories).ToList() : [];

            List<string> sourceSubDirectoriesList = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories).ToList();
            List<string> replicaSubDirectoriesList = Directory.Exists(replicaPath) ? Directory.GetDirectories(replicaPath, "*", SearchOption.AllDirectories).ToList() : [];

            Dictionary<string, (string FullFileName, string Hash)> sourceFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(sourceFiles, sourcePath);
            Dictionary<string, (string FullFileName, string Hash)> replicaFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(replicaFiles, replicaPath);

            Dictionary<string, string> sourceSubDirectories = SubDirectoriesHelper.CreateSubDirectoriesDictonary(sourceSubDirectoriesList, sourcePath);
            Dictionary<string, string> replicaSubDirectories = SubDirectoriesHelper.CreateSubDirectoriesDictonary(replicaSubDirectoriesList, replicaPath);

            // assert
            Assert.That(sourceFiles.Count, Is.EqualTo(replicaFiles.Count));
            Assert.That(sourceSubDirectories.Count, Is.EqualTo(replicaSubDirectories.Count));

            foreach (var fileWithHash in sourceFilesWithHashes)
            {
                (string FullFileName, string Hash) = replicaFilesWithHashes[fileWithHash.Key];
                Assert.That(fileWithHash.Value.Hash, Is.EqualTo(Hash));
            }

            foreach (KeyValuePair<string, string> sourceSubDirectory in sourceSubDirectories)
            {
                Assert.That(replicaSubDirectories.ContainsKey(sourceSubDirectory.Key), Is.True);
            }
        }

        [TestCaseSource(nameof(TestCasesWhenSourceDontExist))]
        public void Test_Synchronization_SourceNotExisting(string sourceNotExisting, string replica)
        {
            // setup
            string sourcePath = Path.Combine(BasePath, sourceNotExisting);
            string replicaPath = Path.Combine(BasePath, replica);

            // act
            m_synchronization.Synchronize(sourcePath, replicaPath);

            // assert
            Assert.That(Directory.Exists(sourcePath), Is.EqualTo(false));
            Assert.That(Directory.Exists(replicaPath), Is.EqualTo(false));
        }

        [TestCaseSource(nameof(TestCasesWhenSourceEmpty))]
        public void Test_Synchronization_SourceEmpty(string sourceEmpty, string replica)
        {
            // setup
            string sourcePath = Path.Combine(BasePath, sourceEmpty);
            string replicaPath = Path.Combine(BasePath, replica);

            // act
            m_synchronization.Synchronize(sourcePath, replicaPath);

            List<string> sourceFiles = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories).ToList();
            List<string> replicaFiles = Directory.GetFiles(replicaPath, "*", SearchOption.AllDirectories).ToList();

            // assert
            Assert.That(sourceFiles.Count, Is.EqualTo(0));
            Assert.That(replicaFiles.Count, Is.EqualTo(0));
        }

        private static string SourceWithContent = "SourceWithContent";
        private static string SourceWithContentUpdated = "SourceWithContentUpdated";
        private static string SourceWithMoreContent = "SourceWithMoreContent";
        private static string SourceWithMoreContentUpdated = "SourceWithMoreContentUpdated";
        private static string SourceWithContentAndEmptyDirectories = "SourceWithContentAndEmptyDirectories";
        private static string SourceWithOnlyEmptySubDirectories = "SourceWithOnlyEmptySubDirectories";
        private static string SourceNotExisting = "SourceNotExisting";
        private static string SourceEmpty = "SourceEmpty";

        private static string ReplicaWithContent = "ReplicaWithContent";
        private static string ReplicaWithMoreContent = "ReplicaWithMoreContent";
        private static string ReplicaWithContentAndEmptyDirectories = "ReplicaWithContentAndEmptyDirectories";
        private static string ReplicaWithOnlyEmptySubDirectories = "ReplicaWithOnlyEmptySubDirectories";
        private static string ReplicaNotExisting = "ReplicaNotExisting";
        private static string ReplicaEmpty = "ReplicaEmpty";

        public static readonly object[] TestCasesWhenSourceFolderExist =
        {
            new object[] { SourceWithContent, ReplicaWithContent },
            new object[] { SourceWithMoreContent, ReplicaWithContent },
            new object[] { SourceWithContent, ReplicaWithMoreContent },
            new object[] { SourceWithMoreContent, ReplicaWithMoreContent },
            new object[] { SourceWithContent, ReplicaNotExisting },
            new object[] { SourceWithContent, ReplicaEmpty },
            new object[] { SourceWithMoreContent, ReplicaNotExisting },
            new object[] { SourceWithMoreContent, ReplicaEmpty },
            new object[] { SourceWithContentUpdated, ReplicaWithContent },
            new object[] { SourceWithMoreContentUpdated, ReplicaWithMoreContent },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaWithContentAndEmptyDirectories },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaWithContent },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaWithMoreContent },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaNotExisting },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaEmpty },
            new object[] { SourceWithContentAndEmptyDirectories, ReplicaWithOnlyEmptySubDirectories },
            new object[] { SourceWithContent, ReplicaWithContentAndEmptyDirectories },
            new object[] { SourceWithMoreContent, ReplicaWithContentAndEmptyDirectories },
            new object[] { SourceWithContent, ReplicaWithOnlyEmptySubDirectories },
            new object[] { SourceWithMoreContent, ReplicaWithOnlyEmptySubDirectories },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaWithOnlyEmptySubDirectories },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaWithContentAndEmptyDirectories },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaWithContent },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaWithMoreContent },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaNotExisting },
            new object[] { SourceWithOnlyEmptySubDirectories, ReplicaEmpty },
        };

        public static readonly object[] TestCasesWhenSourceDontExist =
        {
            new object[] { SourceNotExisting, ReplicaWithContent },
            new object[] { SourceNotExisting, ReplicaWithMoreContent },
            new object[] { SourceNotExisting, ReplicaNotExisting },
            new object[] { SourceNotExisting, ReplicaEmpty },
            new object[] { SourceNotExisting, ReplicaWithContentAndEmptyDirectories },
        };

        public static readonly object[] TestCasesWhenSourceEmpty =
        {
            new object[] { SourceEmpty, ReplicaWithContent },
            new object[] { SourceEmpty, ReplicaWithMoreContent },
            new object[] { SourceEmpty, ReplicaNotExisting },
            new object[] { SourceEmpty, ReplicaEmpty },
            new object[] { SourceEmpty, ReplicaWithContentAndEmptyDirectories },
        };

        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        private Synchronization m_synchronization;
    }
}
