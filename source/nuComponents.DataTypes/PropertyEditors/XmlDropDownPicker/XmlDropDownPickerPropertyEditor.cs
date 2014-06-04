namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using nuComponents.DataTypes.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.XmlDropDownPickerAlias, "nuComponents: Xml DropDown Picker", EmbeddedResource.RootUrl + "DropDownPicker/DropDownPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DropDownPicker/DropDownPickerEditorController.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "XmlDataSource/XmlDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationTypeMapping/RelationTypeMappingConfigController.js")]
    public class XmlDropDownPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XmlDropDownPickerPreValueEditor();
        }
    }
}
