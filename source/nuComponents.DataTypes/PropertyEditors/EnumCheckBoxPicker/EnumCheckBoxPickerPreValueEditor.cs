
namespace nuComponents.DataTypes.PropertyEditors.EnumCheckBoxPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class EnumCheckBoxPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("checkBoxPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public string CheckBoxPicker { get; set; }

        /// <summary>
        /// NOTE: the name 'EnumDropDownPickerApi' is injected into the 'apiController' hidden field value
        /// </summary>
        [PreValueField("apiController", "EnumCheckBoxPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
