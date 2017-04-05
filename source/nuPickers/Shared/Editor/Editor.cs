namespace nuPickers.Shared.Editor
{
    using DataSource;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.TypeaheadListPicker;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Editor
    {
        /// <summary>
        /// Get a collection of all the (key/label) items for a picker
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="dataSource">the datasource</param>
        /// <param name="customLabelMacro">an optional macro to use for custom labels</param>
        /// <param name="typeahead">optional typeahead text to filter down on items returned</param>
        /// <returns>a collection of <see cref="EditorDataItem"/></returns>
        internal static IEnumerable<EditorDataItem> GetEditorDataItems(                                                        
                                                        int currentId,
                                                        int parentId,
                                                        string propertyAlias, // used in custom label
                                                        IDataSource dataSource, 
                                                        string customLabelMacro,
                                                        string typeahead = null)
        {
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>(); // default return data

            if (dataSource != null)
            {
                dataSource.Typeahead = typeahead; // set any typeahead text that the datasource may filter on

                editorDataItems = dataSource.GetEditorDataItems(currentId, parentId); // both are passed as current id may = 0 (new content)

                if (!string.IsNullOrWhiteSpace(customLabelMacro))
                {
                    editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias)
                                            .ProcessEditorDataItems(editorDataItems);
                }

                // if the datasource didn't handle the typeahead text, then it needs to be done here (post custom label processing ?)
                if (!dataSource.HandledTypeahead && !string.IsNullOrWhiteSpace(typeahead))
                {
                    editorDataItems = new TypeaheadListPicker(typeahead)
                                            .ProcessEditorDataItems(editorDataItems);
                }
            }

            return editorDataItems;
        }

        /// <summary>
        /// Get a collection of the picked (key/label) items
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="dataSource">the datasource</param>
        /// <param name="customLabelMacro">an optional macro to use for custom labels</param>
        /// <returns>a collection of <see cref="EditorDataItem"/></returns>
        internal static IEnumerable<EditorDataItem> GetPickedEditorDataItems(
                                                        int currentId,
                                                        int parentId,
                                                        string propertyAlias, // used in custom label
                                                        IDataSource dataSource,
                                                        string customLabelMacro)

        {
            throw new NotImplementedException();
        }
    }
}