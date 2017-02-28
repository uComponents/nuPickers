
namespace nuPickers.Shared.TypeaheadListPicker
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;

    internal class TypeaheadListPicker
    {
        private string Typeahead { get; set; } // the value supplied by the user - the current typeahead text

        internal TypeaheadListPicker(string typeahead)
        {
            this.Typeahead = typeahead;
        }

        internal IEnumerable<EditorDataItem> ProcessEditorDataItems(IEnumerable<EditorDataItem> editorDataItems, bool StripHtmlTags = true)
        {
            if (this.Typeahead != null)
            {
                if (StripHtmlTags)
                {
                return editorDataItems.Where(x => this.StripHtmlTags(x.Label).IndexOf(this.Typeahead, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                else
                {
                    return editorDataItems.Where(x => x.Label.IndexOf(this.Typeahead, StringComparison.OrdinalIgnoreCase) >= 0);
                }
            }

            return editorDataItems;
        }

        private string StripHtmlTags(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }
    }
}
