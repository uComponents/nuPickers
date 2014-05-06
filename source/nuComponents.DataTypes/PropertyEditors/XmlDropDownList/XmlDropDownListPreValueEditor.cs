namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownList
{    
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class XmlDropDownListPreValueEditor : PreValueEditor, IPickerPreValueEditor
    {
        [PreValueField("", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        /// <summary>
        /// NOTE: the name 'XmlDropDownListApi' is injected into the 'ApiController' hidden field value
        /// </summary>
        [PreValueField("XmlDropDownListApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
