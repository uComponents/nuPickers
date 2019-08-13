using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.SqlDropDownPicker
{
    internal class SqlDropDownPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }

        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.EmbeddedResource.ROOT_URL + "SqlDataSource/SqlDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("relationMapping", "Relation Mapping", EmbeddedResource.EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfig.html", HideLabel = true)]
        public object RelationMapping { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }

    }
}