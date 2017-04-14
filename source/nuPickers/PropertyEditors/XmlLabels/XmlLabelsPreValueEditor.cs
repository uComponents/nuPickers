namespace nuPickers.PropertyEditors.XmlLabels
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class XmlLabelsPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrlPrefixed + "XmlDataSource/XmlDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "", EmbeddedResource.RootUrlPrefixed + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        /// <summary>
        /// currently no ui, but forces controller to be loaded
        /// </summary>
        [PreValueField("labels", "", EmbeddedResource.RootUrlPrefixed + "Labels/LabelsConfig.html", HideLabel = true)]
        public string Labels { get; set; }

        [PreValueField("layoutDirection", "Layout Direction", EmbeddedResource.RootUrlPrefixed + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }
    }
}
