namespace nuPickers.Shared.JsonDataSource
{
    using Newtonsoft.Json.Linq;
    using Editor;
    using System.Collections.Generic;
    using System.Linq;
	using Newtonsoft.Json;
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
		    List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

		    var dataFromUrl = Helper.GetDataFromUrl(this.Url);

		    if (!string.IsNullOrEmpty(dataFromUrl))
		    {

			    JToken jToken = JToken.Parse(dataFromUrl);

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
					    var dataItems = new List<EditorDataItem>();
					    
						IEnumerable<JToken> selectTokens = null;
					    
						try // we cannot evaluate JSONPath validity so we'll wrap in a Try Catch
					    {
							selectTokens = jToken.SelectTokens(this.JsonPath);
					    }
					    catch (JsonException jEx)
					    {
							LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "Check JSONPath: " + this.JsonPath, jEx);
					    }

					    if (selectTokens != null)
					    {
						    IEnumerable<JToken> tokens = selectTokens.Where(x => x != null);

						    foreach (JToken token in tokens)
						    {
							    if (token.HasValues)   // prevents NullReferenceExceptions
							    {
								    var item = new EditorDataItem();

								    var keySelect = token.SelectToken(this.KeyJsonPath);
								    var tokenSelect = token.SelectToken(this.LabelJsonPath);

								    if (keySelect != null && tokenSelect != null) // only add value if we match a Key AND a Label
								    {
									    item.Key = keySelect.ToString();
									    item.Label = tokenSelect.ToString();

									    dataItems.Add(item);
								    }
							    }
						    }
					    }

					    editorDataItems = dataItems.ToList();
				    }
			    }
			    // TODO: distinct on editor data item keys
		    }

		    return editorDataItems;
        }
    }
}
