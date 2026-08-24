using System.Collections.Generic;

namespace Internalexamportal.Common.Model
{
    public class NotificationModel
    {
        public string DispatchType { get; set; }
        public List<string> To { get; set; }
        public string From { get; set; }
        public List<AttachmentFile> Attachment { get; set; }
        public string Body { get; set; }                                                           
        public string Subject { get; set; }
    }

    public class AttachmentFile
    {
        public FileType FileType { get; set; }
        public string Path { get; set; }
    }

    public enum FileType
    {
        Web,
        Local
    }
}
