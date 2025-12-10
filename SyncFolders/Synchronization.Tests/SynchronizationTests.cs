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
            // Create temporary source folder with some files and subdirectories inside
            string basePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            m_sourceWithContent = Path.Combine(basePath, "SourceWithContent");
            Directory.CreateDirectory(m_sourceWithContent);
            
            File.WriteAllText(Path.Combine(m_sourceWithContent, "file1.txt"), "Some file content.");

            string subDir = Path.Combine(m_sourceWithContent, "subfolder");
            Directory.CreateDirectory(subDir);
            File.WriteAllText(Path.Combine(subDir, "file2.txt"), "Another file with some different content.");

            // Create a non-existing source folder path
            m_sourceNotExisting = Path.Combine(basePath, "SourceNotExisting");

            // Create an empty source folder
            m_sourceEmpty = Path.Combine(basePath, "SourceEmpty");
            Directory.CreateDirectory(m_sourceEmpty);

            // Create a path for replica with content
            m_replicaToBeFilledWithContent = Path.Combine(basePath, "ReplicaWithContent");

            // Create a replica path that should not exist
            m_replicaThatShouldNotExist = Path.Combine(basePath, "ReplicaThatShouldNotExist");
            Directory.CreateDirectory(m_replicaThatShouldNotExist);
            File.WriteAllText(Path.Combine(m_replicaThatShouldNotExist, "fileReplica1.txt"), "Some file content.");

            // Create a replica path that should be empty
            m_replicaThatShouldBeEmpty = Path.Combine(basePath, "ReplicaThatShouldBeEmpty");
            Directory.CreateDirectory(m_replicaThatShouldBeEmpty);
            File.WriteAllText(Path.Combine(m_replicaThatShouldBeEmpty, "fileReplica1.txt"), "Some file content.");

            // create a synchronization object
            m_synchronization = new Synchronization();
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up after tests
            if (Directory.Exists(m_sourceWithContent))
                Directory.Delete(m_sourceWithContent, true);

            if (Directory.Exists(m_sourceNotExisting))
                Directory.Delete(m_sourceNotExisting, true);

            if (Directory.Exists(m_sourceEmpty))
                Directory.Delete(m_sourceEmpty, true);

            if (Directory.Exists(m_replicaToBeFilledWithContent))
                Directory.Delete(m_replicaToBeFilledWithContent, true);
        }

        [Test]
        public void Test_Synchronization_BothWithContentButSourceWithMoreFiles()
        {
            // act
            m_synchronization.Synchronize(m_sourceWithContent, m_replicaToBeFilledWithContent);

            List<string> sourceFiles = Directory.GetFiles(m_sourceWithContent, "*", SearchOption.AllDirectories).ToList();
            List<string> replicaFiles = Directory.Exists(m_replicaToBeFilledWithContent) ? Directory.GetFiles(m_replicaToBeFilledWithContent, "*", SearchOption.AllDirectories).ToList() : [];

            // assert
            Assert.That(sourceFiles.Count, Is.EqualTo(replicaFiles.Count));

            Dictionary<string, string> sourceFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(sourceFiles);
            Dictionary<string, string> replicaFilesWithHashes = Md5Helper.CalculateMd5HashesForFiles(replicaFiles);

            foreach (var fileWithHash in sourceFilesWithHashes)
            {
                string replicaHashForFile = replicaFilesWithHashes[fileWithHash.Key];
                Assert.That(fileWithHash.Value, Is.EqualTo(replicaHashForFile));
            }
        }

        [Test]
        public void Test_Synchronization_BothWithContentButReplicaWithMoreFiles()
        {
        }

        [Test]
        public void Test_Synchronization_BothWithEqualContent()
        {
        }

        [Test]
        public void Test_Synchronization_BothWithContentButFilesWereUpdatedSoReplicaNeedToBeSynchronized()
        {
        }

        [Test]
        public void Test_Synchronization_SourceNotExisting()
        {
            // act
            m_synchronization.Synchronize(m_sourceNotExisting, m_replicaThatShouldNotExist);

            // assert
            Assert.That(Directory.Exists(m_sourceNotExisting), Is.EqualTo(false));
            Assert.That(Directory.Exists(m_replicaThatShouldNotExist), Is.EqualTo(false));
        }

        [Test]
        public void Test_Synchronization_SourceEmpty()
        {
            // act
            m_synchronization.Synchronize(m_sourceEmpty, m_replicaThatShouldBeEmpty);

            List<string> sourceFiles = Directory.GetFiles(m_sourceWithContent, "*", SearchOption.AllDirectories).ToList();
            List<string> replicaFiles = Directory.GetFiles(m_replicaToBeFilledWithContent, "*", SearchOption.AllDirectories).ToList();

            // assert
            Assert.That(sourceFiles.Count, Is.EqualTo(0));
            Assert.That(replicaFiles.Count, Is.EqualTo(0));
        }

        

        private string m_sourceWithContent;
        private string m_sourceNotExisting;
        private string m_sourceEmpty;

        private string m_replicaToBeFilledWithContent;
        private string m_replicaThatShouldNotExist;
        private string m_replicaThatShouldBeEmpty;
        private Synchronization m_synchronization;
    }
}
