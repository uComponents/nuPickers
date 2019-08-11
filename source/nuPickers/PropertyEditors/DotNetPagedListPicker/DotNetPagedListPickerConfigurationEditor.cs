using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using nuPickers.PropertyEditors.EnumPrefetchListPicker;
using Umbraco.Core;
using Umbraco.Core.PropertyEditors;

namespace nuPickers.PropertyEditors.DotNetPagedListPicker
{
    internal class DotNetPagedListPickerConfigurationEditor : ConfigurationEditor<DotNetPagedListPickerConfiguration>
    {
        public override Dictionary<string, object> ToConfigurationEditor(DotNetPagedListPickerConfiguration configuration)
        {
            var configuredItems = configuration?.Items; // ordered
            object editorItems;

            if (configuredItems == null)
            {
                editorItems = new object();
            }
            else
            {
                var d = new Dictionary<string, object>();
                editorItems = d;
                var sortOrder = 0;
                foreach (var item in configuredItems)
                    d[item.Id.ToString()] = GetItemValue(item, configuration.UseLabel, sortOrder++);
            }

            var dataSource = configuration?.DataSource;
            var saveFormat = configuration?.SaveFormat;
            var customLabel = configuration?.CustomLabel;

            var listPicker = configuration?.ListPicker;
            var useLabel = configuration?.UseLabel ?? false;
            var pagedListPicker = configuration?.PagedListPicker;
            var relationMapping = configuration?.RelationMapping;

            return new Dictionary<string, object>
            {
                { "items", editorItems },
                { "useLabel", useLabel },
                {"saveFormat",saveFormat},
                {"customLabel", customLabel},

                { "dataSource", dataSource },
                { "listPicker", listPicker },
                {"pagedListPicker",pagedListPicker},
                {"relationMapping",relationMapping}

            };
        }
        private object GetItemValue(ValueListConfiguration.ValueListItem item, bool useLabel, int sortOrder)
        {
            // in:  ValueListItem, Id = <id>, Value = <color> | { "value": "<color>", "label": "<label>" }
            //                                        (depending on useLabel)
            // out: { "value": "<color>", "label": "<label>", "sortOrder": <sortOrder> }

            var v = new ItemValue
            {
                Source = item.Value,
                Label = item.Value,
                SortOrder = sortOrder
            };

            if (item.Value.DetectIsJson())
            {
                try
                {
                    var o = JsonConvert.DeserializeObject<ItemValue>(item.Value);
                    o.SortOrder = sortOrder;
                    return o;
                }
                catch
                {
                    // parsing Json failed, don't do anything, get the value (sure?)
                    return new ItemValue { Source = item.Value, Label = item.Value, SortOrder = sortOrder };
                }
            }

            return new ItemValue { Source = item.Value, Label = item.Value, SortOrder = sortOrder };
        }
        // represents an item we are exchanging with the editor
        private class ItemValue
        {
            [JsonProperty("value")]
            public string Source { get; set; }

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("sortOrder")]
            public int SortOrder { get; set; }
        }
          public override DotNetPagedListPickerConfiguration FromConfigurationEditor(IDictionary<string, object> editorValues, DotNetPagedListPickerConfiguration configuration)
        {
            var output = new DotNetPagedListPickerConfiguration();

            if (!editorValues.TryGetValue("items", out var jjj) || !(jjj is JArray jItems))
                return output; // oops

            // handle useLabel
            if (editorValues.TryGetValue("useLabel", out var useLabelObj))
            {
                var convertBool = useLabelObj.TryConvertTo<bool>();
                if (convertBool.Success)
                    output.UseLabel = convertBool.Result;
            }
            if (editorValues.TryGetValue("dataSource", out var dataSourceObj))
            {
                var convertString = dataSourceObj.TryConvertTo<object>();
                if (convertString.Success)
                    output.DataSource = convertString.Result;
            }
            if (editorValues.TryGetValue("customLabel", out var customlabelObj))
            {
                var convertString = customlabelObj.TryConvertTo<string>();
                if (convertString.Success)
                    output.CustomLabel = convertString.Result;
            }
            if (editorValues.TryGetValue("pagedListPicker", out var pagedListPickerObj))
            {
                var convertString = pagedListPickerObj.TryConvertTo<object>();
                if (convertString.Success)
                    output.PagedListPicker = convertString.Result;
            }
            if (editorValues.TryGetValue("listPicker", out var listPickerObj))
            {
                var convertString = listPickerObj.TryConvertTo<object>();
                if (convertString.Success)
                    output.ListPicker = convertString.Result;
            }
            if (editorValues.TryGetValue("relationMapping", out var relationMappingObj))
            {
                var convertString = relationMappingObj.TryConvertTo<object>();
                if (convertString.Success)
                    output.RelationMapping = convertString.Result;
            }

            // auto-assigning our ids, get next id from existing values
            var nextId = 1;
            if (configuration?.Items != null && configuration.Items.Count > 0)
                nextId = configuration.Items.Max(x => x.Id) + 1;

            // create ValueListItem instances - ordered (items get submitted in the sorted order)
            foreach (var item in jItems.OfType<JObject>())
            {


                var value = item.Property("value")?.Value?.Value<string>();
                if (string.IsNullOrWhiteSpace(value)) continue;

                var id = item.Property("id")?.Value?.Value<int>() ?? 0;
                if (id >= nextId) nextId = id + 1;

                var label = item.Property("label")?.Value?.Value<string>();
                value = JsonConvert.SerializeObject(new { value, label });

                output.Items.Add(new ValueListConfiguration.ValueListItem { Id = id, Value = value });
            }

            // ensure ids
            foreach (var item in output.Items)
                if (item.Id == 0)
                    item.Id = nextId++;

            return output;
        }
    }
}