namespace nuComponents.DataTypes.PropertyEditors.EnumDropDownPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class EnumDropDownPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        /// <summary>
        /// NOTE: the name 'EnumDropDownPickerApi' is injected into the 'apiController' hidden field value
        /// </summary>
        [PreValueField("apiController", "EnumDropDownPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
