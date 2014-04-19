namespace nuComponents.DataTypes.XPathTemplatableList
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    
    [PropertyEditor("XPathTemplatableList", "XPath TemplatableList", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/XPathTemplatableListEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/XPathTemplatableListController.js")]
    public class XPathTemplatableListPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XPathTemplatableListPreValueEditor();
        }

        internal class XPathTemplatableListPreValueEditor : PreValueEditor
        {
            public XPathTemplatableListPreValueEditor()
            {
                this.Fields.Add(new PreValueField()
                {
                    Key = "type",
                    Description = "xml schema to query",
                    View = "nodeType",
                    Name = "Type"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "xPath",
                    Description = "all matched nodes are used as drop down options",
                    View = "requiredfield",
                    Name = "XPath Expression"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "listHeight",
                    Description = "fixed height in px - 0 means not set",
                    View = "number",
                    Name = "List Height"
                });

                
                this.Fields.Add(new PreValueField()
                {
                    Key = "macro",
                    Description = "macro expects an int parameter named 'id'",
                    View = "/Umbraco/App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldMacro.html",
                    Name = "Macro"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "cssFile",
                    Description = "can use classes: .xpath-templatable-list.datattype-id-??.Property-alias=??",
                    View = "/Umbraco/App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldCssFile.html",
                    Name = "Css File"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "scriptFile",
                    Description = "executed after datatype has initialized",
                    View = "/Umbraco/App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldScriptFile.html",
                    Name = "Script File"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "minItems",
                    Description = "number of items that must be selected",
                    View = "number",
                    Name = "Min Items"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "maxItems",
                    Description = "number of items that can be selected - 0 means no limit",
                    View = "number",
                    Name = "Max Items"
                });

                this.Fields.Add(new PreValueField()
                {
                    Key = "allowDuplicates",
                    Description = "when true, duplicate items can be selected",
                    View = "boolean",
                    Name = "Allow Duplicates"
                });
            }
        }
    }
}
