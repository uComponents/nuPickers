
namespace nuPickers.Shared.Editor
{
    using DataSource;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;

    internal static class Editor
    {
        /// <summary>
        /// Get all options for a picker
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="dataSource">the datasource</param>
        /// <param name="customLabelMacro">an optional macro to use for custom labels</param>
        /// <param name="typeahead">optional typeahead text to filter down on items returned</param>
        /// <returns></returns>
        internal static IEnumerable<EditorDataItem> GetEditorDataItems(                                                        
                                                        int currentId,
                                                        int parentId,
                                                        string propertyAlias, // used in custom label
                                                        IDataSource dataSource, 
                                                        string customLabelMacro,
                                                        string typeahead)
        {
            dataSource.Typeahead = typeahead; // set any typeahead text that the datasource may filter on

            IEnumerable<EditorDataItem> editorDataItems = dataSource.GetEditorDataItems(currentId, parentId);
 
            if (!string.IsNullOrWhiteSpace(customLabelMacro))
            {
                editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias)
                                        .ProcessEditorDataItems(editorDataItems);
            }

            // if the datasource didn't handle the typeahead text, then it needs to be done post custom label processing
            if (!dataSource.HandledTypeahead)
            {
                editorDataItems = new TypeaheadListPicker(typeahead)
                                        .ProcessEditorDataItems(editorDataItems);
            }

            return editorDataItems;
        }
    }
}
