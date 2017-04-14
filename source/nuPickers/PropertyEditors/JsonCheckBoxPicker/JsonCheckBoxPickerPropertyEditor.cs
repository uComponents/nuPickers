namespace nuPickers.PropertyEditors.JsonCheckBoxPicker
{
    using ClientDependency.Core;
    using nuPickers.EmbeddedResource;
    using nuPickers.PropertyEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.JsonCheckBoxPickerAlias, "nuPickers: Json CheckBox Picker", EmbeddedResource.RootUrlPrefixed + "CheckBoxPicker/CheckBoxPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrlPrefixed + "CheckBoxPicker/CheckBoxPickerEditor.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrlPrefixed + "LayoutDirection/LayoutDirection.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "CheckBoxPicker/CheckBoxPickerEditorController.js.nu")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "Editor/EditorResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "DataSource/DataSourceResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "RelationMapping/RelationMappingResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatResource.js.nu")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrlPrefixed + "PropertyEditor/PropertyEditorConfig.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "JsonDataSource/JsonDataSourceConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "CustomLabel/CustomLabelConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "RelationMapping/RelationMappingConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatConfigController.js.nu")]
    public class JsonCheckBoxPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new JsonCheckBoxPickerPreValueEditor();
        }
    }
}