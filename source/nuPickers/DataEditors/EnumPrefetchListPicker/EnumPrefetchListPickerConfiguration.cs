using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.EnumPrefetchListPicker
{
    internal class EnumPrefetchListPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }

        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("dataSource", "Label Macro",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("prefetchListPicker", "Prefetch List Picker",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "PrefetchListPicker/PrefetchListPickerConfig.html", HideLabel = true)]
        public object PrefetchListPicker { get; set; }

        [ConfigurationField("listPicker", "List Picker", EmbeddedResource.EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html",
            HideLabel = true)]
        public object ListPicker { get; set; }

        [ConfigurationField("saveFormat", "Save Format",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}