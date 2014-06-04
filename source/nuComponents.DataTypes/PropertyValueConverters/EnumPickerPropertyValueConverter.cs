
namespace nuComponents.DataTypes.PropertyValueConverters
{
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Core.Models.PublishedContent;
    using nuComponents.DataTypes.PropertyEditors;

    [PropertyValueType(typeof(Picker))]
    [PropertyValueCache(PropertyCacheValue.All, PropertyCacheLevel.Content)]
    public class EnumPickerPropertyValueConverter : PropertyValueConverterBase
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return new string[] { 
                        PropertyEditorConstants.EnumCheckBoxPickerAlias,
                        PropertyEditorConstants.EnumDropDownPickerAlias,
                        PropertyEditorConstants.EnumPrefetchListPickerAlias,
                        PropertyEditorConstants.EnumRadioButtonPickerAlias,
                    }
                    .Contains(propertyType.PropertyTypeAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            return new EnumPicker(propertyType.ContentType.Id, propertyType.DataTypeId, source);
        }
    }
}
