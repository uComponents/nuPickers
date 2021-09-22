﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using nuPickers.Shared.Editor;
using Umbraco.Core.Dictionary;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.Web.Composing;

namespace nuPickers.Shared.CustomLabel
{
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

        

        private readonly IUmbracoComponentRenderer _componentRenderer;

        /// <summary>
        ///
        /// </summary>
        /// <param name="macroAlias">alias of Macro to execute</param>
        /// <param name="contextId">node, media or member id</param>
        /// <param name="propertyAlias">property alias</param>
        internal CustomLabel(string macroAlias, int contextId, string propertyAlias, IUmbracoComponentRenderer componentRenderer)
        {
            _componentRenderer = componentRenderer;

            MacroAlias = macroAlias;
            ContextId = contextId;
            PropertyAlias = propertyAlias;

            // the macro requires a published context to run in
            IPublishedContent currentNode = Current.UmbracoContext.Content.GetById(contextId);
            if (currentNode != null)
            {
                // current page is published so use this as the macro context
                HttpContext.Current.Items["pageID"] = contextId;
                HasMacroContext = true;
            }
            else
            {
                // fallback nd find first published page to use as host
                IPublishedContent contextNode = Current.UmbracoContext.Content.GetSingleByXPath(
                    string.Concat("descendant::*[@parentID = ",
                        Current.Services.ContentService.GetRootContent().FirstOrDefault(), "]"));
                if (contextNode != null)
                {
                    HttpContext.Current.Items["pageID"] = contextNode.Id;
                    ContextId = contextNode.Id;
                    HasMacroContext = true;
                }
            }
        }

        /// <summary>
        /// parses the collection of options, potentially transforming the content of the label
        /// </summary>
        /// <param name="editorDataItems">collection of options</param>
        /// <returns></returns>
        internal IEnumerable<EditorDataItem> ProcessEditorDataItems(IEnumerable<EditorDataItem> editorDataItems)
        {
            var dataItems = editorDataItems.ToList();
            string keys = string.Join(", ", dataItems.Select(x => x.Key)); // csv of all keys
            int counter = 0;
            int total = dataItems.Count();

            foreach (EditorDataItem editorDataItem in dataItems)
            {
                counter++;
                editorDataItem.Label =
                    ProcessMacro(editorDataItem.Key, editorDataItem.Label, keys, counter, total);
            }

            return dataItems.Where(x => !string.IsNullOrWhiteSpace(x.Label)); // remove any options without a label
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
            if (!string.IsNullOrWhiteSpace(MacroAlias) && HasMacroContext)
            {
                Macro macro = new Macro {Alias = MacroAlias};
                Dictionary<string, object> properties = new Dictionary<string, object>();

                properties.Add("contextId".ToLower(), ContextId.ToString());
                properties.Add("propertyAlias".ToLower(), PropertyAlias);

                properties.Add("key", key);
                properties.Add("label", label);

                properties.Add("keys", keys);
                properties.Add("counter", counter);
                properties.Add("total", total);

                label = RenderToString(macro, properties);
            }

            return label;
        }

        /// <summary>
        /// Method added here to remove the need for the more generic ControlExtensions (as unlikely to need this functionality elsewhere)
        /// </summary>
        /// <param name="macro"></param>
        /// <param name="properties"></param>
        private string RenderToString(Macro macro, Dictionary<string, object> properties)
        {
            return _componentRenderer.RenderMacro(ContextId, macro.Alias, properties).ToHtmlString();
        }
    }
}