using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.LucenePrefetchListPicker
{
    internal class LucenePrefetchListPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }

        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "LuceneDataSource/LuceneDataSourceConfig.html", HideLabel =
                true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Custom Label",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public object CustomLabel { get; set; }

        [ConfigurationField("prefetchListPicker", "",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "PrefetchListPicker/PrefetchListPickerConfig.html", HideLabel =
                true)]
        public object PrefetchListPicker { get; set; }

        [ConfigurationField("listPicker", "List Picker",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public object ListPicker { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel =
                true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }
    }
}