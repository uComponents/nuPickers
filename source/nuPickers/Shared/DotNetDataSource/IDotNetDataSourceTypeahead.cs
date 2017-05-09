namespace nuPickers.Shared.DotNetDataSource
{
    public interface IDotNetDataSourceTypeahead
    {
        string Typeahead { get; set; }

        //IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string typeahead);
    }
}