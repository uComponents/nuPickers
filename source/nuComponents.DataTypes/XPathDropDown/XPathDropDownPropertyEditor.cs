
namespace nuComponents.DataTypes.XPathDropDown
{
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using ClientDependency.Core;

    [PropertyEditor("XPathDropDown", "XPath DropDown", "App_Plugins/nuComponents/DataTypes/XPathDropDown/XPathDropDownEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathDropDown/XPathDropDownController.js")]
    public class XPathDropDownPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XPathDropDownPreValueEditor();
        }


        internal class XPathDropDownPreValueEditor : PreValueEditor
        {
            public XPathDropDownPreValueEditor()
            {
                Fields.Add(new PreValueField()
                    {
                        Description = "all matched nodes are used as drop down options",
                        Key = "xPath",
                        View = "requiredfield",
                        Name = "XPath Expression"
                    });
            }
        }

    }
}
