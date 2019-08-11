namespace nuPickers.PropertyEditors.LucenePrefetchListPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class LucenePrefetchListPickerPreValueEditor : ValueListConfiguration
    {
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.ROOT_URL + "LuceneDataSource/LuceneDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [ConfigurationField("customLabel", "Custom Label", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("prefetchListPicker", "", EmbeddedResource.ROOT_URL + "PrefetchListPicker/PrefetchListPickerConfig.html", HideLabel = true)]
        public string PrefetchListPicker { get; set; }

        [ConfigurationField("listPicker", "List Picker", EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}