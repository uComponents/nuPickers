namespace nuPickers.Shared.SaveFormat
{
    using Editor;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal static class SaveFormat
    {
        /// <summary>
        /// Ignore the specified saved format, and try and restore collection directly from the supplied string value
        /// </summary>
        /// <param name="value">the value as a string</param>
        /// <returns></returns>
        internal static IEnumerable<string> GetKeys(string value)
        {
            return SaveFormat.GetKeyValuePairs(value).Select(x => x.Key);
        }

        /// <summary>
        /// Attempt to create a collection of the picked (key/label) items from the saved string value
        /// NOTE: the CSV format, or relations only will not work, as the data isn't in the saved value (will return false)
        /// </summary>
        /// <param name="value">the saved value as a string</param>
        /// <param name="editorDataItems"></param>
        /// <returns>bool flag to indicate whether the items could be created from the savedValue supplied</returns>
        internal static bool TryGetDataEditorItems(string value, out IEnumerable<EditorDataItem> editorDataItems)
        {
            editorDataItems = SaveFormat.GetKeyValuePairs(value)
                                        .Where(x => x.Value != null)
                                        .Select(x => new EditorDataItem() { Key = x.Key, Label = x.Value });

            return editorDataItems.Count() > 0;
        }

        /// <summary>
        /// Keys are always stored, but labels are not stored in when using CSV or Relations Only
        /// </summary>
        /// <param name="value">the raw string value</param>
        /// <returns>a collection of key / label pairs, where labels are populated if in the source data (null indicates data not available in saved value)</returns>
        private static IEnumerable<KeyValuePair<string, string>> GetKeyValuePairs(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (value[0])
                {
                    case '[':
                        // TODO: check json is valid
                        return JsonConvert.DeserializeObject<JArray>(value).Select(x => new KeyValuePair<string, string>(x["key"].ToString(), x["label"].ToString()));

                    case '<':
                        // TODO: check xml is valid
                        return XDocument.Parse(value).Descendants("Picked").Select(x => new KeyValuePair<string, string>(x.Attribute("Key").Value, x.Value));

                    default: // csv
                        return value.Split(',').Select(x => new KeyValuePair<string, string>(x, null)); // NOTE: label is null
                }
            }

            return Enumerable.Empty<KeyValuePair<string, string>>();
        }
    }
}