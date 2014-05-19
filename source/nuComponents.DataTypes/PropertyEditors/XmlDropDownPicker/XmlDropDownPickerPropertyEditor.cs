namespace nuComponents.DataTypes.PropertyEditors.XmlDropDownPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor("xmlDropDownPicker", "nuComponents: Xml DropDown Picker", "App_Plugins/nuComponents/DataTypes/Shared/DropDownPicker/DropDownPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/DropDownPicker/DropDownPickerEditorController.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/Picker/PickerResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, "App_Plugins/nuComponents/DataTypes/Shared/PropertyEditor/PropertyEditorConfig.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/XmlDataSource/XmlDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/RelationTypeMapping/RelationTypeMappingConfigController.js")]
    public class XmlDropDownPickerPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XmlDropDownPickerPreValueEditor();
        }
    }
}
