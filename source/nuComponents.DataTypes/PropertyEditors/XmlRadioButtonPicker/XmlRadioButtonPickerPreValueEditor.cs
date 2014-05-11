
namespace nuComponents.DataTypes.PropertyEditors.XmlRadioButtonPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class XmlRadioButtonPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("labelMacro", "Label Macro", "App_Plugins/nuComponents/DataTypes/Shared/LabelMacro/LabelMacroConfig.html", Description = "expects a string parameter named 'key'")]
        public string LabelMacro { get; set; }

        [PreValueField("radioButtonPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/RadioButtonPicker/RadioButtonPickerConfig.html", HideLabel = true)]
        public string RadioButtonPicker { get; set; }

        [PreValueField("apiController", "XmlRadioButtonPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
