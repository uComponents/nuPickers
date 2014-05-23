
namespace nuComponents.DataTypes.Shared.TypeaheadPicker
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using nuComponents.DataTypes.Shared.Picker;
    using System.Collections.Generic;

    internal class TypeaheadPicker
    {
        private string Typeahead { get; set; } // the value supplied by the user - the current typeahead text

        internal TypeaheadPicker(string typeahead)
        {
            this.Typeahead = typeahead;
        }

        internal IEnumerable<PickerEditorOption> ProcessPickerEditorOptions(IEnumerable<PickerEditorOption> pickerEditorOptions)
        {
            if (this.Typeahead != null)
            {
                return pickerEditorOptions.Where(x => this.StripHtmlTags(x.Label).StartsWith(this.Typeahead, StringComparison.OrdinalIgnoreCase));
            }

            return pickerEditorOptions;
        }

        private string StripHtmlTags(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
    }
}
