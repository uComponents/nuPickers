namespace nuPickers.Shared.DataSource
{
    using nuPickers.Shared.Editor;
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
        /// <param name="parentId">the parent id</param>
        /// <param name="typeahead"></param>
        /// <returns>collection of <see cref="EditorDataItem"/> POCOs that are used as options for a picker</returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string typeahead);

        /// <summary>
        /// Get the data items for the specified keys
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string[] keys);
    }
}