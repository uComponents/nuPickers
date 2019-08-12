using ClientDependency.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace nuPickers.DataEditors.JsonLabels
{
    // EDITOR UI
    [DataEditor(DataEditorConstants.JsonLabelsAlias, "nuPickers: Json Labels", EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirection.css" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsEditorController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "Editor/EditorResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.EmbeddedResource.ROOT_URL + "PropertyEditor/PropertyEditorConfig.css" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "JsonDataSource/JsonDataSourceConfigController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "Labels/LabelsConfigController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfigController.js" + EmbeddedResource.EmbeddedResource.FILE_EXTENSION)]
    public class JsonLabelsPropertyEditor : DataEditor
    {
        protected override IConfigurationEditor CreateConfigurationEditor() => new JsonLabelsConfigurationEditor();

        public JsonLabelsPropertyEditor(ILogger logger) : base(logger)
        {
        }
    }
}