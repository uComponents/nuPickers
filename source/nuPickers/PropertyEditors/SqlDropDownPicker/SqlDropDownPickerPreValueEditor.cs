namespace nuPickers.PropertyEditors.SqlDropDownPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class SqlDropDownPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrlPrefixed + "SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("relationMapping", "", EmbeddedResource.RootUrlPrefixed + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}