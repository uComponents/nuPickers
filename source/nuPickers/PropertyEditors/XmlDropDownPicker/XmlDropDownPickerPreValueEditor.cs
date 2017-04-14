namespace nuPickers.PropertyEditors.XmlDropDownPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class XmlDropDownPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("relationMapping", "", EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
