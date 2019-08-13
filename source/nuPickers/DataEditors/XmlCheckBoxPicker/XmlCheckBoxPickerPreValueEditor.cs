namespace nuPickers.DataEditors.XmlCheckBoxPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class XmlCheckBoxPickerPreValueEditor ValueListConfiguration
    {
        [ConfigurationField("dataSource", "", EmbeddedResource.ROOT_URL + "XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("checkBoxPicker", "", EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public string CheckBoxPicker { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction", EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [ConfigurationField("relationMapping", "", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
