namespace nuPickers.Shared.DotNetDataSource
{
    /// <summary>
    /// Implementing this interface will enable a dot-net-data-source to support returning returning only those options matching specific keys
    /// This is required when using a csv save format (that doesn't store any label data) & a data source dependant on paging or typeahead values to return data
    /// </summary>
    public interface IDotNetDataSourceKeyed
    {
        /// <summary>
        /// (set by nuPickers before query)
        /// </summary>
        string[] Keys { set; }

        //IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string[] keys);
    }
}