
namespace nuComponents.DataTypes.Shared.LabelMacro
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using System.Xml;
    using System.Xml.XPath;
    using umbraco;
    using umbraco.NodeFactory;
    using umbraco.presentation.templateControls;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using LegacyMacroService = umbraco.cms.businesslogic.macro.Macro; // aliasing as Macro classes exist in two namespaces
    using nuComponents.DataTypes.Shared.XmlDataSource;
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.Core;

    public class LabelMacro
    {
        private string Alias { get; set; }

        private bool HasContext 
        {
            get
            {
                return HttpContext.Current.Items.Contains("pageId");
            }
        }

        public LabelMacro(string alias)
        {
            this.Alias = alias;

            // set context
            Node contextNode = uQuery.GetNodesByXPath(string.Concat("descendant::*[@parentID = ", uQuery.RootNodeId, "]")).FirstOrDefault();
            if (contextNode != null)
            {
                HttpContext.Current.Items["pageID"] = contextNode.Id; // required deeper in macro.renderMacro to get context
            }

        }

        public string ProcessMacro(string key, string fallback)
        {
            string markup = fallback;

            if (!string.IsNullOrWhiteSpace(this.Alias))
            {
                Macro macro = new Macro() { Alias = this.Alias };
                macro.MacroAttributes.Add("key", key);

                markup = macro.RenderToString();
            }

            return markup;
        }
    }
}
