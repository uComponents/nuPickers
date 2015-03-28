
namespace nuPickers.Shared.JsonDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using System.Linq;

    [PluginController("nuPickers")]
    public class JsonDataSourceApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            JsonDataSource jsonDataSource = ((JObject)data.config.dataSource).ToObject<JsonDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = jsonDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker(data.ids != null ? null : (string)data.typeahead);

            // process the labels and then handle any type ahead text
            editorDataItems = typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));

            // if there are ids then filter by ids
            if (data.ids != null)
        {
                IEnumerable<string> collectionIds = data.ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => collectionIds.Contains(x.Key));
            }

            return editorDataItems;

        }
    }
}
