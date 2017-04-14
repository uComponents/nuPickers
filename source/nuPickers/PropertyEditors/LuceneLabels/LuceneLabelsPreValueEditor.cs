namespace nuPickers.PropertyEditors.LuceneLabels
{
    using nuPickers.EmbeddedResource;
    using Umbraco.Core.PropertyEditors;

    internal class LuceneLabelsPreValueEditor : PreValueEditor
    {
        [PreValueField("dataSource", "", EmbeddedResource.RootUrl + "LuceneDataSource/LuceneDataSourceConfig.html", HideLabel = true)]
        public string DataSource { get; set; }

        [PreValueField("customLabel", "", EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        /// <summary>
        /// currently no ui, but forces controller to be loaded
        /// </summary>
        [PreValueField("labels", "", EmbeddedResource.RootUrl + "Labels/LabelsConfig.html", HideLabel = true)]
        public string Labels { get; set; }

        [PreValueField("layoutDirection", "Layout Direction", EmbeddedResource.RootUrl + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }
    }
}