namespace nuComponents.DataTypes.PropertyEditors.EnumDropDownPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.EnumDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class EnumDropDownPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            EnumDataSource enumDataSource = ((JObject)config.dataSource).ToObject<EnumDataSource>();

            return enumDataSource.GetEditorOptions();
        }
    }
}
