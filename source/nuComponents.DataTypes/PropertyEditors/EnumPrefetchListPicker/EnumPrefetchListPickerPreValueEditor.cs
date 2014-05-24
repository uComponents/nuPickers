
namespace nuComponents.DataTypes.PropertyEditors.EnumListPicker
{
    using Umbraco.Core.PropertyEditors;

    internal class EnumPrefetchListPickerPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "", EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("prefetchListPicker", "", EmbeddedResource.RootUrl + "PrefetchListPicker/PrefetchListPickerConfig.html", HideLabel = true)]
        public string PrefetchListPicker { get; set; }

        [PreValueField("relationTypeMapping", "", EmbeddedResource.RootUrl + "RelationTypeMapping/RelationTypeMappingConfig.html", HideLabel = true)]
        public string RelationTypeMapping { get; set; }

        [PreValueField("saveFormat", "Save Format", EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfig.html")]
        public string SaveFormat { get; set; }
    }
}
