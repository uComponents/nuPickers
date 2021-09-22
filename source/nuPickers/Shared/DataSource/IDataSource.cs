using Umbraco.Core.PropertyEditors;

namespace nuPickers.Shared.DataSource
{
    using nuPickers.Shared.Editor;
    using Paging;
    using System.Collections.Generic;

    /// <summary>
    /// All datasources must implement this, so that Editor.GetGetEditorDataItems() can be used by all API controllers & the Picker obj
    /// </summary>
    internal interface IDataSource
    {
        /// <summary>
        /// flag to indicate whether the datasource handled any type ahead text
        /// (if the datasource doesn't handle it directly, then the typeahead text is processed later)
        /// </summary>
        bool HandledTypeahead { get; }

        /// <summary>
        /// Get the data items to pick from
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id (incase it's a new item so current = 0)</param>
        /// <param name="typeahead">any typeahead text that the datasource can filter on (can be null)</param>
        /// <returns>collection of <see cref="EditorDataItem"/> POCOs that are used as options for a picker</returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string typeahead);

        /// <summary>
        /// Get the data items for the specified keys
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id (incase it's a new item so current = 0)</param>
        /// <param name="keys">the collection of keys to get items for</param>
        /// <returns>collection of <see cref="EditorDataItem"/> POCOs that have been picked</returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string[] keys);

        /// <summary>
        /// Get the data items for a page
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id (incase it's a new item so current = 0)</param>
        /// <param name="pageMarker">specifies the itemsPerPage & page values</param>
        /// <param name="total">the total number of items in full data collection (not just subset requested)</param>
        /// <returns>collection of <see cref="EditorDataItem"/> POCOs that are used as options for a paged picker</returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total);
    }
}