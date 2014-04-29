namespace nuComponents.DataTypes.XPathTemplatableList
{
    using Umbraco.Core.PropertyEditors;

    //was internal - made public for the API controller
    public class XPathTemplatableListPreValueEditor : PreValueEditor
    {
        [PreValueField("xmlSchema", "Xml Schema", "nodeType", Description = "xml schema to query")]
        public string XmlSchema { get; set; }

        [PreValueField("optionsXPath", "Options XPath", "requiredfield", Description = "all matched elements are used as options")]
        public string OptionsXPath { get; set; }

        // TODO: How best to set a default value ?
        [PreValueField("keyAttribute", "Key Attribute", "requiredfield", Description = "attribute on each matched xml element to use as the key (this must be unique)")]
        public string KeyAttribute { get; set; }

        [PreValueField("labelAttribute", "Label Attribute", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueLabelAttribute.html", Description = "attribute on each matched xml element to use as the label (not used if macro selected)")]
        public string LabelAttribute { get; set; }

        // TODO: rename
        [PreValueField("macro", "Label Macro", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueMacro.html", Description = "macro expects an int parameter named 'id'")]
        public string Macro { get; set; }

        [PreValueField("cssFile", "Css File", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueCssFile.html", Description = "can use classes: .xpath-templatable-list.datattype-id-??.Property-alias=??")]
        public string CssFile { get; set; }

        [PreValueField("scriptFile", "Script File", "App_Plugins/nuComponents/DataTypes/XPathTemplatableList/PreValueScriptFile.html", Description = "executed after datatype has initialized")]
        public string ScriptFile { get; set; }

        [PreValueField("listHeight", "List Height", "number", Description = "fix height in px (and use scrollbar) - 0 means fluid")]
        public int ListHeight { get; set; }

        [PreValueField("minItems", "Min Items", "number", Description = "number of items that must be selected")]
        public int MinItems { get; set; }

        [PreValueField("maxItems", "Max Items", "number", Description = "number of items that can be selected - 0 means no limit")]
        public int MaxItems { get; set; }

        [PreValueField("allowDuplicates", "Allow Duplicates", "boolean", Description = "when true, duplicate items can be selected")]            
        public bool AllowDuplicates { get; set; }

        [PreValueField("showUnselectable", "Show Unselectable", "boolean", Description = "when true, unselectable items are disabled, rather than hidden")]
        public bool ShowUnselectable { get; set; }
    }
}
