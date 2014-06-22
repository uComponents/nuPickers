using System;
using nuComponents.DataTypes.Shared.XmlDataSource;
using Umbraco.Core.Logging;

namespace nuComponents.DataTypes.Shared.SqlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.CustomLabel;
    using nuComponents.DataTypes.Shared.Editor;
    using nuComponents.DataTypes.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
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
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromBody] dynamic data)
        {
            try
            {
                SqlDataSource sqlDataSource = ((JObject) data.config.dataSource).ToObject<SqlDataSource>();
                sqlDataSource.Typeahead = (string) data.typeahead;

                IEnumerable<EditorDataItem> editorDataItems = sqlDataSource.GetEditorDataItems(contextId);

                CustomLabel customLabel = new CustomLabel((string) data.config.customLabel, contextId);
                TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string) data.typeahead);

                // process the labels and then handle any type ahead text
                return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
            }
            catch (Exception e)
            {
                LogHelper.Error<SqlDataSourceApiController>("Error getting datasource data", e);
                throw e;
            }
        }
    }
}
