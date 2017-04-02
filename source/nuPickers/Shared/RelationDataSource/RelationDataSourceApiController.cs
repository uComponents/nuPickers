
namespace nuPickers.Shared.RelationDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using umbraco.cms.businesslogic.relation;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuPickers.Shared.Editor;
    using umbraco;
    using CustomLabel;
    using Newtonsoft.Json.Linq;

    [PluginController("nuPickers")]
    public class RelationDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            return RelationType.GetAll()
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            key = x.Alias,
                            label = x.Name,
                            biDirectional = x.Dual
                        });
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            return Editor.GetEditorDataItems(
                            currentId,
                            parentId,
                            propertyAlias,
                            ((JObject)data.config.dataSource).ToObject<RelationDataSource>(),
                            (string)data.config.customLabel,
                            null);
        }
    }
}
