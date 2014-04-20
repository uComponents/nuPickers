namespace nuComponents.DataTypes.XPathTemplatableList
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;

    [PropertyEditor("XPathTemplatableList", "XPath TemplatableList", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/XPathTemplatableListEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/XPathTemplatableListEditorController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/XPathTemplatableListPreValueController.js")]
    public class XPathTemplatableListPropertyEditor : PropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new XPathTemplatableListPreValueEditor();
        }
        
        internal class XPathTemplatableListPreValueEditor : PreValueEditor
        {
            [PreValueField("Xml Schema", "nodeType", Description = "xml schema to query")]
            public string XmlSchema { get; set; }

            [PreValueField("Options XPath", "requiredfield", Description = "all matched elements are used as options")]
            public string OptionsXPath { get; set; }

            // TODO: How best to set a default value ?
            [PreValueField("Key Attribute", "requiredfield", Description = "attribute value on each option to use as the key")]
            public string KeyAttribute { get; set; }

            //public string LabelAttribute { get; set; } // setting a macro would make this field redundant - perhaps this datatype has a default macro instead ?

            [PreValueField("Macro", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldMacro.html", Description = "macro expects an int parameter named 'id'")]
            public string Macro { get; set; }

            [PreValueField("Css File", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldCssFile.html", Description = "can use classes: .xpath-templatable-list.datattype-id-??.Property-alias=??")]
            public string CssFile { get; set; }

            [PreValueField("Script File", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueFieldScriptFile.html", Description = "executed after datatype has initialized")]
            public string ScriptFile { get; set; }

            [PreValueField("List Height", "number", Description = "fix height in px (and use scrollbar) - 0 means fluid")]
            public int ListHeight { get; set; }
            
            [PreValueField("Min Items", "number", Description = "number of items that must be selected")]
            public int MinItems { get; set; }

            [PreValueField("Max Items", "number", Description = "number of items that can be selected - 0 means no limit")]
            public int MaxItems { get; set; }

            [PreValueField("Allow Duplicates", "boolean", Description = "when true, duplicate items can be selected")]
            public bool AllowDuplicates { get; set; }
        }
    }
}
