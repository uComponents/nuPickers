
namespace nuComponents.DataTypes.PropertyEditors.EnumListPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // EDITOR UI
    [PropertyEditor("enumListPicker", "nuComponents: Enum List Picker", "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerEditor.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerEditorController.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/Picker/PickerResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/EnumDataSource/EnumDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/ListPicker/ListPickerConfigController.js")]
    public class EnumListPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new EnumListPickerPreValueEditor();
        }
    }
}
