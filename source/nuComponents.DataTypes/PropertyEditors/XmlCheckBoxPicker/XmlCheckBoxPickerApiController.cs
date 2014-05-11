namespace nuComponents.DataTypes.PropertyEditors.XmlCheckBoxPicker
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.LabelMacro;
    using nuComponents.DataTypes.Shared.Picker;
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XmlCheckBoxPickerApiController : UmbracoAuthorizedJsonController, IPickerApiController
    {
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config)
        {
            XmlDataSource xmlDataSource = ((JObject)config.dataSource).ToObject<XmlDataSource>();

            IEnumerable<PickerEditorOption> pickerEditorOptions = xmlDataSource.GetEditorOptions();

            if (config.labelMacro != null)
            {
                LabelMacro labelMacro = new LabelMacro((string)config.labelMacro);

                foreach (PickerEditorOption pickerEditorOption in pickerEditorOptions)
                {
                    pickerEditorOption.Markup = labelMacro.ProcessMacro(pickerEditorOption.Key, pickerEditorOption.Markup);
                }
            }

            return pickerEditorOptions;
        }
    }
}
