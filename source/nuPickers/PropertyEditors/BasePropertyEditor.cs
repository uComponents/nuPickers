
namespace nuPickers.PropertyEditors
{
    using nuPickers.Shared.SaveFormat;
    using Umbraco.Core.PropertyEditors;

    public abstract class BasePropertyEditor : PropertyEditor
    {
        protected override PropertyValueEditor CreateValueEditor()
        {
            return new SaveFormatPropertyValueEditor(base.CreateValueEditor());
        }
    }
}
