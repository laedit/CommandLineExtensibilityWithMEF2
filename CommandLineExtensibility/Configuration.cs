using System;

namespace CommandLineExtensibility
{
    public class Configuration
    {
        private readonly string currentDirectory;
        private readonly int random = new Random().Next();

        private Configuration(string currentDirectory)
        {
            this.currentDirectory = currentDirectory;
        }

        internal static Configuration ReadFrom(string currentDirectory)
        {
            return new Configuration(currentDirectory);
        }
    }
}