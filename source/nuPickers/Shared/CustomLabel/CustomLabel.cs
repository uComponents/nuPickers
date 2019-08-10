
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Composing;

namespace nuPickers.Shared.CustomLabel
{
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.UI;

    internal class CustomLabel
    {
        private string MacroAlias { get; set; }

        /// <summary>
        /// return true when there is a published page anywhere on the site
        /// </summary>
        [DefaultValue(false)]
        private bool HasMacroContext { get; set; }

        private int ContextId { get; set; }

        private string PropertyAlias { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="macroAlias">alias of Macro to execute</param>
        /// <param name="contextId">node, media or member id</param>
        /// <param name="propertyAlias">property alias</param>
        internal CustomLabel(string macroAlias, int contextId, string propertyAlias)
        {
            this.MacroAlias = macroAlias;
            this.ContextId = contextId;
            this.PropertyAlias = propertyAlias;

            // the macro requires a published context to run in
            IPublishedContent currentNode = Current.UmbracoContext.ContentCache.GetById(contextId);
            if (currentNode != null)
            {
                // current page is published so use this as the macro context
                HttpContext.Current.Items["pageID"] = contextId;
                this.HasMacroContext = true;
            }
            else
            {
                 // fallback nd find first published page to use as host
                 IPublishedContent contextNode = Current.UmbracoContext.ContentCache.GetSingleByXPath(string.Concat("descendant::*[@parentID = ", Current.Services.ContentService.GetRootContent().FirstOrDefault(), "]"));
                 if (contextNode != null)
                 {
                     HttpContext.Current.Items["pageID"] = contextNode.Id;
                     this.HasMacroContext = true;
                 }
            }

        }

        /// <summary>
        /// parses the collection of options, potentially transforming the content of the label
        /// </summary>
        /// <param name="contextId">the content / media or member being edited</param>
        /// <param name="editorDataItems">collection of options</param>
        /// <returns></returns>
        internal IEnumerable<EditorDataItem> ProcessEditorDataItems(IEnumerable<EditorDataItem> editorDataItems)
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
            if (!string.IsNullOrWhiteSpace(this.MacroAlias) && this.HasMacroContext)
            {
                Macro macro = new Macro() { Alias = this.MacroAlias };
/*
                macro.Properties.Add(new MacroProperty("contextId".ToLower(), this.ContextId.ToString()));
                macro.Properties.Add(new MacroProperty("propertyAlias".ToLower(), this.PropertyAlias));

                macro.Properties.Add("key", key);
                macro.Properties.Add("label", label);

                macro.Properties.Add("keys", keys);
                macro.Properties.Add("counter", counter);
                macro.Properties.Add("total", total);
*/
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
             //   macro.RenderControl(htmlTextWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
