namespace nuComponents.DataTypes.Shared
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using umbraco.cms.businesslogic.macro;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponentsDataTypesShared")]
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
