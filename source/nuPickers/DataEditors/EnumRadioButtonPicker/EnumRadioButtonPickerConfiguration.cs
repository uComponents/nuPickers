using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.EnumRadioButtonPicker
{
    internal class EnumRadioButtonPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro", EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public object CustomLabel { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction", EmbeddedResource.EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public object LayoutDirection { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}