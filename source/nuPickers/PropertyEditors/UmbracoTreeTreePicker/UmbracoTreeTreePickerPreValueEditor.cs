namespace nuPickers.PropertyEditors.UmbracoTreeTreePicker
{
    using Umbraco.Core.PropertyEditors;

    internal class UmbracoTreeTreePickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "UmbracoTreeDataSource/UmbracoTreeDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("treePicker", "", EmbeddedResource.RootUrl + "TreePicker/TreePickerConfig.html", HideLabel = true)]
        public string TreePicker { get; set; }

        [PreValueField("relationMapping", "", EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}