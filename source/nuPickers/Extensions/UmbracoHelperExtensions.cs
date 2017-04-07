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
		/// <param name="id">The Umbraco id to get IPublishedContent of</param>
		/// <returns>null or IPublishedContent</returns>
		internal static IPublishedContent GetPublishedContent(this UmbracoHelper umbracoHelper, int id)
        {
            IPublishedContent publishedContent = null;

            publishedContent = umbracoHelper.TypedContent(id);

            if (publishedContent == null)
            {
                // fallback to attempting to get media
                publishedContent = umbracoHelper.TypedMedia(id);
            }

            if (publishedContent == null)
            {
                // fallback to attempting to get member
                try
                {
                    publishedContent = umbracoHelper.TypedMember(id);
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