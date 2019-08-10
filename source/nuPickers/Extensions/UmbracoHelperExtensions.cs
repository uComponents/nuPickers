using Umbraco.Core.Models.PublishedContent;

namespace nuPickers.Extensions
{
    using Umbraco.Core.Models;
    using Umbraco.Web;

    internal static class UmbracoHelperExtensions
    {
        /// <summary>
        /// for a given umbraco Id, attempt to get the appropriate IPublishedContent (be that a typed content / media or member)
        /// </summary>
        /// <param name="umbracoHelper">The <see cref="UmbracoHelper"/> class on which this extension method is associated</param>
        /// <param name="id">The Umbraco string id (expected to be a guid) to get IPublishedContent for</param>
        /// <returns>null or IPublishedContent</returns>
        internal static IPublishedContent GetPublishedContent(this UmbracoHelper umbracoHelper, int id)
        {
            return umbracoHelper.GetPublishedContent(id.ToString());
        }

        /// <summary>
		/// for a given umbraco Id, attempt to get the appropriate IPublishedContent (be that a typed content / media or member)
		/// </summary>
		/// <param name="umbracoHelper">The <see cref="UmbracoHelper"/> class on which this extension method is associated</param>
		/// <param name="id">The Umbraco id to get IPublishedContent for</param>
		/// <returns>null or IPublishedContent</returns>
		internal static IPublishedContent GetPublishedContent(this UmbracoHelper umbracoHelper, string id)
        {
            IPublishedContent publishedContent = null;

            publishedContent = umbracoHelper.Content(id);

            if (publishedContent == null)
            {
                // fallback to attempting to get media
                publishedContent = umbracoHelper.Media(id);
            }

            if (publishedContent == null)
            {
                // fallback to attempting to get member
                try
                {
                    publishedContent = umbracoHelper.Member(id);
                }
                catch
                {
                    // HACK: suppress Umbraco error
                }
            }

            return publishedContent;
        }
    }
}