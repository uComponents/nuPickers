namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownList
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XmlDropDownListApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<object> GetEditorOptions([FromBody] dynamic config)
        {
            XmlDataSource xmlDataSource = ((JObject)config.DataSource).ToObject<XmlDataSource>();

            return xmlDataSource.GetEditorOptions();
        }
    }
}
