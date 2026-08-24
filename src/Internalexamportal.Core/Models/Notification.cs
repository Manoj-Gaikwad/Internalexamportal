using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Internalexamportal.Core.Models
{

    public class Notification
    {
		public int Id { get; set; }
        public string DispatchType { get; set; }
        public List<string> To { get; set; }
        public string From { get; set; }
        public List<AttachmentFile> Attachment { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }

    public class AttachmentFile
    {
        [JsonConverter(typeof(MemoryStreamJsonConverter))]
        public Stream File { get; set; }
        public string Name { get; set; }
    }
}
