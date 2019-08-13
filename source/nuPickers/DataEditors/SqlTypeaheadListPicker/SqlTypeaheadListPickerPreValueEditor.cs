namespace nuPickers.DataEditors.SqlTypeaheadListPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class SqlTypeaheadListPickerPreValueEditor ValueListConfiguration
    {
        [ConfigurationField("dataSource", "", EmbeddedResource.ROOT_URL + "SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("typeaheadListPicker", "", EmbeddedResource.ROOT_URL + "TypeaheadListPicker/TypeaheadListPickerConfig.html", HideLabel = true)]
        public string TypeaheadListPicker { get; set; }

        [ConfigurationField("listPicker", "", EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public string ListPicker { get; set; }

        [ConfigurationField("relationMapping", "", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public string RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
