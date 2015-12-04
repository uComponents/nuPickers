namespace nuPickers.Shared.JsonDataSource
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Logging;

    public class JsonDataSource
    {
        public string Url { get; set; }

        public string JsonPath { get; set; }

        public string KeyJsonPath { get; set; }

        public string LabelJsonPath { get; set; }

        /// <summary>
        /// Main method for retrieving nuPicker data items.
        /// </summary>
        /// <param name="contextId">Current context node Id</param>
        /// <returns>List of items for displaying inside a nuPicker JSON data type.</returns>
        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>(); // prepare return value

            JToken jToken = null; // object representation of all json source data

            try
            {
                jToken = JToken.Parse(Helper.GetDataFromUrl(this.Url));
            }
            catch (JsonException jsonException)
            {
                LogHelper.Error(typeof(nuPickers.Shared.JsonDataSource.JsonDataSource), "Check JSON at Url: " + this.Url, jsonException);
            }

            if (jToken != null)
            {
                // check for: [ 'a', 'b', 'c' ] and create editor items without using any JsonPath
                if (jToken is JArray && jToken.ToObject<object[]>().All(x => x is string))
                {
                    editorDataItems = jToken.ToObject<string[]>()
                                            .Select(x => new EditorDataItem()
                                            {
                                                Key = x,
                                                Label = x
                                            })
                                            .ToList();
                }
                else // use JsonPath
                {
                    IEnumerable<JToken> jTokens = null;

                    try
                    {
                        jTokens = jToken.SelectTokens(this.JsonPath);
                    }
                    catch (JsonException jsonException)
                    {
                        LogHelper.Error(typeof(nuPickers.Shared.JsonDataSource.JsonDataSource), "Check JSONPath: " + this.JsonPath, jsonException);
                    }

                    if (jTokens != null)
                    {
                        foreach(JToken jsonPathToken in jTokens.Where(x => x is JObject && x.HasValues))
                        {
                            JToken keyToken = jsonPathToken.SelectToken(this.KeyJsonPath);
                            JToken labelToken = jsonPathToken.SelectToken(this.LabelJsonPath);

                            if (keyToken != null && labelToken != null)
                            {
                                editorDataItems.Add(
                                    new EditorDataItem()
                                    {
                                        Key = keyToken.ToString(),
                                        Label =labelToken.ToString()
                                    });
                            }
                        }
                    }
                }
            }

            // TODO: distinct on editor data item keys

            return editorDataItems;
        }
    }
}
