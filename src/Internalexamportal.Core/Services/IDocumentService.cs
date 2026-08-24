using Aspose.Words;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using Internalexamportal.Core.Models;

namespace Internalexamportal.Core.Services
{
    public interface IDocumentService
    {
        JObject ToJObject<T>(T model) where T : class;
        Stream GetTemplateStream(TemplateModel model, SaveFormat format, string filePath);
        string ToBase64String(Stream stream);
    }
}
