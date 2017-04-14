namespace nuPickers.PropertyEditors.EnumCheckBoxPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class EnumCheckBoxPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "", EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("checkBoxPicker", "", EmbeddedResource.RootUrl + "CheckBoxPicker/CheckBoxPickerConfig.html", HideLabel = true)]
        public string CheckBoxPicker { get; set; }

        [PreValueField("layoutDirection", "Layout Direction", EmbeddedResource.RootUrl + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}