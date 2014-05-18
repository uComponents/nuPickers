
namespace nuComponents.DataTypes.PropertyEditors.SqlCheckBoxPicker
{
    using Umbraco.Core.PropertyEditors;
    using nuComponents.DataTypes.Interfaces;

    internal class RelationMatchesPreValueEditor : PreValueEditor
    {
        // TODO: replace with a more generic RelationType picker - but need to be able to configure it 
        [PreValueField("relationMatches", "", "App_Plugins/nuComponents/DataTypes/Shared/RelationMatches/RelationMatchesConfig.html", HideLabel = true)]
        public string RelationMatches { get; set; }

        [PreValueField("customLabel", "", "App_Plugins/nuComponents/DataTypes/Shared/CustomLabel/CustomLabelConfig.html", HideLabel = true)]
        public string CustomLabel { get; set; }

        [PreValueField("layoutDirection", "Layout Direction", "App_Plugins/nuComponents/DataTypes/Shared/LayoutDirection/LayoutDirectionConfig.html")]
        public string LayoutDirection { get; set; }

        [PreValueField("apiController", "RelationMatchesApi", "App_Plugins/nuComponents/DataTypes/Shared/HiddenConstant/HiddenConstantConfig.html", HideLabel = true)]
        public string ApiController { get; set; }
    }
}
