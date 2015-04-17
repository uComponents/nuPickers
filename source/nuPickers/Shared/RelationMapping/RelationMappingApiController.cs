namespace nuPickers.Shared.RelationMapping
{
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using umbraco;
    using Umbraco.Core.Models;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class RelationMappingApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            return ApplicationContext.Services.RelationService.GetAllRelationTypes()
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            key = x.Alias,
                            label = x.Name,
                            bidirectional = x.IsBidirectional
                        });
        }

        [HttpGet]
        public IEnumerable<int> GetRelatedIds([FromUri] int contextId, [FromUri] string propertyAlias, [FromUri] string relationTypeAlias, [FromUri] bool relationsOnly)
        {
            IRelationType relationType = ApplicationContext.Services.RelationService.GetRelationTypeByAlias(relationTypeAlias);

            if (relationType != null)
            {
                // get all relations of this type
                IEnumerable<IRelation> relations = ApplicationContext.Services.RelationService.GetAllRelationsByRelationType(relationType.Id);

                // construct object used to identify a relation (this is serialized into the relation comment field)
                RelationMappingComment relationMappingComment = new RelationMappingComment(contextId, propertyAlias);

                // filter down potential relations, by relation type direction
                if (relationType.IsBidirectional && relationsOnly)
                {
                    relations = relations.Where(x => x.ChildId == contextId || x.ParentId == contextId);
                    relations = relations.Where(x => new RelationMappingComment(x.Comment).DataTypeDefinitionId == relationMappingComment.DataTypeDefinitionId);
                }
                else
                {
                    relations = relations.Where(x => x.ChildId == contextId);
                    relations = relations.Where(x => new RelationMappingComment(x.Comment).PropertyTypeId == relationMappingComment.PropertyTypeId);

                    if (relationMappingComment.IsInArchetype())
                    {
                        relations = relations.Where(x => new RelationMappingComment(x.Comment).MatchesArchetypeProperty(relationMappingComment.PropertyAlias));
                    }
                }

                return relations.Select(x => (x.ParentId != contextId) ? x.ParentId : x.ChildId);
            }

            return null;
        }

        [HttpPost]
        public void UpdateRelationMapping([FromUri] int contextId, [FromUri] string propertyAlias, [FromUri] string relationTypeAlias, [FromUri] bool relationsOnly, [FromBody] dynamic data)
        {
            IRelationType relationType = ApplicationContext.Services.RelationService.GetRelationTypeByAlias(relationTypeAlias);

            if (relationType != null)
            {
                // WARNING: Duplicate code
                // get all relations of this type
                List<IRelation> relations = ApplicationContext.Services.RelationService.GetAllRelationsByRelationType(relationType.Id).ToList();

                // construct object used to identify a relation (this is serialized into the relation comment field)
                RelationMappingComment relationMappingComment = new RelationMappingComment(contextId, propertyAlias);

                if (relationType.IsBidirectional && relationsOnly)
                {
                    relations = relations.Where(x => x.ChildId == contextId || x.ParentId == contextId).ToList();
                    relations = relations.Where(x => new RelationMappingComment(x.Comment).DataTypeDefinitionId == relationMappingComment.DataTypeDefinitionId).ToList();
                }
                else
                {
                    relations = relations.Where(x => x.ChildId == contextId).ToList();
                    relations = relations.Where(x => new RelationMappingComment(x.Comment).PropertyTypeId == relationMappingComment.PropertyTypeId).ToList();

                    if (relationMappingComment.IsInArchetype())
                    {
                        relations = relations.Where(x => new RelationMappingComment(x.Comment).MatchesArchetypeProperty(relationMappingComment.PropertyAlias)).ToList();
                    }
                }

                if (ApplicationContext.Services.EntityService.GetObjectType(contextId) == UmbracoObjectTypesExtensions.GetUmbracoObjectType(relationType.ChildObjectType))
                {
                    foreach (int pickedId in ((JArray)data).Select(x => x.Value<int>()))
                    {
                        // ensure picked type is valid
                        if (ApplicationContext.Services.EntityService.GetObjectType(pickedId) == UmbracoObjectTypesExtensions.GetUmbracoObjectType(relationType.ParentObjectType))
                        {

                            if (!relations.Exists(x => x.ParentId == pickedId))
                            {
                                Relation r = new Relation(pickedId, contextId, relationType);
                                r.Comment = relationMappingComment.GetComment();
                                ApplicationContext.Services.RelationService.Save(r);
                            }
                            // housekeeping
                            relations.RemoveAll(x => x.ChildId == contextId && x.ParentId == pickedId && x.RelationTypeId == relationType.Id);
                        }
                    }
                }

                // clean up remaining relations
                if (relations.Any())
                {
                    foreach (IRelation relation in relations)
                    {
                        ApplicationContext.Services.RelationService.Delete(relation);
                    }
                }
            }
        }
    }
}
