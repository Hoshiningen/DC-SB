using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
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

        const uint FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
        const uint FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
        const uint FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
        const uint FORMAT_MESSAGE_ARGUMENT_ARRAY = 0x00002000;
        const uint FORMAT_MESSAGE_FROM_HMODULE = 0x00000800;
        const uint FORMAT_MESSAGE_FROM_STRING = 0x00000400;

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
                FileSecurity security = new FileSecurity();
                security.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier("S-1-1-0"), FileSystemRights.FullControl, InheritanceFlags.None, PropagationFlags.None, AccessControlType.Allow));
                File.Create(path, 1, FileOptions.None, security).Close();
            }
        }

        public static void IniWriteValue(string Section, string Key, string Value)
        {
            long result = WritePrivateProfileStringA(Section, Key, Value, FilePath);
            if (result == 0)
            {
                string errorMessage = new Win32Exception(Marshal.GetLastWin32Error()).Message;
                Console.WriteLine(errorMessage);
                ErrorHandler.Raise("Following error occured when saving data to config file:\n{0}\nData:\n[{1}]{2}={3}\n", 
                    errorMessage, Section, Key, Value);
            }
        }

        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(5000);
            int i = GetPrivateProfileStringA(Section, Key, "", temp, 5000, FilePath);
            if (i == 0) return null;
            else return temp.ToString().Trim();
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileStringA(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileStringA(string section, string key, string def, StringBuilder retVal, int size, string filePath);
    }
}
