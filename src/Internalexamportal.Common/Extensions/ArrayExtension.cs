using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Internalexamportal.Common.Extensions
{
    public static class ArrayExtensions
    {
        public static void Split<T>(this T[] array, string separator, out T[] first, out T[] last)
        {
            var index = Array.IndexOf(array, separator);
            first = array.Take(index).ToArray();
            last = array.Skip(index + 1).ToArray();
        }
    }
}
