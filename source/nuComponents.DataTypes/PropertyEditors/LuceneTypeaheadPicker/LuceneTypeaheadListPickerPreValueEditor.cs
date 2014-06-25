
namespace nuComponents.DataTypes.PropertyEditors.LuceneTypeaheadListPicker
{
    using Umbraco.Core.PropertyEditors;

    internal class LuceneTypeaheadListPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "LuceneDataSource/LuceneDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "Label Macro", EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("typeaheadListPicker", "", EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerConfig.html", HideLabel = true)]
        public string TypeaheadListPicker { get; set; }

        [PreValueField("listPicker", "", EmbeddedResource.RootUrl + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [PreValueField("relationMapping", "", EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
