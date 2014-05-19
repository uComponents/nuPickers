
namespace nuComponents.DataTypes.Shared.CustomLabel
{
    using nuComponents.DataTypes.Shared.Picker;
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

        internal CustomLabel(string alias)
        {
            this.Alias = alias;

            // set a context by finding the first published node
            Node contextNode = uQuery.GetNodesByXPath(string.Concat("descendant::*[@parentID = ", uQuery.RootNodeId, "]")).FirstOrDefault();
            if (contextNode != null)
            {
                HttpContext.Current.Items["pageID"] = contextNode.Id; // required deeper in macro.renderMacro to get context
                this.HasContext = true;
            }
        }

        /// <summary>
        /// parses the collection of options, potentially transforming the content of the label
        /// </summary>
        /// <param name="contextId">the content / media or member being edited</param>
        /// <param name="pickerEditorOptions">collection of options</param>
        /// <returns></returns>
        public IEnumerable<PickerEditorOption> ProcessPickerEditorOptions(int contextId, IEnumerable<PickerEditorOption> pickerEditorOptions)
        {
            foreach (PickerEditorOption pickerEditorOption in pickerEditorOptions)
            {
                pickerEditorOption.Label = this.ProcessMacro(pickerEditorOption.Key, pickerEditorOption.Label);
            }

            return pickerEditorOptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">passed by parameter into the macro</param>
        /// <param name="fallback">value to return if macro fails</param>
        /// <returns>the output of the macro as a string</returns>
        private string ProcessMacro(string key, string fallback)
        {
            string label = fallback;

            if (!string.IsNullOrWhiteSpace(this.Alias) && this.HasContext)
            {
                Macro macro = new Macro() { Alias = this.Alias };
                macro.MacroAttributes.Add("key", key);

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
