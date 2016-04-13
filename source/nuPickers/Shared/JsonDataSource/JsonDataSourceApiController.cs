
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

    [PluginController("nuPickers")]
    public class JsonDataSourceApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            int contextId = currentId;

            JsonDataSource jsonDataSource = ((JObject)data.config.dataSource).ToObject<JsonDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = jsonDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);

            // process the labels and then handle any type ahead text
            return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
        }
    }
}
