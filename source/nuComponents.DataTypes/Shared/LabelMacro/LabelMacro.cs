
namespace nuComponents.DataTypes.Shared.LabelMacro
{
    using System.Linq;
    using System.Web;
    using umbraco;
    using umbraco.NodeFactory;
    using umbraco.presentation.templateControls;

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

            // set a context by finding the first published node
            Node contextNode = uQuery.GetNodesByXPath(string.Concat("descendant::*[@parentID = ", uQuery.RootNodeId, "]")).FirstOrDefault();
            if (contextNode != null)
            {
                HttpContext.Current.Items["pageID"] = contextNode.Id; // required deeper in macro.renderMacro to get context
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">passed by parameter into the macro</param>
        /// <param name="fallback">value to return if macro fails</param>
        /// <returns>the output of the macro as a string</returns>
        public string ProcessMacro(string key, string fallback)
        {
            string markup = fallback;

            if (!string.IsNullOrWhiteSpace(this.Alias) && this.HasContext)
            {
                Macro macro = new Macro() { Alias = this.Alias };
                macro.MacroAttributes.Add("key", key);

                markup = macro.RenderToString();
            }

            return markup;
        }
    }
}
