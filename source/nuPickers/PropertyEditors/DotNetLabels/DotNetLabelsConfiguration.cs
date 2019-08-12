namespace nuPickers.PropertyEditors.DotNetLabels
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class DotNetLabelsConfiguration : ValueListConfiguration
    {
        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.ROOT_URL + "DotNetDataSource/DotNetDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Custom Label",
            EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        /// <summary>
        /// currently no ui, but forces controller to be loaded
        /// </summary>
        [ConfigurationField("labels", "Labels", EmbeddedResource.ROOT_URL + "Labels/LabelsConfig.html", HideLabel =
            true)]
        public string Labels { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction",
            EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
    }
}