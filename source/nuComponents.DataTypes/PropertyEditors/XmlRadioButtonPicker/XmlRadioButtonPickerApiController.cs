
namespace nuComponents.DataTypes.PropertyEditors.XmlRadioButtonPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XmlRadioButtonPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            XmlDataSource xmlDataSource = ((JObject)config.dataSource).ToObject<XmlDataSource>();

            return xmlDataSource.GetEditorOptions();
        }
    }
}

