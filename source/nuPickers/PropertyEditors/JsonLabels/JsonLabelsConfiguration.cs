namespace nuPickers.PropertyEditors.JsonLabels
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class JsonLabelsConfiguration : ValueListConfiguration
    {
        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }

        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.ROOT_URL + "JsonDataSource/JsonDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Custom Label", EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html",
            HideLabel = true)]
        public object CustomLabel { get; set; }

        /// <summary>
        /// currently no ui, but forces controller to be loaded
        /// </summary>
        [ConfigurationField("labels", "Labels", EmbeddedResource.ROOT_URL + "Labels/LabelsConfig.html", HideLabel = true)]
        public object Labels { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction",
            EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public object LayoutDirection { get; set; }
    }
}