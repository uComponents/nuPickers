namespace nuPickers.PropertyEditors.EnumPrefetchListPicker
{
    using Umbraco.Core.Logging;
    using ClientDependency.Core;
    using nuPickers.EmbeddedResource;
    using nuPickers.PropertyEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [DataEditor(PropertyEditorConstants.EnumPrefetchListPickerAlias, "nuPickers: Enum PrefetchList Picker",
        EmbeddedResource.ROOT_URL + "PrefetchListPicker/PrefetchListPickerEditor.html", ValueType = "TEXT" )]
    [PropertyEditorAsset(ClientDependencyType.Css,
        EmbeddedResource.ROOT_URL + "ListPicker/ListPickerEditor.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "ListPicker/ListPickerEditorController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "ListPicker/ListPickerEditorDirectives.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "PrefetchListPicker/PrefetchListPickerEditorController.js" +
        EmbeddedResource.FILE_EXTENSION)]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "Editor/EditorResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js" + EmbeddedResource.FILE_EXTENSION)]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css,
        EmbeddedResource.ROOT_URL + "PropertyEditor/PropertyEditorConfig.css" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "EnumDataSource/EnumDataSourceConfigController.js" +
        EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "CustomLabel/CustomLabelConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    [PropertyEditorAsset(ClientDependencyType.Javascript,
        EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatConfigController.js" + EmbeddedResource.FILE_EXTENSION)]
    public class EnumPrefetchListPropertyEditor : BasePropertyEditor
    {
        protected override IConfigurationEditor CreateConfigurationEditor() =>
            new EnumPrefetchListPickerConfigurationEditor();


        public EnumPrefetchListPropertyEditor(ILogger logger) : base(logger)
        {
        }
    }
}