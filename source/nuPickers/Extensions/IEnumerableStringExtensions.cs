using Umbraco.Core.Models.PublishedContent;

namespace nuPickers.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    internal static class IEnumerableStringExtensions
    {

        /// <summary>
        /// TODO: migrate out of Picker obj
        /// parse a collection of strings, and attempt to return a collection of IPublishedContent
        /// </summary>
        /// <param name="keys"></param>
        /// <returns>a collection (populated, or empty)</returns>
        internal static IEnumerable<IPublishedContent> AsPublishedContent(this IEnumerable<string> keys, IUmbracoContextFactory contextFactory)
        {

            using (var cf = contextFactory.EnsureUmbracoContext())
            {
                var cache = cf.UmbracoContext.ContentCache;
                return keys
                    .Select(x =>  cache.GetByRoute(x))
                    .Where(x => x != null);
            }
        }
    }
}