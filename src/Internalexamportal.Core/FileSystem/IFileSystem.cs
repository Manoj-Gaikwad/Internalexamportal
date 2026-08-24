using System.Collections.Generic;
using System.IO;
using Internalexamportal.DocumentManager.Model;

namespace Internalexamportal.Core.FileSystem
{
    public interface IFileSystem
    {
        //For Directory
        void CreateDirectory(string directoryPath);
        void DeleteDirectory(string directoryPath);
        bool DirectoryExists(string directoryPath);
        string GetDirectoryPath(params string[] directories);
        bool FilesInDirectoryExist(string directoryPath);
        void ClearDirectory(string directoryPath);

        //For Files
        void CreateFile(string filePath, Stream stream, string mimeType = "");
        void DeleteFileWithParentFolder(string filePath);
        void DeleteFileFromParentFolder(string filePath);
        List<ContentFile> GetAllFilesInDirectory(string directoryPath);
        bool FileExists(string filePath);
        string GetFilePath(string extension, params string[] directories);
        MemoryStream GetFileStream(string filePath);
        byte[] GetFileByByteArray(string filePath);
    }
}
