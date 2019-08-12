namespace nuPickers.PropertyEditors.DotNetCheckBoxPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class DotNetCheckBoxPickerConfiguration : ValueListConfiguration
    {

        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.ROOT_URL + "DotNetDataSource/DotNetDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }

        [ConfigurationField("customLabel", "Custom Label", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("checkBoxPicker", "Checkbox Picker", EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public object CheckBoxPicker { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction", EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }
    }
}
