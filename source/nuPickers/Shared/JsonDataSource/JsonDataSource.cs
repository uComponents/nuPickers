namespace nuPickers.Shared.JsonDataSource
{
    using DataSource;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using Paging;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core.Logging;

    public class JsonDataSource : IDataSource
    {
        private readonly IProfilingLogger _logger;
        public JsonDataSource(  IProfilingLogger profilingLogger)
        {
            _logger = profilingLogger;
        }

        public string Url { get; set; }

        public string JsonPath { get; set; }

        public string KeyJsonPath { get; set; }

        public string LabelJsonPath { get; set; }

        bool IDataSource.HandledTypeahead { get { return false; } }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems(currentId).Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = this.GetEditorDataItems(currentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        /// <summary>
        /// Main method for retrieving nuPicker data items.
        /// </summary>
        /// <param name="contextId">Current context node Id</param>
        /// <returns>List of items for displaying inside a nuPicker JSON data type.</returns>
        private IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>(); // prepare return value

            var url = this.Url.Replace("@contextId", contextId.ToString());

            JToken jToken = null; // object representation of all json source data

            try
            {
                jToken = JToken.Parse(Helper.GetDataFromUrl(url));
            }
            catch (JsonException jsonException)
            {
                _logger.Info<JsonDataSource>(  "{Message} (Check JSON at Url: {URL})",jsonException.Message, url  );

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
                        _logger.Info<JsonDataSource>(  "{Message} (Check JSONPath: {JsonPath})",jsonException.Message, JsonPath  );

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