namespace nuPickers.Extensions
{
    using System.Linq;
    using Umbraco.Core.Models;

    public static class IPublishedContentExtensions
    {
        /// <summary>
        /// Get a Picker model for the supplied propertyAlias on this <see cref="IPublishedContent"/>  
        /// </summary>
        /// <param name="publishedContent"></param>
        /// <param name="propertyAlias"></param>
        /// <returns>A <see cref="Picker"/> or null</returns>
        public static Picker GetPicker(this IPublishedContent publishedContent, string propertyAlias)
        {
            var propertyType = publishedContent.ContentType.PropertyTypes.SingleOrDefault(x => x.PropertyTypeAlias == propertyAlias);
            if (propertyType != null)
            {
                if (PickerPropertyValueConverter.IsPicker(propertyType.PropertyEditorAlias))
                {
                    return new Picker(publishedContent.Id, propertyAlias);
                }
            }

            return null;
        }
    }
}