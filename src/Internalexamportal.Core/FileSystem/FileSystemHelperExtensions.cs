using System.IO;

namespace Internalexamportal.Core.FileSystem
{
    public static class FileSystemHelperExtensions
    {
        public static string GetEscapedPath(this string path)
        {
            return path.Replace("\\", "/");
        }

        public static string GetParentDirectory(this string filePath)
        {
            return GetEscapedPath(Path.GetDirectoryName(filePath));
        }

        public static string GetMimeType(this string fileName)
        {
            string mimeType = "application/unknown";
            string ext = Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();

            return mimeType;
        }
    }
}
