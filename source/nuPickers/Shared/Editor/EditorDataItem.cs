namespace nuPickers.Shared.Editor
{
    using Newtonsoft.Json;

    /// <summary>
    /// POCO model representing an item that a picker can pick (the key and a label)
    /// </summary>
    public class EditorDataItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}