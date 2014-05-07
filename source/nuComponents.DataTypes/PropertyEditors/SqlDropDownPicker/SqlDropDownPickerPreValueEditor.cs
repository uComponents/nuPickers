namespace nuComponents.DataTypes.PropertyEditors.SqlDropDownPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class SqlDropDownPickerPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("dataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        /// <summary>
        /// NOTE: the name 'SqlDropDownPickerApi' is injected into the 'apiController' hidden field value
        /// </summary>
        [PreValueField("apiController", "SqlDropDownPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
