using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Internalexamportal.Core.Extentions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAllBytes(this Stream instream)
        {
            if (instream is MemoryStream)
                return ((MemoryStream)instream).ToArray();

            var memoryStream = new MemoryStream();
            instream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
