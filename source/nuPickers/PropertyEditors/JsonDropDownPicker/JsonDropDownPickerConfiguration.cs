namespace nuPickers.PropertyEditors.JsonDropDownPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class JsonDropDownPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.ROOT_URL + "JsonDataSource/JsonDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping",
            EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format",
            EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }

        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
    }
}