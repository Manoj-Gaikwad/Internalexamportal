using System;

namespace Internalexamportal.Common.Extensions
{
    public static class StringExtension
    {
        public static string ToCamelCase(this string input)
        {
            if (!string.IsNullOrEmpty(input) && input.Length > 1)
            {
                return Char.ToLowerInvariant(input[0]) + input.Substring(1).Replace(" ", "");
            }
            return input;
        }

    }
}
