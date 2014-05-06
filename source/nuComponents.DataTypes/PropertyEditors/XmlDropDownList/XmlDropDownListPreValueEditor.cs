namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownList
{
    using Umbraco.Core.PropertyEditors;

    internal class XmlDropDownListPreValueEditor : PreValueEditor
    {
        [PreValueField("xmlDataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string XmlDataSource { get; set; }

        /// <summary>
        /// NOTE: the name is injected into the hidden field value
        /// </summary>
        [PreValueField("apiController", "XmlDropDownListApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
