using ClientDependency.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace nuPickers.DataEditors.EnumLabels
{
    // EDITOR UI
    [DataEditor(DataEditorConstants.EnumLabelsAlias, "nuPickers: Enum Labels",
        EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirection.css" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsEditorController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "Editor/EditorResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "PropertyEditor/PropertyEditorConfig.css" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfigController.js" +
        EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsConfigController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfigController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    public class EnumLabelsDataEditor : DataEditor
    {
        protected override IConfigurationEditor CreateConfigurationEditor() => new EnumLabelsConfigurationEditor();

        public EnumLabelsDataEditor(ILogger logger) : base(logger)
        {
        }
    }
}