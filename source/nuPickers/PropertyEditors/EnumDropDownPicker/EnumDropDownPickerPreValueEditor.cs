
namespace nuPickers.PropertyEditors.EnumDropDownPicker
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class EnumDropDownPickerPreValueEditor : ConfigurationEditor
    {
        [ConfigurationField("dataSource", "", EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [ConfigurationField("saveFormat", "Save Format", EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}