namespace nuComponents.DataTypes.PropertyEditors.SqlDropDownPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.SqlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class SqlDropDownPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            SqlDataSource sqlDataSource = ((JObject)config.dataSource).ToObject<SqlDataSource>();

            return sqlDataSource.GetEditorOptions();
        }
    }
}
