using System.IO;

namespace Internalexamportal.DocumentManager.Model
{
    public class ContentFile
    {
        public Stream FileStream { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
    }
}
