namespace nuPickers.DataEditors.XmlDropDownPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class XmlDropDownPickerPreValueEditor ValueListConfiguration
    {
        [ConfigurationField("dataSource", "", EmbeddedResource.ROOT_URL + "XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [ConfigurationField("relationMapping", "", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
