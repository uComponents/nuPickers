
namespace nuPickers.Shared.DotNetDataSource
{
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DotNetDataSource
    {
        public string AssemblyName { get; set; }

        public string ClassName { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems()
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            return ((IDotNetDataSource)Activator.CreateInstance(this.AssemblyName, this.ClassName).Unwrap())
                        .GetEditorDataItems()
                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value });
        }
    }
}
