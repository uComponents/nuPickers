
namespace nuPickers.Shared.CustomLabel
{
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using umbraco;
    using umbraco.NodeFactory;
    using umbraco.presentation.templateControls;
    using System.IO;
    using System.Text;
    using System.Web.UI;

    public class CustomLabel
    {
        private string Alias { get; set; }

        private bool HasContext { get; set; }

        internal CustomLabel(string alias, int contextId)
        {
            this.Alias = alias;

            // the macro requires a published context to run in, and the current item being edited might not be a published page
            Node currentNode = uQuery.GetNode(contextId);
            if (currentNode != null)
            {
                HttpContext.Current.Items["pageID"] = contextId;
                this.HasContext = true;
            }
            else
            {
                 // find first published page to use as host
                 Node contextNode = uQuery.GetNodesByXPath(string.Concat("descendant::*[@parentID = ", uQuery.RootNodeId, "]")).FirstOrDefault();
                 if (contextNode != null)
                 {
                     HttpContext.Current.Items["pageID"] = contextNode.Id;
                     this.HasContext = true;
                 }
            }
        }

        /// <summary>
        /// parses the collection of options, potentially transforming the content of the label
        /// </summary>
        /// <param name="contextId">the content / media or member being edited</param>
        /// <param name="editorDataItems">collection of options</param>
        /// <returns></returns>
        public IEnumerable<EditorDataItem> ProcessEditorDataItems(IEnumerable<EditorDataItem> editorDataItems)
        {
            string keys = string.Join(", ", editorDataItems.Select(x => x.Key)); // csv of all keys
            int counter = 0;
            int total = editorDataItems.Count();

            foreach (EditorDataItem editorDataItem in editorDataItems)
            {
                counter++;
                editorDataItem.Label = this.ProcessMacro(editorDataItem.Key, editorDataItem.Label, keys, counter, total);
            }

            return editorDataItems.Where(x => !string.IsNullOrWhiteSpace(x.Label)); // remove any options without a label
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">passed by parameter into the macro</param>
        /// <param name="label">value to return if macro fails</param>
        /// <param name="keys">csv of all keys</param>
        /// <param name="counter">current postion</param>
        /// <param name="total">total number of keys</param>
        /// <returns>the output of the macro as a string</returns>
        private string ProcessMacro(string key, string label, string keys, int counter, int total)
        {
            if (!string.IsNullOrWhiteSpace(this.Alias) && this.HasContext)
            {
                Macro macro = new Macro() { Alias = this.Alias };
                macro.MacroAttributes.Add("key", key);
                macro.MacroAttributes.Add("label", label);
                macro.MacroAttributes.Add("keys", keys);
                macro.MacroAttributes.Add("counter", counter);
                macro.MacroAttributes.Add("total", total);

                label = this.RenderToString(macro);
            }

            return label;
        }

        /// <summary>
        /// Method added here to remove the need for the more generic ControlExtensions (as unlikely to need this functionality elsewhere)
        /// </summary>
        /// <param name="macro"></param>
        private string RenderToString(Macro macro)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (StringWriter stringWriter = new StringWriter(stringBuilder))
            using (HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter))
            {
                macro.RenderControl(htmlTextWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
