namespace nuPickers.Shared.DataSource
{
    using Newtonsoft.Json;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;

    /// <summary>
    /// Model for the api controller response
    /// </summary>
    public class GetEditorDataItemsResponse
    {
        /// <summary>
        /// collection of editor data items
        /// </summary>
        [JsonProperty("editorDataItems")]
        public IEnumerable<EditorDataItem> EditorDataItems { get; internal set; }

        /// <summary>
        /// total of available editor data items (not all may be in the collection, for example a paged picker will request a subset)
        /// </summary>
        [JsonProperty("count")]
        public int? Count { get; internal set; }
    }
}