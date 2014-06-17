
namespace nuComponents.DataTypes.Shared.JsonDataSource
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;

    public class JsonDataSource
    {
        public string JsonData { get; set; }

        public string Url { get; set; }

        public string OptionsJsonPath { get; set; }
        
        public string KeyJsonPath { get; set; }
        
        public string LabelJsonPath { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            JObject jsonDoc;
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            switch (this.JsonData)
            {
               case "url":
                    jsonDoc = JObject.Parse(GetContents(this.Url));
                    break;

                default:
                    jsonDoc = null;
                    break;
            }

            if (jsonDoc != null)
            { 
                //Do the lookups
                var optionsIterator = jsonDoc.SelectTokens(OptionsJsonPath).GetEnumerator();

                List<string> keys = new List<string>(); // used to keep track of keys, so that duplicates aren't added

                string key;
                string label;

                while (optionsIterator.MoveNext())
                {
                    key = optionsIterator.Current.SelectToken(this.KeyJsonPath).Value<string>();

                        // only add item if it has a unique key - failsafe
                        if (!string.IsNullOrWhiteSpace(key) && !keys.Any(x => x == key))
                        {
                            // TODO: ensure key doens't contain any commas (keys are converted saved as csv)
                            keys.Add(key); // add key so that it's not reused

                            // set default markup to use the configured label XPath
                            label = optionsIterator.Current.SelectToken(this.LabelJsonPath).Value<string>();
                            
                            editorDataItems.Add(new EditorDataItem()
                            {
                                Key = key,
                                Label = label
                            });
                        }
                    }
                }
            

            return editorDataItems;
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
