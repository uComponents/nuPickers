namespace nuComponents.DataTypes.XPathTemplatableList
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    [PropertyEditor("xPathTemplatableList", "XPath Templatable List", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/Editor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/Editor.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/ApiResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/EditorController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueCssFileController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueLabelAttributeController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueMacroController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueScriptFileController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueStateResource.js")]
    //[PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/ConfigurationController.js")]
    public class XPathTemplatableListPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XPathTemplatableListPreValueEditor();
        }
    }
}
