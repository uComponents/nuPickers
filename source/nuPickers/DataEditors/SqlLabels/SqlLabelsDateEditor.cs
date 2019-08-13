using Umbraco.Core.Logging;

namespace nuPickers.DataEditors.SqlLabels
{
    using ClientDependency.Core;
    using EmbeddedResource;
    using DataEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [DataEditor(DataEditorConstants.SqlLabelsAlias, "nuPickers: Sql Labels", EmbeddedResource.ROOT_URL + "Labels/LabelsEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirection.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "Labels/LabelsEditorController.js" + EmbeddedResource.FILE_EXTENSION)]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "Editor/EditorResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js" + EmbeddedResource.FILE_EXTENSION)]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.ROOT_URL + "PropertyEditor/PropertyEditorConfig.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SqlDataSource/SqlDataSourceConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "Labels/LabelsConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    public class SqlLabelsDateEditor : DataEditor
    {
        protected override IConfigurationEditor CreateConfigurationEditor() => new SqlLabelsConfigurationEditor();

        public SqlLabelsDateEditor(ILogger logger) : base(logger)
        {
        }
    }
}