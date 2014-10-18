using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nuPickers.Mapping
{
    /// <summary>
    /// A cache provider which satisfies the caching requirements of uMapper.
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Inserts an object into the cache, replacing an existing object
        /// if one already exists with the same <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A key to store the value against.</param>
        /// <param name="value">
        /// The value to set. If <c>null</c>, the inserted value should be
        /// retrievable as <c>null</c>, and return true for <see cref="ContainsKey"/>.
        /// </param>
        void Insert(string key, object value);

        /// <summary>
        /// Gets an object from the cache.
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns>
        /// If the value for <paramref name="key"/> is <c>null</c>, returns <c>null</c>.
        /// Also, returns <c>null</c> if <paramref name="key"/> has no value.
        /// </returns>
        object Get(string key);

        /// <summary>
        /// Checks if the cache contains a value for the key.
        /// </summary>
        /// <param name="key">The key</param>
        bool ContainsKey(string key);

        /// <summary>
        /// Clears the cache.
        /// </summary>
        void Clear();
    }
}
