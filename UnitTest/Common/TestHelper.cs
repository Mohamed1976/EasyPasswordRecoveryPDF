using System;
using System.IO;
using System.Reflection;

namespace UnitTest.Common
{
    public static class TestHelper
    {
        private const string SaveFolder = "DataFolder";

        public static string GetDataFolder()
        {
            string dataFolder = string.Empty;
            var path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            dataFolder = new Uri(path).LocalPath;
            dataFolder = System.IO.Path.Combine(Directory.GetParent(Directory.GetParent(dataFolder).FullName).FullName, SaveFolder);
            return dataFolder;
        }

        private static string dataFolder = string.Empty;
        public static string DataFolder
        {
            get
            {
                if (string.IsNullOrEmpty(dataFolder))
                {
                    dataFolder = GetDataFolder();
                }
                return dataFolder;
            }
        }
    }
}
