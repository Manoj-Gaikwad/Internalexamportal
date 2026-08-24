using System.Collections.Generic;
using System.IO;
using Internalexamportal.DocumentManager.Model;

namespace Internalexamportal.Core.Services
{
    public interface IDocumentStorage
    {
        void DeleteDocument(string filePath);
        void ClearDirectory(string filePath);
        string GetDirectoryPath(params string[] directories);
        string GetDocumentPath(string extension, params string[] parameters);
        MemoryStream GetDocumentStream(string filePath);
        void UploadDocument(string documentPath, Stream stream, string mimeType = "");
        byte[] GetDocumentByteArray(string filePath);
        bool FileExists(string filePath);
        bool DirectoryExists(string filePath);
        bool FilesInDirectoryExist(string directoryPath);
        List<ContentFile> GetAllFilesInDirectory(string directoryPath);
        void DeleteDocumentTemplate(string filePath);
    }
}
