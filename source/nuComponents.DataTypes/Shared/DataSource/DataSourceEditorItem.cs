
namespace nuComponents.DataTypes.Shared.Picker
{
    using Newtonsoft.Json;

    public class DataSourceEditorItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
