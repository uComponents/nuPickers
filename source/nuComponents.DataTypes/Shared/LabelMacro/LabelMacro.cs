
namespace nuComponents.DataTypes.Shared.LabelMacro
{
    using nuComponents.DataTypes.Shared.Picker;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco;
using umbraco.NodeFactory;
using umbraco.presentation.templateControls;

    public class LabelMacro
    {
        private string Alias { get; set; }

        private bool HasContext { get; set; }

        public LabelMacro(string alias)
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
        /// 
        /// </summary>
        /// <param name="key">passed by parameter into the macro</param>
        /// <param name="fallback">value to return if macro fails</param>
        /// <returns>the output of the macro as a string</returns>
        private string ProcessMacro(string key, string fallback)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="macroAlias">the macro to use</param>
        /// <param name="pickerEditorOptions">collection of options, this method may update their .Label properties</param>
        /// <returns></returns>
        public static IEnumerable<PickerEditorOption> ProcessPickerEditorOptions(string macroAlias, IEnumerable<PickerEditorOption> pickerEditorOptions)
        {
            if (macroAlias != null)
            {
                LabelMacro labelMacro = new LabelMacro(macroAlias);

                foreach (PickerEditorOption pickerEditorOption in pickerEditorOptions)
                {
                    pickerEditorOption.Markup = labelMacro.ProcessMacro(pickerEditorOption.Key, pickerEditorOption.Markup);
                }
            }

            return pickerEditorOptions;
        }
    }
}
