namespace nuPickers.PropertyEditors.EnumRadioButtonPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class EnumRadioButtonPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrlPrefixed + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "Label Macro", EmbeddedResource.RootUrlPrefixed + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("layoutDirection", "Layout Direction", EmbeddedResource.RootUrlPrefixed + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}