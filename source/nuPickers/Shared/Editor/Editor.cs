using System.Web.UI;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.Shared.Editor
{
    using DataSource;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.TypeaheadListPicker;
    using Paging;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Composing;
    using Umbraco.Web;

    internal static class Editor
    {

        public static IFactory Factory
        => Umbraco.Core.Composing.Current.Factory;

        public static IUmbracoComponentRenderer UmbracoComponentRenderer
                 => Factory.GetInstance<IUmbracoComponentRenderer>();



        /// <summary>
        /// Get a collection of all the (key/label) items for a picker (with optional typeahead)
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
                                                        string propertyAlias,
                                                        IDataSource dataSource,
                                                        string customLabelMacro,
                                                        string typeahead = null)
        {
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>(); // default return data

            if (dataSource != null)
            {
                editorDataItems = dataSource.GetEditorDataItems(currentId, parentId, typeahead); // both are passed as current id may = 0 (new content)

                if (!string.IsNullOrWhiteSpace(customLabelMacro))
                {
                    editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias, UmbracoComponentRenderer).ProcessEditorDataItems(editorDataItems);
                }

                // if the datasource didn't handle the typeahead text, then it needs to be done here (post custom label processing ?)
                if (!dataSource.HandledTypeahead && !string.IsNullOrWhiteSpace(typeahead))
                {
                    editorDataItems = new TypeaheadListPicker(typeahead).ProcessEditorDataItems(editorDataItems);
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
        internal static IEnumerable<EditorDataItem> GetEditorDataItems(
                                                        int currentId,
                                                        int parentId,
                                                        string propertyAlias,
                                                        IDataSource dataSource,
                                                        string customLabelMacro,
                                                        string[] keys)

        {
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>(); // default return data

            if (dataSource != null)
            {
                editorDataItems = dataSource.GetEditorDataItems(currentId, parentId, keys);

                if (!string.IsNullOrWhiteSpace(customLabelMacro))
                {
                    editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias, UmbracoComponentRenderer).ProcessEditorDataItems(editorDataItems);
                }

                // ensure sort order matches order of keys supplied
                editorDataItems = editorDataItems.OrderBy(x => keys.IndexOf(x.Key));
            }

            return editorDataItems;
        }

        /// <summary>
        /// Get a page of (key/label) items for a picker
        /// </summary>
        /// <param name="currentId">the current id</param>
        /// <param name="parentId">the parent id</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="dataSource">the datasource</param>
        /// <param name="customLabelMacro">an optional macro to use for custom labels</param>
        /// <param name="itemsPerPage">number of items per page</param>
        /// <param name="page">the page of (key/label) items to get</param>
        /// <returns>a collection of <see cref="EditorDataItem"/></returns>
        internal static IEnumerable<EditorDataItem> GetEditorDataItems(
                                                int currentId,
                                                int parentId,
                                                string propertyAlias,
                                                IDataSource dataSource,
                                                string customLabelMacro,
                                                int itemsPerPage,
                                                int page,
                                                out int total)
        {
            IEnumerable<EditorDataItem> editorDataItems = Enumerable.Empty<EditorDataItem>(); // default return data
            total = -1;

            if (dataSource != null)
            {
                editorDataItems = dataSource.GetEditorDataItems(
                                                currentId,
                                                parentId,
                                                new PageMarker(itemsPerPage, page),
                                                out total);

                if (!string.IsNullOrWhiteSpace(customLabelMacro))
                {
                    editorDataItems = new CustomLabel(customLabelMacro, currentId, propertyAlias, UmbracoComponentRenderer).ProcessEditorDataItems(editorDataItems);
                }
            }

            return editorDataItems;
        }
    }
}