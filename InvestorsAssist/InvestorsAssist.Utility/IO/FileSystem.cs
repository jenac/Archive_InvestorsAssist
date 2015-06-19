using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Utility.IO
{
    public static class FileSystem
    {
        public const string _applicationName = "InvestorsAssist";
        public static string GetGoogleDriveFolder()
        {
            string value = Directory.GetParent(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                value = Directory.GetParent(value).FullName;
            }
            return Path.Combine(value, "Google Drive", _applicationName);
        }

        public static string GetOneDriveFolder()
        {
            string value = Directory.GetParent(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if (Environment.OSVersion.Version.Major >= 6)
            {
                value = Directory.GetParent(value).FullName;
            }
            return Path.Combine(value, "OneDrive", _applicationName);
        }

        public static void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public static string GetLogsFolder()
        {
            return Path.Combine(GetProfileFolder(), "Logs");
        }

        public static string GetTempFolder()
        {
            return Path.Combine(GetProfileFolder(), "Temp");
        }

        private static string GetProfileFolder()
        {
            string value = Environment.GetEnvironmentVariable("ALLUSERSPROFILE");
            return Path.Combine(value, _applicationName);
        }
        public static string ToUnixPath(string windowsPath)
        {
            return windowsPath.Replace('\\', '/');
        }
    }
}
