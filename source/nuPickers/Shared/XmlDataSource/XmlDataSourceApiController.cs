namespace nuPickers.Shared.XmlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class XmlDataSourceApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            return Editor.GetEditorDataItems(
                                currentId, 
                                parentId, 
                                propertyAlias, 
                                ((JObject)data.config.dataSource).ToObject<XmlDataSource>(), 
                                (string)data.config.customLabel, 
                                (string)data.typeahead);
        }
    }
}