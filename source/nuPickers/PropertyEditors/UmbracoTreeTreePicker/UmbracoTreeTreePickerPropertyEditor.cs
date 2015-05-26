namespace nuPickers.PropertyEditors.UmbracoTreeTreePicker
{
    using ClientDependency.Core;

    using nuPickers.PropertyEditors;

    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.UmbracoTreeTreePickerAlias, "nuPickers: Umbraco Tree Tree Picker", EmbeddedResource.RootUrl + "TreePicker/TreePickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TreePicker/TreePickerEditorController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TreePicker/TreePickerEditorDialogController.js.nu")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "Editor/EditorResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingResource.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js.nu")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "UmbracoTreeDataSource/UmbracoTreeDataSourceConfigController.js.nu")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfigController.js.nu")]

    public class XmlTypeaheadPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new UmbracoTreeTreePickerPreValueEditor();
        }
    }
}