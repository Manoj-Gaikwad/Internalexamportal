using System;

namespace Internalexamportal.Core.Features.ApplicationError
{
    class ApplicationErrorModel
    {
        public int Id { get; set; }
        public string ExceptionType { get; set; }
        public string Message { get; set; }
        public DateTime ExceptionDateUTC { get; set; }
    }
}
