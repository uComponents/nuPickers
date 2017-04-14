namespace nuPickers.PropertyEditors.JsonTypeaheadListPicker
{
    using ClientDependency.Core;
    using nuPickers.EmbeddedResource;
    using nuPickers.PropertyEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.JsonTypeaheadListPickerAlias, "nuPickers: Json TypeaheadList Picker", EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditor.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditorController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditorDirectives.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditorController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditorDirectives.js.nu")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "Editor/EditorResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js.nu")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "JsonDataSource/JsonDataSourceConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfigController.js.nu")]
    public class JsonTypeaheadPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new JsonTypeaheadListPickerPreValueEditor();
        }
    }
}