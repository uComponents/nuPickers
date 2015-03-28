namespace nuPickers.Shared.SqlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using System.Linq;

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
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            SqlDataSource sqlDataSource = ((JObject)data.config.dataSource).ToObject<SqlDataSource>();
            // if there are ids then ignore typeahead
            sqlDataSource.Typeahead = data.ids != null ? null : (string)data.typeahead;

            IEnumerable<EditorDataItem> editorDataItems = sqlDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);
            // if there are ids then ignore typeahead
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker(data.ids != null ? null : (string)data.typeahead);

            // process the labels and then handle any type ahead text
            editorDataItems = typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));

            // if there are ids then filter by ids
            if (data.ids != null)
        {
                IEnumerable<string> collectionIds = data.ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => collectionIds.Contains(x.Key));
            }

            return editorDataItems;

        }
    }
}
