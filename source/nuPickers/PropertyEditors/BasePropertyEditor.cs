
using Umbraco.Core.Logging;
using Umbraco.Web.Models.ContentEditing;

namespace nuPickers.PropertyEditors
{
    using nuPickers.Shared.SaveFormat;
    using Umbraco.Core.PropertyEditors;

    public abstract class BasePropertyEditor : DataEditor
    {
        protected override IDataValueEditor CreateValueEditor()  => new SaveFormatPropertyValueEditor(Attribute);


        protected BasePropertyEditor(ILogger logger, EditorType type = EditorType.PropertyValue) : base(logger, type)
        {
        }
    }
}
