
using Umbraco.Core.Logging;

namespace nuPickers.PropertyEditors
{
    using Shared.SaveFormat;
    using Umbraco.Core.PropertyEditors;

    public abstract class BasePropertyEditor : DataEditor
    {
        protected override IDataValueEditor CreateValueEditor()  => new SaveFormatPropertyValueEditor(Attribute);


        protected BasePropertyEditor(ILogger logger, EditorType type = EditorType.PropertyValue) : base(logger, type)
        {
        }
    }
}
