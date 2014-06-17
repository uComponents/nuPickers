
namespace nuComponents.DataTypes.PropertyEditors.JsonRadioButtonPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using nuComponents.DataTypes.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.JsonRadioButtonPickerAlias, "nuComponents: Json RadioButton Picker", EmbeddedResource.RootUrl + "RadioButtonPicker/RadioButtonPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "LayoutDirection/LayoutDirection.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RadioButtonPicker/RadioButtonPickerEditorController.js")]

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "Editor/EditorResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "JsonDataSource/JsonDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationMapping/RelationMappingConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatConfigController.js")]
    public class JsonRadioButtonPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new JsonRadioButtonPickerPreValueEditor();
        }
    }
}
