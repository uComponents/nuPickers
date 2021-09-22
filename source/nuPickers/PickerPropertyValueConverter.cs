using System;
using nuPickers.DataEditors;
using Umbraco.Core;
using Umbraco.Web.Composing;

namespace nuPickers
{
    using System.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web;

    public class PickerPropertyValueConverter : PropertyValueConverterBase
    {
        public PickerPropertyValueConverter()
        {
        }

        /// <summary>
        /// This is a generic converter for all nuPicker Picker PropertyEditors
        /// </summary>
        /// <param name="publishedPropertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return IsPicker(propertyType.EditorAlias);
        }

        public override bool? IsValue(object value, PropertyValueLevel level)
        {
            // otherwise use the old magic null & string comparisons
            return value != null && (!(value is string) || string.IsNullOrWhiteSpace((string) value) == false);
        }


        /// <summary>
        /// Helper to check to see if the supplied propertyEditorAlias corresponds with a nuPicker Picker
        /// </summary>
        /// <param name="propertyEditorAlias"></param>
        /// <returns></returns>
        public static bool IsPicker(string propertyEditorAlias)
        {
            return new string[]
                {
                    DataEditorConstants.DotNetCheckBoxPickerAlias,
                    DataEditorConstants.DotNetDropDownPickerAlias,
                    DataEditorConstants.DotNetPagedListPickerAlias,
                    DataEditorConstants.DotNetPrefetchListPickerAlias,
                    DataEditorConstants.DotNetRadioButtonPickerAlias,
                    DataEditorConstants.DotNetTypeaheadListPickerAlias,
                    DataEditorConstants.EnumCheckBoxPickerAlias,
                    DataEditorConstants.EnumDropDownPickerAlias,
                    DataEditorConstants.EnumPrefetchListPickerAlias,
                    DataEditorConstants.EnumRadioButtonPickerAlias,
                    DataEditorConstants.JsonCheckBoxPickerAlias,
                    DataEditorConstants.JsonDropDownPickerAlias,
                    DataEditorConstants.JsonPagedListPickerAlias,
                    DataEditorConstants.JsonPrefetchListPickerAlias,
                    DataEditorConstants.JsonRadioButtonPickerAlias,
                    DataEditorConstants.JsonTypeaheadListPickerAlias,
                    DataEditorConstants.LuceneCheckBoxPickerAlias,
                    DataEditorConstants.LuceneDropDownPickerAlias,
                    DataEditorConstants.LucenePagedListPickerAlias,
                    DataEditorConstants.LucenePrefetchListPickerAlias,
                    DataEditorConstants.LuceneRadioButtonPickerAlias,
                    DataEditorConstants.LuceneTypeaheadListPickerAlias,
                    DataEditorConstants.SqlCheckBoxPickerAlias,
                    DataEditorConstants.SqlDropDownPickerAlias,
                    DataEditorConstants.SqlPagedListPickerAlias,
                    DataEditorConstants.SqlPrefetchListPickerAlias,
                    DataEditorConstants.SqlRadioButtonPickerAlias,
                    DataEditorConstants.SqlTypeaheadListPickerAlias,
                    DataEditorConstants.XmlCheckBoxPickerAlias,
                    DataEditorConstants.XmlDropDownPickerAlias,
                    DataEditorConstants.XmlPagedListPickerAlias,
                    DataEditorConstants.XmlPrefetchListPickerAlias,
                    DataEditorConstants.XmlRadioButtonPickerAlias,
                    DataEditorConstants.XmlTypeaheadListPickerAlias
                }
                .Contains(propertyEditorAlias);
        }

        public override object ConvertIntermediateToXPath(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            // source should come from ConvertSource and be a string (or null) already
            return inter;
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
            => PropertyCacheLevel.Element;

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(Picker);

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

            IPublishedContent assignedContentItem = owner as IPublishedContent;

            if (assignedContentItem != null)
            {
                contextId = assignedContentItem.Id;
                var pathIds = assignedContentItem.Path?.Split(new char[] { ',' }).Select(id => Convert.ToInt32(id)).ToArray();
                parentId = pathIds.Count() >= 2 ? pathIds[pathIds.Count() - 2] : parentId;
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
        /// Override the default behav our which duck types (converting a string that looks like a number into a number)
        /// to always return a string (or null)
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="source">expected as a string</param>
        /// <param name="preview">flag to indicate if in preview mode</param>
        /// <returns>source as a string, or null</returns>
        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType,
            object source,
            bool preview)
        {
            var attempt = source.TryConvertTo<string>();

            if (attempt.Success)
                return attempt.Result;

            return null;
        }
    }
}
