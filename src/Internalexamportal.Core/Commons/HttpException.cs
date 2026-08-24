using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Commons
{
    public abstract class HttpException : Exception
    {
        public HttpException(string? message) : base(message)
        {
        }

        public abstract Task SetContext(HttpContext context, JsonSerializerSettings jsonSerializerSettings);
    }
}
