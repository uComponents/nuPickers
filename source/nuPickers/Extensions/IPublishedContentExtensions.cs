using Umbraco.Core.Models.PublishedContent;

namespace nuPickers.Extensions
{
    using System.Linq;
    using Umbraco.Core.Models;

    public static class IPublishedContentExtensions
    {
        /// <summary>
        /// Get a Picker model for the supplied propertyAlias on this <see cref="IPublishedContent"/>
        /// </summary>
        /// <param name="publishedContent">The IPublishedContent that this extension method extends</param>
        /// <param name="propertyAlias">The property alias to get the Picker for</param>
        /// <returns>A <see cref="Picker"/> or null</returns>
        public static Picker GetPicker(this IPublishedContent publishedContent, string propertyAlias)
        {
            var propertyType = publishedContent.ContentType.PropertyTypes.SingleOrDefault(x => x.EditorAlias == propertyAlias);
            if (propertyType != null)
            {
                if (PickerPropertyValueConverter.IsPicker(propertyType.EditorAlias))
                {
                    return new Picker(publishedContent.Id, propertyAlias);
                }
            }

            return null;
        }
    }
}