namespace nuPickers.Shared.DotNetDataSource
{
    /// <summary>
    /// Implementing this interface will enable a dot-net-data-source to support returning a page sub-set of options 
    /// ItemsPerPage/Page passed in preference to Skip/Take, as can reliably convert from: ItemsPerPage/Page -> Skip/Take, the same cannot be said for: Skip/Take -> ItemsPerPage/Page
    /// </summary>
    public interface IDotNetDataSourcePaged //: IDotNetDataSourceKeyed
    {
        /// <summary>
        /// (set by nuPickers before query)
        /// </summary>
        int ItemsPerPage { set; }

        /// <summary>
        /// (set by nuPickers before query)
        /// </summary>
        int Page { set; }

        /// <summary>
        /// The total number of items available, as if skip=0 and take=infinite (read by nuPickers after query)
        /// </summary>
        int Total { get; }

        //IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, int itemsPerPage, int page, out int total);
    }
}