namespace nuPickers.Shared.SqlDataSource
{
    using System.Collections.Generic;
    using System.Configuration;
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
    }
}