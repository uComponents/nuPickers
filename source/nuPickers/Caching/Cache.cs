namespace nuPickers.Caching
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Runtime.Caching;

    internal sealed class Cache
    {
        /// <summary>
        /// Singleton instance of the <see cref="Cache"/> class
        /// </summary>
        private static readonly Cache cache = new Cache();

        /// <summary>
        /// Internal instance of the <see cref="MemoryCache"/> class
        /// </summary>
        private MemoryCache memoryCache;

        /// <summary>
        /// Locker collection of all cache keys currently having their 'expensive functions' evaluated
        /// string = cacheKey, object = used as a locking object
        /// </summary>
        private ConcurrentDictionary<string, object> cacheKeysBeingHandled;

        static Cache()
        {
        }

        /// <summary>
        /// Private constructor
        /// </summary>
        private Cache()
        {
            this.memoryCache = new MemoryCache("nuPickers");

            this.cacheKeysBeingHandled = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Get the instance of this cache
        /// </summary>
        internal static Cache Instance
        {
            get
            {
                return cache;
            }
        }

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        internal T Get<T>(string cacheKey)
        {
            bool found;

            return this.Get<T>(cacheKey, out found);
        }

        /// <summary>
        /// Queries key in cache for object of type T
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get</param>
        /// <param name="found">output parameter, indicates whether the return value was found in the cache</param>
        /// <returns>an object from cache of type T, else default(T)</returns>
        internal T Get<T>(string cacheKey, out bool found)
        {
            object obj = this.memoryCache[cacheKey];

            if (obj is T)
            {
                found = true;
                return (T)obj;
            }

            found = false;
            return default(T);
        }

        /// <summary>
        /// Queries key in cache for object of type T, otherwise executes expensiveFunc and ensures cache is set
        /// </summary>
        /// <typeparam name="T">type of object expected</typeparam>
        /// <param name="cacheKey">key to the cache item to get (or set if not found)</param>
        /// <param name="expensiveFunc">function to execute if cache item not found</param>
        /// <param name="timeout">(optional) number of seconds before cache value should time out, default 0 = no timeout</param>
        /// <returns>an object from cache of type T, else the result of the expensiveFunc</returns>
        internal T GetSet<T>(string cacheKey, Func<T> expensiveFunc, int timeout = 0)
        {
            bool found;
            T cachedObject = this.Get<T>(cacheKey, out found);

            if (!found)
            {
                this.cacheKeysBeingHandled.TryAdd(cacheKey, new object()); // object used as a locker

                lock (this.cacheKeysBeingHandled[cacheKey])
                {
                    // re-check to see if another thread beat us to setting this value
                    cachedObject = this.Get<T>(cacheKey, out found);
                    if (!found)
                    {
                        cachedObject = expensiveFunc();

                        this.Set(cacheKey, cachedObject, timeout);
                    }
                }

                object obj;
                this.cacheKeysBeingHandled.TryRemove(cacheKey, out obj);
            }

            return cachedObject;
        }

        /// <summary>
        /// Populate the cache key with the supplied object
        /// </summary>
        /// <param name="cacheKey">key for the cache item to set</param>
        /// <param name="objectToCache">object to put into the cache item</param>
        /// <param name="timeout">(optional) number of seconds before cache value should time out, default 0 = no timeout</param>
        internal void Set(string cacheKey, object objectToCache, int timeout = 0)
        {
            if (objectToCache != null)
            {
                if (timeout > 0)
                {
                    this.memoryCache.Set(
                        cacheKey,
                        objectToCache,
                        new CacheItemPolicy() { AbsoluteExpiration = DateTime.Now.AddSeconds(timeout) });
                }
                else
                {
                    this.memoryCache[cacheKey] = objectToCache;
                }
            }
        }

        /// <summary>
        /// Deletes an item from cache
        /// </summary>
        /// <param name="cacheKey">the key to the item to remove</param>
        internal void Remove(string cacheKey)
        {
            this.memoryCache.Remove(cacheKey);
        }

        /// <summary>
        /// Wipe all cache items
        /// </summary>
        internal void Wipe()
        {
            foreach (string key in this.memoryCache.Select(x => x.Key))
            {
                this.memoryCache.Remove(key);
            }
        }
    }
}