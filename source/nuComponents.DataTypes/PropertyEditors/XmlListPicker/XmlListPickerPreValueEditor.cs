namespace nuComponents.DataTypes.PropertyEditors.XmlListPicker
{
    using Umbraco.Core.PropertyEditors;

    internal class XmlListPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("xmlDataSource", "", "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string XmlDataSource { get; set; }

        [PreValueField("listPicker", "", "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        /// <summary>
        /// tells the list picker editor which controller it should use use (maybe better to share data between XmlDataSource & ListPicker instead ?)
        /// NOTE: the name is injected into the hidden field value
        /// </summary>
        [PreValueField("listPickerApiController", "XmlListPickerApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ListPickerApiController { get; set; }
    }
}
