
namespace nuComponents.DataTypes.PropertyEditors.XmlTypeAheadPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor("xmlTypeaheadPicker", "nuComponents: Xml Typeahead Picker", EmbeddedResource.RootUrl + "TypeaheadPicker/TypeaheadPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadPicker/TypeaheadPickerEditorController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadPicker/TypeaheadPickerEditorDirectives.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "XmlDataSource/XmlDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationTypeMapping/RelationTypeMappingConfigController.js")]
    public class XmlTypeaheadPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XmlTypeaheadPickerPreValueEditor();
        }
    }
}
