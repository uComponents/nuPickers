
namespace nuComponents.DataTypes.Shared.LuceneDataSource
{
    using nuComponents.DataTypes.Shared.Editor;
    using System.Collections.Generic;

    public class LuceneDataSource
    {
        public string LuceneSearcher { get; set; }

        public string RawQuery { get; set; }
        
        public string KeyField { get; set; }
        
        public string LabelField { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            return editorDataItems;
        }
    }
}
