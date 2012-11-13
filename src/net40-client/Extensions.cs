using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VDS.Common
{
    /// <summary>
    /// Internal use extension methods
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Turns a single item into an enumerable
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="item">Item</param>
        /// <returns>Enumerable containing the single item</returns>
        internal static IEnumerable<T> AsEnumerable<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// Gets the safe string representation of an object which is the ToString() result for non-null objects and String.Empty otherwise
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns></returns>
        internal static String ToSafeString(this Object obj)
        {
            return (obj != null ? obj.ToString() : String.Empty);
        }
    }
}
