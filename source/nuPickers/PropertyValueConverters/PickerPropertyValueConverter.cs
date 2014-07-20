
namespace nuPickers.PropertyValueConverters
{
    using System.Linq;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Core.Models.PublishedContent;
    using nuPickers.PropertyEditors;

    [PropertyValueType(typeof(Picker))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class PickerPropertyValueConverter : PropertyValueConverterBase
    {
        /// <summary>
        /// This is a generic converter for all nuPicker Picker PropertyEditors
        /// this may be subclassed for specific types (eg. Enum collection properties, or Xml and Content / Media / Members)
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(PublishedPropertyType propertyType)
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
                    .Contains(propertyType.PropertyEditorAlias);
        }

        public override object ConvertSourceToObject(PublishedPropertyType propertyType, object source, bool preview)
        {
            return new Picker(propertyType.ContentType.Id, propertyType.DataTypeId, source);
        }
    }
}
