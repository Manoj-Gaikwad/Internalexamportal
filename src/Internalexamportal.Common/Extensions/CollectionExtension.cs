using System;
using System.Collections.Generic;
using System.Linq;

namespace Internalexamportal.Common.Extensions
{
    public static class CollectionExtension
    {
        // Reuse a single Random instance (thread-safe in .NET 6)
        private static readonly Random _random = new Random();

        /// <summary>
        /// Randomizes the order of elements in a collection and returns a List<T>.
        /// </summary>
        public static List<T> Randomize<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source
                .OrderBy(_ => _random.Next())
                .ToList();
        }
    }
}
