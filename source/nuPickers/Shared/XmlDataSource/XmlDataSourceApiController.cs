
namespace nuPickers.Shared.XmlDataSource
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
    public class XmlDataSourceApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            XmlDataSource xmlDataSource = ((JObject)data.config.dataSource).ToObject<XmlDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = xmlDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);

            // process the labels and then handle any type ahead text
            return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> getEditorDataItemsByIds([FromUri] int contextId, [FromUri] string propertyAlias, [FromUri] string ids, [FromBody] dynamic data)
        {
            XmlDataSource xmlDataSource = ((JObject)data.config.dataSource).ToObject<XmlDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = xmlDataSource.GetEditorDataItems(contextId);

            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => ids.Contains(x.Key));
            }

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker(null);

            // process the labels and then handle any type ahead text
            return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
        }
    }
}
