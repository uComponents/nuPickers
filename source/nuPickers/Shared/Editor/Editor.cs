
namespace nuPickers.Shared.Editor
{
    using DataSource;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Editor
    {
        /// <summary>
        /// Get all options for a picker (will be used by all API controllers, and the Picker obj)
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
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>(); // default return data

            if (dataSource != null)
            {
                dataSource.Typeahead = typeahead; // set any typeahead text that the datasource may filter on

                editorDataItems = dataSource.GetEditorDataItems(currentId, parentId);

                if (!string.IsNullOrWhiteSpace(customLabelMacro))
                {
                    editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias)
                                            .ProcessEditorDataItems(editorDataItems);
                }

                // if the datasource didn't handle the typeahead text, then it needs to be done post custom label processing
                if (!dataSource.HandledTypeahead && !string.IsNullOrWhiteSpace(typeahead))
                {
                    editorDataItems = new TypeaheadListPicker(typeahead)
                                            .ProcessEditorDataItems(editorDataItems);
                }
            }
            return editorDataItems;
        }
    }
}
