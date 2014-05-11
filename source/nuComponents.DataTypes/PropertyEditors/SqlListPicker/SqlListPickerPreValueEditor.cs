
namespace nuComponents.DataTypes.PropertyEditors.SqlListPicker
{
    using nuComponents.DataTypes.Interfaces;
    using Umbraco.Core.PropertyEditors;

    internal class SqlListPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        // get data
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        // transform data
        [PreValueField("customLabel", "Label Macro", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        // present data
        [PreValueField("listPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        // wire up
        [PreValueField("apiController", "SqlListPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
