
namespace nuPickers.Shared.SaveFormat
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    internal static class SaveFormat
    {
        /// <summary>
        /// ignore the specified saved format, and try and restore collection directly from the saved value
        /// </summary>
        /// <param name="savedValue"></param>
        /// <returns></returns>
        internal static IEnumerable<string> GetSavedKeys(string savedValue)
        {
            if (!string.IsNullOrWhiteSpace(savedValue))
            {
                switch (savedValue[0])
                {
                    case '[':
                        // TODO: check json is valid
                        return JsonConvert.DeserializeObject<JArray>(savedValue).Select(x => x["key"].ToString());

                    case '<':
                        // TODO: check xml is valid
                        return XDocument.Parse(savedValue).Descendants("Picked").Select(x => x.Attribute("Key").Value);

                    default: // csv
                        return savedValue.Split(',');                          
                }
            }

            return Enumerable.Empty<string>();
        }
    }
}
