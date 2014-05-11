
namespace nuComponents.DataTypes.Shared.Picker
{
    using Newtonsoft.Json;

    public class PickerEditorOption
    {
        [JsonProperty("key")]
        public string Key { get; set; }
     
        [JsonProperty("label")]
        public string Label { get; set; }
    }
}
