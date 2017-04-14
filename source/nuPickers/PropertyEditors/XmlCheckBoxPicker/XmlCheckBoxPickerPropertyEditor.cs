namespace nuPickers.PropertyEditors.XmlCheckBoxPicker
{
    using ClientDependency.Core;
    using nuPickers.EmbeddedResource;
    using nuPickers.PropertyEditors;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.XmlCheckBoxPickerAlias, "nuPickers: Xml CheckBox Picker", EmbeddedResource.RootUrl + "CheckBoxPicker/CheckBoxPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "CheckBoxPicker/CheckBoxPickerEditor.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "LayoutDirection/LayoutDirection.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CheckBoxPicker/CheckBoxPickerEditorController.js.nu")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "Editor/EditorResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js.nu")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "XmlDataSource/XmlDataSourceConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfigController.js.nu")]
    public class XmlCheckBoxPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XmlCheckBoxPickerPreValueEditor();
        }
    }
}
