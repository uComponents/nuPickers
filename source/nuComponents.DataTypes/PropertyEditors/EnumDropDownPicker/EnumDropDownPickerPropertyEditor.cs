namespace nuComponents.DataTypes.PropertyEditors.EnumDropDownPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor("EnumDropDownPicker", "nuComponents: Enum DropDown Picker", "App_Plugins/nuComponents/DataTypes/Shared/DropDownPicker/DropDownPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/DropDownPicker/DropDownPickerEditorController.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/Picker/PickerResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/EnumDataSource/EnumDataSourceConfigController.js")]
    public class EnumDropDownPickerPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new EnumDropDownPickerPreValueEditor();
        }
    }
}
