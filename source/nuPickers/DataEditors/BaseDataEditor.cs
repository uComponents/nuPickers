
using nuPickers.Shared.SaveFormat;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors
{
    public abstract class BaseDataEditor : DataEditor
    {
        protected override IDataValueEditor CreateValueEditor()  => new SaveFormatPropertyValueEditor(Attribute);


        protected BaseDataEditor(ILogger logger) : base(logger)
        {
        }
    }
}
