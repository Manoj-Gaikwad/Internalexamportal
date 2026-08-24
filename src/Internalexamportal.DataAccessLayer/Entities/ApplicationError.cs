using System;

namespace Internalexamportal.DataAccessLayer.Entities
{
    public class ApplicationError
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; }
        public string User { get; set; }
    }
}
