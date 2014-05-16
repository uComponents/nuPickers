
namespace nuComponents.DataTypes.PropertyEditors.SqlListPicker
{
    using nuComponents.DataTypes.Interfaces;
    using Umbraco.Core.PropertyEditors;

    internal class SqlListPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("listPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [PreValueField("saveFormat", "Save Format", "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }

        [PreValueField("apiController", "SqlListPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
