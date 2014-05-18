
namespace nuComponents.DataTypes.Shared.XmlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.CustomLabel;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XmlDataSourceApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            XmlDataSource xmlDataSource = ((JObject)config.dataSource).ToObject<XmlDataSource>();

            IEnumerable<PickerEditorOption> pickerEditorOptions = xmlDataSource.GetEditorOptions();

            return CustomLabel.ProcessPickerEditorOptions((string)config.customLabel, pickerEditorOptions);
        }
    }
}
