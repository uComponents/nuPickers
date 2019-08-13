namespace nuPickers.DataEditors.SqlCheckBoxPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class SqlCheckBoxPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.ROOT_URL + "SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public object CustomLabel { get; set; }

        [ConfigurationField("checkBoxPicker", "Checkbox Picker", EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public object CheckBoxPicker { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction", EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public object LayoutDirection { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }
    }
}