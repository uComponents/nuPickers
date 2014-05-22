namespace nuComponents.DataTypes.Shared.SqlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.CustomLabel;
    using nuComponents.DataTypes.Shared.Picker;
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
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromUri] int contextId, [FromBody] dynamic config)
        {
            SqlDataSource sqlDataSource = ((JObject)config.dataSource).ToObject<SqlDataSource>();

            IEnumerable<PickerEditorOption> pickerEditorOptions = sqlDataSource.GetEditorOptions(contextId);

            CustomLabel customLabel = new CustomLabel((string)config.customLabel, contextId);

            return customLabel.ProcessPickerEditorOptions(pickerEditorOptions);
        }
    }
}
