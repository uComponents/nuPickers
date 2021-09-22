﻿namespace nuPickers.DataEditors.LuceneTypeaheadListPicker
{
    using EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class LuceneTypeaheadListPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.ROOT_URL + "LuceneDataSource/LuceneDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public object CustomLabel { get; set; }

        [ConfigurationField("typeaheadListPicker", "Type ahead list Picker", EmbeddedResource.ROOT_URL + "TypeaheadListPicker/TypeaheadListPickerConfig.html", HideLabel = true)]
        public object TypeaheadListPicker { get; set; }

        [ConfigurationField("listPicker", "List Picker", EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public object ListPicker { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping", EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}