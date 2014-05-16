namespace nuComponents.DataTypes.PropertyEditors.XmlListPicker
{
    using nuComponents.DataTypes.Interfaces;
    using Umbraco.Core.PropertyEditors;

    internal class XmlListPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        // get data
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        // transform data
        [PreValueField("customLabel", "", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        // present data
        [PreValueField("listPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [PreValueField("saveFormat", "Save Format", "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }

        // wire up
        [PreValueField("apiController", "XmlListPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
