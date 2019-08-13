namespace nuPickers.DataEditors.XmlTypeaheadListPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class XmlTypeaheadListPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.ROOT_URL + "XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "Label Macro", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("typeaheadListPicker", "", EmbeddedResource.ROOT_URL + "TypeaheadListPicker/TypeaheadListPickerConfig.html", HideLabel = true)]
        public string TypeaheadListPicker { get; set; }

        [PreValueField("listPicker", "", EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [PreValueField("relationMapping", "", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
