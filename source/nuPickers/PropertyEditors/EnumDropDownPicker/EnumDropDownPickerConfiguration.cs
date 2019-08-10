
namespace nuPickers.PropertyEditors.EnumDropDownPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    public class EnumDropDownPickerConfiguration : ValueListConfiguration
    {
        [ConfigurationField("dataSource", "Data Source", EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public object SaveFormat { get; set; }
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
    }
}