using Umbraco.Core.Logging;

namespace nuPickers.DataEditors.SqlCheckBoxPicker
{
    using ClientDependency.Core;
    using EmbeddedResource;
    using DataEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [DataEditor(DataEditorConstants.SqlCheckBoxPickerAlias, "nuPickers: Sql CheckBox Picker", EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerEditor.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.ROOT_URL + "LayoutDirection/LayoutDirection.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "CheckBoxPicker/CheckBoxPickerEditorController.js" + EmbeddedResource.FILE_EXTENSION)]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "Editor/EditorResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js" + EmbeddedResource.FILE_EXTENSION)]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.ROOT_URL + "PropertyEditor/PropertyEditorConfig.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SqlDataSource/SqlDataSourceConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    public class SqlCheckBoxPickerDataEditor : DataEditor
    {
        protected override IConfigurationEditor CreateConfigurationEditor() =>
            new SqlCheckBoxPickerConfigurationEditor();

        public SqlCheckBoxPickerDataEditor(ILogger logger) : base(logger)
        {
        }
    }
}
