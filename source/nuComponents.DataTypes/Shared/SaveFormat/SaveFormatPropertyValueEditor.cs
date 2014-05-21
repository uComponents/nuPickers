
namespace nuComponents.DataTypes.Shared.SaveFormat
{
    using System.Xml.Linq;
    using Umbraco.Core.Models;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Core.Services;

    public class SaveFormatPropertyValueEditor : PropertyValueEditor
    {
        public SaveFormatPropertyValueEditor(PropertyValueEditor propertyValueEditor)
        {
            this.HideLabel = propertyValueEditor.HideLabel;
            this.View = propertyValueEditor.View;
            this.ValueType = propertyValueEditor.ValueType;
            foreach (IPropertyValidator propertyValidator in propertyValueEditor.Validators)
            {
                this.Validators.Add(propertyValidator);
            }
        }

        /// <summary>
        /// when saving to the xml cache, if the value can be converted to xml then ensure it's not wrapped in CData
        /// </summary>
        /// <param name="property"></param>
        /// <param name="propertyType"></param>
        /// <param name="dataTypeService"></param>
        /// <returns></returns>
        public override XNode ConvertDbToXml(Property property, PropertyType propertyType, IDataTypeService dataTypeService)
        {
            string value = this.ConvertDbToString(property, propertyType, dataTypeService);

            try
            {
                return XElement.Parse(value);
            }
            catch
            {
                return new XCData(value);
            }
        }
    }
}
