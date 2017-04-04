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
            if (!string.IsNullOrWhiteSpace(value))
            {
                switch (value[0])
                {
                    case '[':
                        // TODO: check json is valid
                        return JsonConvert.DeserializeObject<JArray>(value).Select(x => x["key"].ToString());

                    case '<':
                        // TODO: check xml is valid
                        return XDocument.Parse(value).Descendants("Picked").Select(x => x.Attribute("Key").Value);

                    default: // csv
                        return value.Split(',');
                }
            }

            return Enumerable.Empty<string>();
        }


        /// <summary>
        /// Attempt to create a collection of the picked (key/label) items from the saved string value
        /// NOTE: the CSV format, or relations only will not work, as the data isn't in the saved value (will return false)
        /// </summary>
        /// <param name="savedValue">the saved value as a string</param>
        /// <param name="items"></param>
        /// <returns>bool flag to indicate whether the items could be created from the savedValue supplied</returns>
        internal static bool TryGetSavedItems(string savedValue, out IEnumerable<EditorDataItem> items)
        {
            throw new System.NotImplementedException();
        }
    }
}