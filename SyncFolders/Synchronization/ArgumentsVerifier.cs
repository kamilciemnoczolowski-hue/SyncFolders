using System;
using System.Collections.Generic;
using System.Text;

namespace SyncFolders.Synchronization
{
    public class ArgumentsVerifier
    {
        public ArgumentsVerifier(string[] args)
        {
            m_args = args;
        }

        public bool AreArgumentsValid(out string pathToSource, out string pathToReplica, out int interval, out string pathToLogFile)
        {
            pathToSource = pathToReplica = pathToLogFile = string.Empty;
            interval = 0;

            if (m_args.Length != 4)
            {
                WrongArgumentsMessage();
                return false;
            }

            // Check if paths to 'source' and 'replica' looks like actual directory paths
            if (!IsValidPath(m_args[0]))
            {
                WrongArgumentsMessage();
                return false;
            }

            pathToSource = m_args[0];

            if (!IsValidPath(m_args[1]))
            {
                WrongArgumentsMessage();
                return false;
            }

            pathToReplica = m_args[1];

            // Check if interval is a valid integer
            if (!int.TryParse(m_args[2], out int intervalIsSeconds))
            {
                WrongArgumentsMessage();
                return false;
            }

            interval = intervalIsSeconds * 1000;

            // Check if the path to log file looks like a proper file path
            if (!IsValidPath(m_args[3]))
            {
                WrongArgumentsMessage();
                return false;
            }

            pathToLogFile = m_args[3];
            return true;
        }

        private bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            // Check invalid path characters
            if (path.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                return false;

            return true;
        }

        private void WrongArgumentsMessage()
        {
            Console.WriteLine("Program is expecting 4 arguments provided in the command line. They need to be in a proper format. Without that the program will not run.");
            Console.WriteLine("1st argument - PATH to the 'source' folder.");
            Console.WriteLine("2nd argument - PATH to the 'replica' folder.");
            Console.WriteLine("3rd argument - INTEGER that would represent number of seconds between synchronizations.");
            Console.WriteLine("4th argument - PATH to the log file.");
            Console.WriteLine("Please run the file again with those 4 arguments in correct formats.");
        }

        private readonly string[] m_args;
    }
}
