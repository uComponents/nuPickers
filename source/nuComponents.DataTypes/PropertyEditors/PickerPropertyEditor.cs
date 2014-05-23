// left this file here for reference - would be handy if PropertyEditorAsset attributes worked with inheritance, as all pickers share these resources:
namespace nuComponents.DataTypes.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/SaveFormat/SaveFormatResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/Picker/PickerResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/RelationTypeMapping/RelationTypeMappingConfigController.js")]
    public abstract class PickerPropertyEditor : BasePropertyEditor
    {
    }
}
