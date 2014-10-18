using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace nuPickers.Mapping
{
    /// <summary>
    /// Cache provider for the <c>System.Web.Caching.Cache</c>.
    /// </summary>
    public class DefaultCacheProvider : ICacheProvider
    {
        private TimeSpan _slidingExpiration = TimeSpan.FromMinutes(10);

        // Cannot insert null into cache, so use a static representation.
        private static object _nullValue = new object();

        private readonly Cache _cache;
        private readonly Guid _keyPrefix;

        /// <summary>
        /// Instantiates a new cache provider using the default ASP.NET cache
        /// (get it from <c>HttpContext.Current.Cache</c>).
        /// </summary>
        /// <param name="cache">The cache.  Cannot be null.</param>
        /// <param name="slidingExpiration">
        /// Optional override for the sliding expiration period
        /// of values added to the cache.
        /// </param>
        public DefaultCacheProvider(Cache cache, TimeSpan? slidingExpiration = null)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            if (slidingExpiration.HasValue)
            {
                _slidingExpiration = slidingExpiration.Value;
            }

            _cache = cache;
            _keyPrefix = Guid.NewGuid();
        }

        /// <see cref="ICacheProvider.Insert"/>
        public void Insert(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", "key");
            }
            else if (value == null)
            {
                value = _nullValue;
            }

            var qualifiedKey = GetQualifiedKey(key);

            _cache.Insert(qualifiedKey, value, null, Cache.NoAbsoluteExpiration, _slidingExpiration);
        }

        /// <see cref="ICacheProvider.Get"/>
        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be null or empty", "key");
            }

            var qualifiedKey = GetQualifiedKey(key);
            var value = _cache[qualifiedKey];

            return value == _nullValue ? null : value;
        }

        /// <see cref="ICacheProvider.ContainsKey"/>
        public bool ContainsKey(string key)
        {
            var qualifiedKey = GetQualifiedKey(key);

            return _cache[qualifiedKey] != null;
        }

        /// <summary>
        /// Removes all cache items added by this cache provider.
        /// </summary>
        public void Clear()
        {
            var enumerator = _cache.GetEnumerator();
            var keysToRemove = new List<string>();
            while (enumerator.MoveNext())
            {
                var key = enumerator.Key as string;

                if (key != null && key.StartsWith(_keyPrefix.ToString()))
                {
                    keysToRemove.Add(key);
                }
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// Constructs a cache key specific to this cache provider.
        /// </summary>
        /// <param name="key">The key to qualify.</param>
        private string GetQualifiedKey(string key)
        {
            return string.Format("{0}_{1}", _keyPrefix.ToString(), key);
        }
    }

    /// <summary>
    /// Convenience methods for the <see cref="ICacheProvider"/>, as required
    /// by <see cref="INodeMappingEngine"/>.
    /// </summary>
    public static class CacheProviderExtensions
    {
        private const string _propertyValueFormat = "{0}_{1}";
        private const string _aliasFormat = "{0}_DocumentTypeAlias";

        /// <summary>
        /// Inserts a node's property value into the cache.
        /// </summary>
        /// <param name="cache">The cache to use</param>
        /// <param name="id">The ID of the node</param>
        /// <param name="propertyName">The destination property name</param>
        /// <param name="value">The mapped value of the property</param>
        public static void InsertPropertyValue(this ICacheProvider cache, int id, string propertyName, object value)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }
            else if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("The property name cannot be null or empty", "propertyName");
            }

            var key = string.Format(_propertyValueFormat, id, propertyName);

            cache.Insert(key, value);
        }

        /// <summary>
        /// Inserts a node's alias into the cache.
        /// </summary>
        /// <param name="cache">The cache to use</param>
        /// <param name="id">The ID of the node</param>
        /// <param name="alias">The node's document type alias</param>
        public static void InsertAlias(this ICacheProvider cache, int id, string alias)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            var key = string.Format(_aliasFormat, id);

            cache.Insert(key, alias);
        }

        /// <summary>
        /// Gets a model's cached property value.
        /// </summary>
        public static object GetPropertyValue(this ICacheProvider cache, int id, string propertyName)
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }
            else if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentException("The property name cannot be null or empty", "propertyName");
            }

            var key = string.Format(_propertyValueFormat, id, propertyName);

            return cache.Get(key);
        }

        /// <summary>
        /// Gets a node's cached alias.
        /// </summary>
        public static string GetAlias(this ICacheProvider cache, int id)
        {
            var key = string.Format(_aliasFormat, id);

            return cache.Get(key) as string;
        }

        /// <summary>
        /// Check's if a model's property value is cached.
        /// </summary>
        public static bool ContainsPropertyValue(this ICacheProvider cache, int id, string propertyName)
        {
            return cache.ContainsKey(string.Format(_propertyValueFormat, id, propertyName));
        }

        /// <summary>
        /// Check's if a node's alias is cached.
        /// </summary>
        public static bool ContainsAlias(this ICacheProvider cache, int id)
        {
            return cache.ContainsKey(string.Format(_aliasFormat, id));
        }
    }
}
