
namespace nuComponents.DataTypes.PropertyEditors
{
    using nuComponents.DataTypes.Shared.SaveFormat;
    using Umbraco.Core.PropertyEditors;

    public abstract class BasePropertyEditor : PropertyEditor
    {
        protected override PropertyValueEditor CreateValueEditor()
        {
            return new SaveFormatPropertyValueEditor(base.CreateValueEditor());
        }
    }
}
