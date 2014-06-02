
namespace nuComponents.DataTypes.Shared.Editor
{
    using Newtonsoft.Json;

    public class EditorDataItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }
     
        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
