
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

            var relationService = this.ApplicationContext.Services.RelationService;

            IEnumerable<EditorDataItem> editorDataItems = relationService.GetEntitiesFromRelations(
                                                            relationService.GetByRelationTypeAlias((string)data.config.dataSource.relationType)
                                                            .Where(r => r.ParentId == contextId))
                                                            .Select(x => new EditorDataItem()
                                                            {
                                                                Key = x.Item2.Id.ToString(),
                                                                Label = x.Item2.Name.ToString()
                                                            })
                                                            .ToList();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            return customLabel.ProcessEditorDataItems(editorDataItems);
        }
    }
}
