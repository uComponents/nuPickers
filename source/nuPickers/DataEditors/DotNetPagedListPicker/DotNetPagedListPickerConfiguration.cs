﻿using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.DotNetPagedListPicker
{
    internal class DotNetPagedListPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.EmbeddedResource.ROOT_URL + "DotNetDataSource/DotNetDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro",  EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [ConfigurationField("pagedListPicker", "Paged List Picker", EmbeddedResource.EmbeddedResource.ROOT_URL + "PagedListPicker/PagedListPickerConfig.html", HideLabel = true)]
        public object PagedListPicker { get; set; }

        [ConfigurationField("listPicker", "List Picker", EmbeddedResource.EmbeddedResource.ROOT_URL + "ListPicker/ListPickerConfig.html", HideLabel = true)]
        public object ListPicker { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping", EmbeddedResource.EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}