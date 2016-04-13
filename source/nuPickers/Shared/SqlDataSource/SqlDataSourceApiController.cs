namespace nuPickers.Shared.SqlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class SqlDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetConnectionStrings()
        {
            List<string> connectionStrings = new List<string>();

            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                connectionStrings.Add(connectionString.Name);
            }

            return connectionStrings;
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            return GetEditorDataItems(currentId, parentId, propertyAlias, null, data);
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromUri] string ids, [FromBody] dynamic data)
        {
            int contextId = currentId;

            SqlDataSource sqlDataSource = ((JObject)data.config.dataSource).ToObject<SqlDataSource>();
            sqlDataSource.Typeahead = (ids != null) ? null : (string)data.typeahead;

            IEnumerable<EditorDataItem> editorDataItems = sqlDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            // if there are ids then ignore typeahead
            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => collectionIds.Contains(x.Key)).OrderBy(x => Array.FindIndex(collectionIds.ToArray(), y => y == x.Key));
                editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
            }
            else
            {
                // check whether typeahead should query the dataItems after processing them with the custom label
                bool isTypeaheadQueryOnCustomLabels = false;
                if (data.config.typeaheadListPicker != null && data.config.typeaheadListPicker.queryOnCustomLabels != null)
                {
                    bool.TryParse((string)data.config.typeaheadListPicker.queryOnCustomLabels, out isTypeaheadQueryOnCustomLabels);
                }

                if (isTypeaheadQueryOnCustomLabels)
                {
                    editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
                }

                // handle type ahead text
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);
                editorDataItems = typeaheadListPicker.ProcessEditorDataItems(editorDataItems);

                if (!isTypeaheadQueryOnCustomLabels)
                {
                    editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
                }
            }
            return editorDataItems;
        }
    }
}
