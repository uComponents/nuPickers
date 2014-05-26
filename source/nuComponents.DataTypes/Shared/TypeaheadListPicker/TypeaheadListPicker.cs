
namespace nuComponents.DataTypes.Shared.TypeaheadListPicker
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using nuComponents.DataTypes.Shared.Picker;
    using System.Collections.Generic;

    internal class TypeaheadListPicker
    {
        private string Typeahead { get; set; } // the value supplied by the user - the current typeahead text

        internal TypeaheadListPicker(string typeahead)
        {
            this.Typeahead = typeahead;
        }

        internal IEnumerable<PickerEditorOption> ProcessPickerEditorOptions(IEnumerable<PickerEditorOption> pickerEditorOptions)
        {
            if (this.Typeahead != null)
            {
                return pickerEditorOptions.Where(x => this.StripHtmlTags(x.Label).IndexOf(this.Typeahead, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return pickerEditorOptions;
        }

        private string StripHtmlTags(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
    }
}
