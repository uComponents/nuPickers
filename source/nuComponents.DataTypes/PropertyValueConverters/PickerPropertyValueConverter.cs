
namespace nuComponents.DataTypes.PropertyValueConverters
{
    using System.Linq;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Core.Models.PublishedContent;
    using nuComponents.DataTypes.PropertyEditors;

    [PropertyValueType(typeof(Picker))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class PickerPropertyValueConverter : PropertyValueConverterBase
    {
        /// <summary>
        /// This is a generic converter for ALL nuComponents Picker PropertyEditors
        /// this may be subclassed for specific types (eg. Enum collection properties, or Xml and Content / Media / Members)
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return new string[] { 
                        PropertyEditorConstants.SqlCheckBoxPickerAlias,
                        PropertyEditorConstants.SqlDropDownPickerAlias,
                        PropertyEditorConstants.SqlPrefetchListPickerAlias,
                        PropertyEditorConstants.SqlRadioButtonPickerAlias,
                        PropertyEditorConstants.SqlTypeaheadListPickerAlias,
                        PropertyEditorConstants.XmlCheckBoxPickerAlias,
                        PropertyEditorConstants.XmlDropDownPickerAlias,
                        PropertyEditorConstants.XmlPrefetchListPickerAlias,
                        PropertyEditorConstants.XmlRadioButtonPickerAlias,
                        PropertyEditorConstants.XmlTypeaheadListPickerAlias
                    }
                    .Contains(propertyType.PropertyTypeAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return new Picker(propertyType.ContentType.Id, propertyType.DataTypeId, source);
        }
    }
}
