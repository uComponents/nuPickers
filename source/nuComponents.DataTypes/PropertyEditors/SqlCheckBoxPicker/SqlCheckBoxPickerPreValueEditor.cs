
namespace nuComponents.DataTypes.PropertyEditors.SqlCheckBoxPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class SqlCheckBoxPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "Label Macro", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("checkBoxPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public string CheckBoxPicker { get; set; }

        [PreValueField("relationTypeMapping", "", "App_Plugins/nuComponents/DataTypes/Shared/RelationTypeMapping/RelationTypeMappingConfig.html", HideLabel = true)]
        public string RelationTypeMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }

        [PreValueField("apiController", "SqlDataSourceApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
