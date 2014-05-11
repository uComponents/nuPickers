namespace nuComponents.DataTypes.PropertyEditors.XmlCheckBoxPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class XmlCheckBoxPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("labelMacro", "Label Macro", "App_Plugins/nuComponents/DataTypes/Shared/LabelMacro/LabelMacroConfig.html", Description = "expects a string parameter named 'key'")]
        public string LabelMacro { get; set; }

        [PreValueField("checkBoxPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public string CheckBoxPicker { get; set; }

        [PreValueField("apiController", "XmlCheckBoxPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
