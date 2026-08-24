using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Internalexamportal.DocumentManager.Model;
using Microsoft.Extensions.Options;
using Internalexamportal.Core.Configurations;

namespace Internalexamportal.Core.FileSystem
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly FileConfiguration _fileConfiguraiton;
        public LocalFileSystem(
             IOptions<FileConfiguration> fileConfiguration)
        {
            _fileConfiguraiton = fileConfiguration.Value;
        }

        public void ClearDirectory(string directoryPath)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            foreach (FileInfo file in directory.GetFiles())
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                file.Delete();
            }
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        public void CreateDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath)) return;

            Directory.CreateDirectory(directoryPath);
        }

        public void CreateFile(string filePath, Stream stream, string mimeType = "")
        {
            string parentDirectory = filePath.GetParentDirectory();

            //Method call to create the parent directory if it doesnot exist. 
            CreateDirectory(parentDirectory);

            using (var fileStream = File.Create(filePath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }

        public void DeleteDirectory(string directoryPath)
        {
            Directory.Delete(directoryPath);
        }

        public void DeleteFileFromParentFolder(string filePath)
        {
            File.Delete(filePath);
        }

        public void DeleteFileWithParentFolder(string filePath)
        {
            string directoryPath = Path.GetDirectoryName(filePath);

            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            directory.Delete(true);
        }

        public bool DirectoryExists(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public bool FilesInDirectoryExist(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) return false;
            return Directory.EnumerateFiles(directoryPath).Any();
        }

        public List<ContentFile> GetAllFilesInDirectory(string directoryPath)
        {
            if (!DirectoryExists(directoryPath)) return new List<ContentFile>();

            List<ContentFile> responseFiles = Directory.GetFiles(directoryPath)
                .Select(f => new ContentFile
                {
                    Name = Path.GetFileName(f),
                    MimeType = f.GetMimeType(),
                    FileStream = GetFileStream(f)
                }
                ).ToList();

            return responseFiles;
        }

        public string GetDirectoryPath(params string[] directories)
        {
            var list = new List<string> { _fileConfiguraiton.TemplateBaseFolderPath };

            list.AddRange(directories);

            return Path.Combine(list.ToArray()).GetEscapedPath();
        }

        public byte[] GetFileByByteArray(string filePath)
        {
            MemoryStream fileStream = GetFileStream(filePath);

            return fileStream.ToArray();
        }

        public string GetFilePath(string extension, params string[] directories)
        {
            return $"{GetDirectoryPath(directories)}{extension}";
        }

        public MemoryStream GetFileStream(string filePath)
        {
            FileStream fileStream = File.OpenRead(filePath);
            MemoryStream resultStream = new MemoryStream();
            fileStream.CopyTo(resultStream);
            return resultStream;
        }
    }
}
