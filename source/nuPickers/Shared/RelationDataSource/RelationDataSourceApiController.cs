
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
            int contextId = currentId;

            IEnumerable<EditorDataItem> editorDataItems = RelationType.GetByAlias((string)data.config.dataSource.relationType)
                                                                                .GetRelations(contextId)
                                                                                .Select(x => new EditorDataItem()
                                                                                {
                                                                                    Key = x.Child.Id.ToString(),
                                                                                    Label = x.Child.Text
                                                                                })
                                                                                .ToList();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            return customLabel.ProcessEditorDataItems(editorDataItems);
        }
    }
}
