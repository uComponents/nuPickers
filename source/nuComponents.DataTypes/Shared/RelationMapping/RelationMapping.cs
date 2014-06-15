
namespace nuComponents.DataTypes.Shared.RelationMapping
{
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using LegacyRelation = umbraco.cms.businesslogic.relation.Relation;
    using LegacyRelationType = umbraco.cms.businesslogic.relation.RelationType;

    internal static class RelationMapping
    {
        internal static void DeleteRelations(IRelationType relationType, int contextId, string relationScopeIdentifier)
        {
            // always using reverse index
            string sql = "SELECT id FROM umbracoRelation WHERE relType = " + relationType.Id.ToString() + " AND childId = " + contextId.ToString();
            if (!string.IsNullOrWhiteSpace(relationScopeIdentifier))
            {
                sql += " AND comment = '" + relationScopeIdentifier + "'"; // TODO: swap to an xml fragment ? or use a new db column ?
            }

            using (var relations = uQuery.SqlHelper.ExecuteReader(sql))
            {
                LegacyRelation legacyRelation;
                if (relations.HasRecords)
                {
                    while(relations.Read())
                    {
                        legacyRelation = new LegacyRelation(relations.GetInt("id"));
                        legacyRelation.Delete();
                    }
                }
            }            
        }

        internal static void CreateRelation(IRelationType relationType, int contextId, int pickedId, string relationScopeIdentifier)
        {
            LegacyRelationType legacyRelationType = new LegacyRelationType(relationType.Id);

            if (uQuery.GetUmbracoObjectType(contextId) == legacyRelationType.GetChildUmbracoObjectType() &&
                uQuery.GetUmbracoObjectType(pickedId) == legacyRelationType.GetParentUmbracoObjectType())
            {
                LegacyRelation.MakeNew(pickedId, contextId, legacyRelationType, relationScopeIdentifier ?? "");
            }
        }
    }
}
