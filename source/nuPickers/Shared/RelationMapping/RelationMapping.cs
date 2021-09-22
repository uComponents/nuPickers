using Umbraco.Core.Composing;

namespace nuPickers.Shared.RelationMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    /// <summary>
    /// the core relation mapping functionality
    /// </summary>
    internal static class RelationMapping
    {
        /// <summary>
        /// Get all related id for the supplied criteria
        /// </summary>
        /// <param name="contextId">the id of the content, media or member item</param>
        /// <param name="propertyAlias">the property alias of the picker using relation mapping</param>
        /// <param name="relationTypeAlias">the alias of the relation type to use</param>
        /// <param name="relationsOnly"></param>
        /// <returns>a collection of related ids, or an empty collection</returns>
        internal static IEnumerable<int> GetRelatedIds(int contextId, string propertyAlias, string relationTypeAlias, bool relationsOnly)
        {
            if (contextId != 0) // new content / media / members don't have an id (so can't have any relations)
            {
                IRelationType relationType = Current.Services.RelationService.GetRelationTypeByAlias(relationTypeAlias);

                if (relationType != null)
                {
                    // get all relations of this type
                    IEnumerable<IRelation> relations = Current.Services.RelationService.GetAllRelationsByRelationType(relationType.Id);

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
            }

            return Enumerable.Empty<int>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="contextId">the id of the content, media or member item</param>
        /// <param name="propertyAlias">the property alias of the picker using relation mapping</param>
        /// <param name="relationTypeAlias">the alias of the relation type to use</param>
        /// <param name="relationsOnly"></param>
        /// <param name="pickedIds">the ids of all picked items that are to be related to the contextId</param>
        internal static void UpdateRelationMapping(int contextId, string propertyAlias, string relationTypeAlias, bool relationsOnly, int[] pickedIds)
        {
            IRelationType relationType = Current.Services.RelationService.GetRelationTypeByAlias(relationTypeAlias);

            if (relationType != null)
            {
                // get all relations of this type
                List<IRelation> relations = Current.Services.RelationService.GetAllRelationsByRelationType(relationType.Id).ToList();

                // construct object used to identify a relation (this is serialized into the relation comment field)
                RelationMappingComment relationMappingComment = new RelationMappingComment(contextId, propertyAlias);

                // filter down potential relations, by relation type direction
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

                // check current context is of the correct object type (as according to the relation type)
                if (Current.Services.EntityService.GetObjectType(contextId) ==  ObjectTypes.GetUmbracoObjectType(relationType.ChildObjectType.Value))
                {
                    // for each picked item
                    foreach (int pickedId in pickedIds)
                    {
                        // check picked item context if of the correct object type (as according to the relation type)
                        if (Current.Services.EntityService.GetObjectType(pickedId) ==  ObjectTypes.GetUmbracoObjectType(relationType.ParentObjectType.Value))
                        {
                            // if relation doesn't already exist (new picked item)
                            if (!relations.Exists(x => x.ParentId == pickedId))
                            {
                                // create relation
                                Relation relation = new Relation(pickedId, contextId, relationType);
                                relation.Comment = relationMappingComment.GetComment();
                                Current.Services.RelationService.Save(relation);
                            }

                            // housekeeping - remove 'the' relation from the list being processed (there should be only one)
                            relations.RemoveAll(x => x.ChildId == contextId && x.ParentId == pickedId && x.RelationTypeId == relationType.Id);
                        }
                    }
                }

                // delete relations for any items left on the list being processed
                if (relations.Any())
                {
                    foreach (IRelation relation in relations)
                    {
                        Current.Services.RelationService.Delete(relation);
                    }
                }
            }
        }
    }
}