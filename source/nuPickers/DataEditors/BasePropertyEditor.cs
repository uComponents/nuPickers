
using nuPickers.Shared.SaveFormat;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors
{
    public abstract class BasePropertyEditor : DataEditor
    {
        protected override IDataValueEditor CreateValueEditor()  => new SaveFormatPropertyValueEditor(Attribute);


        protected BasePropertyEditor(ILogger logger, EditorType type = EditorType.PropertyValue) : base(logger, type)
        {
        }
    }
}
