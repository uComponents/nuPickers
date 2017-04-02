
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
        /// set any typeahead text so that it may be handled by the data source
        /// </summary>
        string Typeahead {set; }

        /// <summary>
        /// flag to indicate whether the datasource handled any type ahead text
        /// (if the datasource doesn't handle it directly, then the typeahead text is processed later)
        /// </summary>
        bool HandledTypeahead { get; }

        /// <summary>
        /// the main method on the datasource to return the data items
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id</param>
        /// <returns>collection of <see cref="EditorDataItem"/> POCOs that are used as options for a picker</returns>
        IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId);
    }
}
