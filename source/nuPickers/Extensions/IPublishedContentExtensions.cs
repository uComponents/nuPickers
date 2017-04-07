namespace nuPickers.Extensions
{
    using Umbraco.Core.Models;

    public static class IPublishedContentExtensions
    {
        /// <summary>
        /// Get a Picker model for the supplied propertyAlias on this <see cref="IPublishedContent"/>  
        /// </summary>
        /// <param name="publishedContent"></param>
        /// <param name="propertyAlias"></param>
        /// <returns></returns>
        public static Picker GetPicker(this IPublishedContent publishedContent, string propertyAlias)
        {
            return new Picker(publishedContent.Id, propertyAlias);
        }
    }
}