
namespace nuComponents.DataTypes.PropertyEditors.EnumListPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.CustomLabel;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.EnumDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class EnumListPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            EnumDataSource enumDataSource = ((JObject)config.dataSource).ToObject<EnumDataSource>();

            IEnumerable<PickerEditorOption> pickerEditorOptions = enumDataSource.GetEditorOptions();

            return CustomLabel.ProcessPickerEditorOptions((string)config.customLabel, pickerEditorOptions);
        }
    }
}
