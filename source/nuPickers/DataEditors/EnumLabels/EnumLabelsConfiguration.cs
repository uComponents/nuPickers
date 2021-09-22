﻿using Umbraco.Core.PropertyEditors;

namespace nuPickers.DataEditors.EnumLabels
{
    internal class EnumLabelsConfiguration : ValueListConfiguration
    {
        [ConfigurationField("dataSource", "Data Source",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfig.html", HideLabel = true)]
        public object DataSource { get; set; }

        [ConfigurationField("customLabel", "Label Macro",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        /// <summary>
        /// currently no ui, but forces controller to be loaded
        /// </summary>
        [ConfigurationField("labels", "Labels", EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsConfig.html", HideLabel =
            true)]
        public string Labels { get; set; }

        [ConfigurationField("layoutDirection", "Layout Direction",
            EmbeddedResource.EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [ConfigurationField("useLabel", "Include labels?", "boolean", Description = "")]
        public bool UseLabel { get; set; }
    }
}