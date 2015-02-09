
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();
            
            JToken jToken = JToken.Parse(GetDataFromUrl(this.Url));

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
                else // use JsonPaths
                {
                    editorDataItems = jToken.SelectTokens(this.JsonPath)
                                            .Where(x => x is JObject)
                                            .Select(x => new EditorDataItem()
                                                            {
                                                                Key = x.SelectToken(this.KeyJsonPath).ToString(),
                                                                Label = x.SelectToken(this.LabelJsonPath).ToString()
                                                            })
                                            .ToList();
                }
            }
          
            // TODO: distinct on editor data item keys

            return editorDataItems;
        }

        /// <summary>
        /// Downloads a url resource and returns it as a string. Maybe move this into a helpers class?
        /// </summary>
        /// <param name="url">URL to download the resource from</param>
        /// <returns>the string based result of the webcall</returns>
        private static string GetDataFromUrl(string url)
        {
            string data = string.Empty;

            using (WebClient client = new WebClient())
            {
                if (url.StartsWith("~/"))
                {
                    // TODO: might not be on the filesystem
                    // https://github.com/uComponents/nuPickers/issues/50

                    url = HttpContext.Current.Server.MapPath(url);
                }

                data = client.DownloadString(url);
            }

            return data;
        }
    }
}
