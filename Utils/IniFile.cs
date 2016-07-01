using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DC_SB.Utils
{
    public static class IniFile
    {
        public static bool Portable { get; private set; }

        public const string DEFAULT_CONFIG_FILE_NAME = "config.ini";
        public const string DEFAULT_CONFIG_DIR_NAME = "DeathCounter";

        public static string DEFAULT_CONFIG_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DEFAULT_CONFIG_DIR_NAME);
        public static string OLD_CONFIG_DIR = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DEFAULT_CONFIG_DIR_NAME);

        public static string DEFAULT_CONFIG_PATH = Path.Combine(DEFAULT_CONFIG_DIR, DEFAULT_CONFIG_FILE_NAME);
        public static string OLD_CONFIG_PATH = Path.Combine(OLD_CONFIG_DIR, DEFAULT_CONFIG_FILE_NAME);

        private static string filePath;
        public static string FilePath {
            get { return filePath; }
            set
            {
                filePath = value;
                Create(value);
            }
        }

        static IniFile()
        {
            if (!Exists(DEFAULT_CONFIG_PATH) && Exists(OLD_CONFIG_PATH)) FilePath = OLD_CONFIG_PATH;
            else FilePath = DEFAULT_CONFIG_PATH;
        }

        public static bool Exists(string path)
        {
            return File.Exists(path);
        }

        public static void Create(string path)
        {
            string dirName = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }
        }

        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, FilePath);
        }

        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(5000);
            int i = GetPrivateProfileString(Section, Key, "", temp, 5000, FilePath);
            if (i == 0) return null;
            else return temp.ToString().Trim();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    }
}
