namespace nuPickers.Shared.RelationDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Umbraco.Core.Models;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuPickers.Shared.Editor;
    using CustomLabel;

    [PluginController("nuPickers")]
    public class RelationDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            var relationService = this.ApplicationContext.Services.RelationService;

            return relationService.GetAllRelationTypes()
                    .OrderBy(x => x.Name)
                    .Select(x => new {
                                      key = x.Alias,
                                      label = x.Name,
                                      biDirectional = x.IsBidirectional
                                     });
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            int contextId = currentId;

            var relationService = this.ApplicationContext.Services.RelationService;

            var relation = relationService.GetByRelationTypeAlias((string) data.config.dataSource.relationType).FirstOrDefault();
            if (relation != null)
            {
                var realtionType = relation.RelationType;

                IEnumerable<IRelation> editorUmbracoEntities;

                if (realtionType.IsBidirectional)
                {
                    editorUmbracoEntities = relationService.GetByRelationTypeAlias((string) data.config.dataSource.relationType)
                                            .Where(r => r.ParentId == contextId || r.ChildId == contextId);
                }
                else
                {
                    editorUmbracoEntities = relationService.GetByRelationTypeAlias((string)data.config.dataSource.relationType)
                                            .Where(r => r.ParentId == contextId);
                }

                IEnumerable<EditorDataItem> editorDataItems = relationService.GetEntitiesFromRelations(editorUmbracoEntities)
                                                              .Select(x => new EditorDataItem()
                                                              {
                                                                  Key = x.Item2.Id.ToString(),
                                                                  Label = x.Item2.Name.ToString()
                                                              })
                                                              .ToList();

                CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

                return customLabel.ProcessEditorDataItems(editorDataItems);
            }

            return null;

        }
    }
}
