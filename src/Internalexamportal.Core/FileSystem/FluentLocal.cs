using Aspose.Words;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Internalexamportal.DocumentManager.Core;

namespace Internalexamportal.Core.FileSystem
{
    public class FluentLocal : IFluentLocal
    {
        private MemoryStream _templateMemoryStream;
        private MemoryStream _documentMemoryStream;
        private IFileSystem _fileSystem;

        public FluentLocal(IFileSystem fileSystem)
        {
            _templateMemoryStream = new MemoryStream();
            _documentMemoryStream = new MemoryStream();
            _fileSystem = fileSystem;
        }

        public FluentLocal Upload(Stream stream, string path)
        {
            _fileSystem.CreateFile(path, stream);
            return this;
        }

        public FluentLocal UseTemplate(string path)
        {
            _templateMemoryStream = _fileSystem.GetFileStream(path);
            return this;
        }

        public FluentLocal Fill(JObject data, SaveFormat saveFormat)
        {
            _documentMemoryStream = 
                FluentAspose.TakeDocument(_templateMemoryStream)
                   .InjectJObject(data)
                   .Render<FieldMergingCallBack>()
                   .CleanUp()
                   .SaveToStream(saveFormat);

            return this;
        }

        public void Save(string path) => _fileSystem.CreateFile(path, _documentMemoryStream);

        public Stream Get() => _documentMemoryStream;

        public Stream Get(string path) => _fileSystem.GetFileStream(path);

    }

    public interface IFluentLocal
    { 
        FluentLocal UseTemplate(string path);

        FluentLocal Fill(JObject data, SaveFormat saveFormat);

        Stream Get();

        Stream Get(string path);

        void Save(string path);
    }
}
