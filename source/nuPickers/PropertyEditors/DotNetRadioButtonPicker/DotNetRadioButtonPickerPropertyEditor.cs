namespace nuPickers.PropertyEditors.DotNetRadioButtonPicker
{
    using ClientDependency.Core;
    using nuPickers.EmbeddedResource;
    using nuPickers.PropertyEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.DotNetRadioButtonPickerAlias, "nuPickers: DotNet RadioButton Picker", EmbeddedResource.RootUrlPrefixed + "RadioButtonPicker/RadioButtonPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrlPrefixed + "LayoutDirection/LayoutDirection.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "RadioButtonPicker/RadioButtonPickerEditorController.js.nu")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "Editor/EditorResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "DataSource/DataSourceResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "RelationMapping/RelationMappingResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatResource.js.nu")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrlPrefixed + "PropertyEditor/PropertyEditorConfig.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "DotNetDataSource/DotNetDataSourceConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "CustomLabel/CustomLabelConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "RelationMapping/RelationMappingConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrlPrefixed + "SaveFormat/SaveFormatConfigController.js.nu")]
    public class DotnetRadioButtonPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new DotNetRadioButtonPickerPreValueEditor();
        }
    }
}