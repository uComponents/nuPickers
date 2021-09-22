// left this file here for reference - would be handy if PropertyEditorAsset attributes worked with inheritance, as all pickers share these resources:

using nuPickers.EmbeddedResource;
using Umbraco.Core.Logging;

namespace nuComponents.DataTypes.PropertyEditors
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    // RESOURCES (all are referenced as EditorResource consumes the others)
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "Editor/EditorResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "DataSource/DataSourceResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "RelationMapping/RelationMappingResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.ROOT_URL + "SaveFormat/SaveFormatResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/Shared/RelationTypeMapping/RelationMappingConfigController.js")]
    public abstract class PickerDataEditor : DataEditor
    {
        protected PickerDataEditor(ILogger logger) : base(logger)
        {
        }
    }
}
