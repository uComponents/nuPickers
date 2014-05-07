namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuComponents.DataTypes.Shared.Core;

    [PluginController("nuComponents")]
    public class XmlDropDownPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            XmlDataSource xmlDataSource = ((JObject)config.DataSource).ToObject<XmlDataSource>();

            return xmlDataSource.GetEditorOptions();
        }
    }
}
