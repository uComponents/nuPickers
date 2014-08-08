
namespace nuPickers.Shared.JsonDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;

    public class JsonDataSource
    {
        public string Url { get; set; }

        public string JsonPath { get; set; }

        public string KeyJsonPath { get; set; }

        public string LabelJsonPath { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {

            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            var jsonDoc = JToken.Parse(GetContents(this.Url));

            if (jsonDoc != null)
            {
                //detect if the data provided is in object format, or is in array format.
                if (jsonDoc is JArray)
                {
                    var entries = jsonDoc.ToObject<string[]>();
                    editorDataItems = entries.Select(x => new EditorDataItem()
                    {
                        Key = x,
                        Label = x
                    }).ToList();
                }
                else if (jsonDoc is JObject)
                {
                    //Do the lookups
                    var jsonPathIterator = jsonDoc.SelectTokens(JsonPath).GetEnumerator();

                    List<string> keys = new List<string>(); // used to keep track of keys, so that duplicates aren't added

                    string key;
                    string label;

                    while (jsonPathIterator.MoveNext())
                    {
                        key = jsonPathIterator.Current.SelectToken(this.KeyJsonPath).Value<string>();

                        // only add item if it has a unique key - failsafe
                        if (!string.IsNullOrWhiteSpace(key) && !keys.Any(x => x == key))
                        {
                            // TODO: ensure key doens't contain any commas (keys are converted saved as csv)
                            keys.Add(key); // add key so that it's not reused

                            // set default markup to use the configured label XPath
                            label = jsonPathIterator.Current.SelectToken(this.LabelJsonPath).Value<string>();

                            editorDataItems.Add(new EditorDataItem()
                            {
                                Key = key,
                                Label = label
                            });
                        }
                    }
                }
            }
            else
            {
                //this should never happen
                // this means the json file wasnt a jArray or a jObject
            }


            return editorDataItems;
        }

        public IEnumerable<EditorDataItem> GetEditorDataItemsFilteredByIds(int contextId, string ids)
        {
            List<EditorDataItem> result = new List<EditorDataItem>();
            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                result = GetEditorDataItems(contextId).Where(x => ids.Contains(x.Key)).ToList<EditorDataItem>();
            }
            return result;
        }

        /// <summary>
        /// Downloads a url resource and returns it as a string. Maybe move this into a helpers class?
        /// </summary>
        /// <param name="url">URL to download the resource from</param>
        /// <returns>the string based result of the webcall</returns>
        private static string GetContents(string url)
        {
            using (WebClient client = new WebClient())
            {
                if (url.StartsWith("~/"))
                {
                    url = HttpContext.Current.Server.MapPath(url);
                }

                return client.DownloadString(url);
            }
        }
    }
}
