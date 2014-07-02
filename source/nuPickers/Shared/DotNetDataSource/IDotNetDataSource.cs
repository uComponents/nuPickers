
namespace nuPickers.Shared.DotNetDataSource
{
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;

    public interface IDotNetDataSource
    {
        // this is the same method signature for all the internal data sources too
        IEnumerable<EditorDataItem> GetEditorDataItems();
    }
}
