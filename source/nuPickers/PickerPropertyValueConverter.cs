namespace nuPickers
{
    using nuPickers.PropertyEditors;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    [PropertyValueType(typeof(Picker))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class PickerPropertyValueConverter : PropertyValueConverterBase
    {
        /// <summary>
        /// This is a generic converter for all nuPicker Picker PropertyEditors
        /// </summary>
        /// <param name="publishedPropertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(PublishedPropertyType publishedPropertyType)
        {
            return PickerPropertyValueConverter.IsPicker(publishedPropertyType.PropertyEditorAlias);
        }

        /// <summary>
        /// Helper to check to see if the supplied propertyEditorAlias corresponds with a nuPicker Picker
        /// </summary>
        /// <param name="propertyEditorAlias"></param>
        /// <returns></returns>
        public static bool IsPicker(string propertyEditorAlias)
        {
            return new string[] { 
                        PropertyEditorConstants.DotNetCheckBoxPickerAlias,
                        PropertyEditorConstants.DotNetDropDownPickerAlias,
                        PropertyEditorConstants.DotNetPrefetchListPickerAlias,
                        PropertyEditorConstants.DotNetRadioButtonPickerAlias,
                        PropertyEditorConstants.DotNetTypeaheadListPickerAlias,
                        PropertyEditorConstants.JsonCheckBoxPickerAlias,
                        PropertyEditorConstants.JsonDropDownPickerAlias,
                        PropertyEditorConstants.JsonPrefetchListPickerAlias,
                        PropertyEditorConstants.JsonRadioButtonPickerAlias,
                        PropertyEditorConstants.JsonTypeaheadListPickerAlias,
                        PropertyEditorConstants.LuceneCheckBoxPickerAlias,
                        PropertyEditorConstants.LuceneDropDownPickerAlias,
                        PropertyEditorConstants.LucenePrefetchListPickerAlias,
                        PropertyEditorConstants.LuceneRadioButtonPickerAlias,
                        PropertyEditorConstants.LuceneTypeaheadListPickerAlias,
                        PropertyEditorConstants.SqlCheckBoxPickerAlias,
                        PropertyEditorConstants.SqlDropDownPickerAlias,
                        PropertyEditorConstants.SqlPrefetchListPickerAlias,
                        PropertyEditorConstants.SqlRadioButtonPickerAlias,
                        PropertyEditorConstants.SqlTypeaheadListPickerAlias,
                        PropertyEditorConstants.XmlCheckBoxPickerAlias,
                        PropertyEditorConstants.XmlDropDownPickerAlias,
                        PropertyEditorConstants.XmlPrefetchListPickerAlias,
                        PropertyEditorConstants.XmlRadioButtonPickerAlias,
                        PropertyEditorConstants.XmlTypeaheadListPickerAlias,
                        PropertyEditorConstants.EnumCheckBoxPickerAlias,
                        PropertyEditorConstants.EnumDropDownPickerAlias,
                        PropertyEditorConstants.EnumPrefetchListPickerAlias,
                        PropertyEditorConstants.EnumRadioButtonPickerAlias
                    }
                 .Contains(propertyEditorAlias);
        }

        /// <summary>
        /// WARNING: currently PropertyValueConverters are unaware of their context, as such this should only be used by the current page
        /// https://our.umbraco.org/forum/developing-packages/85047-context-property-value-converters
        /// </summary>
        /// <param name="publishedPropertyType"></param>
        /// <param name="source"></param>
        /// <param name="preview"></param>
        /// <returns></returns>
        public override object ConvertSourceToObject(PublishedPropertyType publishedPropertyType, object source, bool preview)
        {
            int contextId = -1;
            int parentId = -1;
            IPublishedContent assignedContentItem;

            try
            {
                assignedContentItem = new UmbracoHelper(UmbracoContext.Current).AssignedContentItem;
            }
            catch
            {
                assignedContentItem = null;
            }

            if (assignedContentItem != null)
            {
                contextId = assignedContentItem.Id;

                if (assignedContentItem.Parent != null)
                {
                    parentId = assignedContentItem.Parent.Id;
                }
            }

            return new Picker(
                        contextId, 
                        parentId,
                        publishedPropertyType.PropertyTypeAlias, 
                        publishedPropertyType.DataTypeId, 
                        publishedPropertyType.PropertyEditorAlias, 
                        source);
        }
    }
}