using System.Collections.Generic;
using System.IO;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.DocumentManager.Model;

namespace Internalexamportal.Core.Services
{
    public class DocumentStorageService : IDocumentStorage
    {
        private readonly IFileSystem _fileSystem;

        public DocumentStorageService(
            IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        //Upload Document
        public void UploadDocument(
            string documentPath,
            Stream stream,
            string mimeType = "")
        {
            _fileSystem.CreateFile(documentPath, stream);
        }

        //Get Document
        public MemoryStream GetDocumentStream(string filePath)
        {
            return _fileSystem.GetFileStream(filePath);
        }


        public byte[] GetDocumentByteArray(string filePath)
        {
            return _fileSystem.GetFileByByteArray(filePath);
        }

        //Delete Document 
        public void DeleteDocument(string filePath)
        {
            _fileSystem.DeleteFileFromParentFolder(filePath);
        }

        //Get document file path
        public string GetDocumentPath(
            string extension = ".docx",
            params string[] parameters)
        {
            return _fileSystem.GetFilePath(extension, parameters);
        }

        //Get directory path
        public string GetDirectoryPath(params string[] directories)
        {
            return _fileSystem.GetDirectoryPath(directories);
        }

        //Check if file exists
        public bool FileExists(string filePath)
        {
            return _fileSystem.FileExists(filePath);
        }

        //Check if directory exists
        public bool DirectoryExists(string filePath)
        {
            return _fileSystem.DirectoryExists(filePath);
        }

        //check if directory exsists and it has files in it.
        public bool FilesInDirectoryExist(string directoryPath)
        {
            return _fileSystem.FilesInDirectoryExist(directoryPath);
        }

        //get all files from a directory
        public List<ContentFile> GetAllFilesInDirectory(string directoryPath)
        {
            return _fileSystem.GetAllFilesInDirectory(directoryPath);
        }

        public void ClearDirectory(string directoryPath)
        {
            _fileSystem.ClearDirectory(directoryPath);
        }

        public void DeleteDocumentTemplate(string filePath)
        {
            _fileSystem.DeleteFileFromParentFolder(filePath);
        }
    }
}
