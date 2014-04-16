
namespace nuComponents.DataTypes.XPathDropDownList
{
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using ClientDependency.Core;

    [PropertyEditor("XPathDropDownList", "XPath DropDownList", "App_Plugins/nuComponents/DataTypes/XPathDropDown/XPathDropDownListEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathDropDown/XPathDropDownListController.js")]
    public class XPathDropDownListPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XPathDropDownListPreValueEditor();
        }


        internal class XPathDropDownListPreValueEditor : PreValueEditor
        {
            public XPathDropDownListPreValueEditor()
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
