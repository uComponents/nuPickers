using System;
using System.Web;

namespace nuPickers
{
    using nuPickers.PropertyEditors;
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    [PropertyType(typeof(Picker))]
    [PropertyCache(PropertyCacheValue.All, PropertyCacheLevel.Element)]
    public class PickerPropertyValueConverter : PropertyValueConverterBase
    {
        /// <summary>
        /// This is a generic converter for all nuPicker Picker PropertyEditors
        /// </summary>
        /// <param name="publishedPropertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return IsPicker(propertyType.EditorAlias);
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
                        PropertyEditorConstants.DotNetPagedListPickerAlias,
                        PropertyEditorConstants.DotNetPrefetchListPickerAlias,
                        PropertyEditorConstants.DotNetRadioButtonPickerAlias,
                        PropertyEditorConstants.DotNetTypeaheadListPickerAlias,
                        PropertyEditorConstants.EnumCheckBoxPickerAlias,
                        PropertyEditorConstants.EnumDropDownPickerAlias,
                        PropertyEditorConstants.EnumPrefetchListPickerAlias,
                        PropertyEditorConstants.EnumRadioButtonPickerAlias,
                        PropertyEditorConstants.JsonCheckBoxPickerAlias,
                        PropertyEditorConstants.JsonDropDownPickerAlias,
                        PropertyEditorConstants.JsonPagedListPickerAlias,
                        PropertyEditorConstants.JsonPrefetchListPickerAlias,
                        PropertyEditorConstants.JsonRadioButtonPickerAlias,
                        PropertyEditorConstants.JsonTypeaheadListPickerAlias,
                        PropertyEditorConstants.LuceneCheckBoxPickerAlias,
                        PropertyEditorConstants.LuceneDropDownPickerAlias,
                        PropertyEditorConstants.LucenePagedListPickerAlias,
                        PropertyEditorConstants.LucenePrefetchListPickerAlias,
                        PropertyEditorConstants.LuceneRadioButtonPickerAlias,
                        PropertyEditorConstants.LuceneTypeaheadListPickerAlias,
                        PropertyEditorConstants.SqlCheckBoxPickerAlias,
                        PropertyEditorConstants.SqlDropDownPickerAlias,
                        PropertyEditorConstants.SqlPagedListPickerAlias,
                        PropertyEditorConstants.SqlPrefetchListPickerAlias,
                        PropertyEditorConstants.SqlRadioButtonPickerAlias,
                        PropertyEditorConstants.SqlTypeaheadListPickerAlias,
                        PropertyEditorConstants.XmlCheckBoxPickerAlias,
                        PropertyEditorConstants.XmlDropDownPickerAlias,
                        PropertyEditorConstants.XmlPagedListPickerAlias,
                        PropertyEditorConstants.XmlPrefetchListPickerAlias,
                        PropertyEditorConstants.XmlRadioButtonPickerAlias,
                        PropertyEditorConstants.XmlTypeaheadListPickerAlias
                    }
                 .Contains(propertyEditorAlias);
        }

        /// <summary>
        /// WARNING: currently PropertyValueConverters are unaware of their context, as such this should only be used by the current page
        /// https://our.umbraco.org/forum/developing-packages/85047-context-property-value-converters
        /// </summary>
        /// <param name="publishedPropertyType"></param>
        /// <param name="source">expected as a string</param>
        /// <param name="preview"></param>
        /// <returns></returns>
        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
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
                        propertyType.EditorAlias,
                        propertyType.DataType.Id,
                        propertyType.Alias,
                        inter);
        }

        /// <summary>
        /// Override the default behavour which duck types (converting a string that looks like a number into a number)
        /// to always return a string (or null)
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="source">expected as a string</param>
        /// <param name="preview">flag to indicate if in preview mode</param>
        /// <returns>source as a string, or null</returns>
        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source,
            bool preview)
        {
            return source as string;
        }




    }
}