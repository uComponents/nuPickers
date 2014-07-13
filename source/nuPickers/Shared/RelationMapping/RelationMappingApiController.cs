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
    using LegacyRelation = umbraco.cms.businesslogic.relation.Relation;
    using LegacyRelationType = umbraco.cms.businesslogic.relation.RelationType;

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
                IEnumerable<IRelation> relations = ApplicationContext.Services.RelationService.GetAllRelationsByRelationType(relationType.Id);

                // construct object used to identify a relation (this is serialized into the relation comment field)
                RelationMappingComment relationMappingComment = new RelationMappingComment(contextId, propertyAlias);

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

                // clear any existing relations
                foreach (IRelation relation in relations)
                {
                    ApplicationContext.Services.RelationService.Delete(relation);
                }

                // create new relations
                LegacyRelationType legacyRelationType = new LegacyRelationType(relationType.Id);

                // ensure context type is valid
                if (uQuery.GetUmbracoObjectType(contextId) == legacyRelationType.GetChildUmbracoObjectType())
                {
                    foreach (int pickedId in ((JArray)data).Select(x => x.Value<int>()))
                    {
                        // ensure picked type is valid
                        if (uQuery.GetUmbracoObjectType(pickedId) == legacyRelationType.GetParentUmbracoObjectType())
                        {
                            LegacyRelation.MakeNew(pickedId, contextId, legacyRelationType, relationMappingComment.GetComment());
                        }
                    }
                }
            }
        }
    }
}
