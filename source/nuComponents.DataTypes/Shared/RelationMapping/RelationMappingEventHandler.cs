
namespace nuComponents.DataTypes.Shared.RelationMapping
{
    using nuComponents.DataTypes.Shared.SaveFormat;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using LegacyRelation = umbraco.cms.businesslogic.relation.Relation;
    using LegacyRelationType = umbraco.cms.businesslogic.relation.RelationType;

    public class RelationMappingEventHandler : ApplicationEventHandler
    {
        public RelationMappingEventHandler()
        {
            ContentService.Saved += (sender, e) => { this.SaveEvent(e.SavedEntities); };
            ContentService.Deleting += (sender, e) => { this.DeleteEvent(e.DeletedEntities); };
            MediaService.Saved += (sender, e) => { this.SaveEvent(e.SavedEntities); };
            MediaService.Deleting += (sender, e) => { this.DeleteEvent(e.DeletedEntities); };
        }

        private void SaveEvent(IEnumerable<IContentBase> savedEntities)
        {
            foreach (IContentBase savedEntity in savedEntities)
            {
                foreach (Property property in savedEntity.Properties)
                {
                    PropertyType propertyType = this.GetPropertyType(property);

                    if (this.SupportsRelationMapping(propertyType))
                    {
                        LegacyRelationType relationType = LegacyRelationType.GetByAlias(this.GetRelationTypeAlias(propertyType));
                        if (relationType != null)
                        {
                            DeleteRelations(relationType, savedEntity.Id, true, GetInstanceIdentifier(propertyType));

                            if (property.Value != null)
                            {
                                foreach(string key in SaveFormat.GetSavedKeys((string)property.Value))
                                {
                                    int pickedId;
                                    if (int.TryParse(key, out pickedId))
                                    {
                                        CreateRelation(relationType, savedEntity.Id, pickedId, true, GetInstanceIdentifier(propertyType));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void DeleteEvent(IEnumerable<IContentBase> deletedEntities)
        {
            foreach (IContentBase deletedEntity in deletedEntities)
            {
                foreach (Property property in deletedEntity.Properties)
                {
                    PropertyType propertyType = this.GetPropertyType(property);

                    if (this.SupportsRelationMapping(propertyType))
                    {
                        string relationTypeAlias = this.GetRelationTypeAlias(propertyType);

                        LegacyRelationType relationType = LegacyRelationType.GetByAlias(relationTypeAlias);
                        if (relationType != null)
                        {                            
                            // clean out any relations refering to a deleted item
                            DeleteRelations(relationType, deletedEntity.Id, true, GetInstanceIdentifier(propertyType));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// WARNING: this is a helper method to get at an internal Umbraco property
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private PropertyType GetPropertyType(Property property)
        {
            PropertyInfo propertyInfo = typeof(Property).GetProperty("PropertyType", BindingFlags.Instance | BindingFlags.NonPublic);

            if (propertyInfo != null)
            {
                return (PropertyType)propertyInfo.GetValue(property);
            }

            return null;
        }

        /// <summary>
        /// A PropertyType supports relation type mapping if its config has a 'relationMapping' key
        /// </summary>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        private bool SupportsRelationMapping(PropertyType propertyType)
        {            
            try
            {
                return ApplicationContext
                        .Current
                        .Services
                        .DataTypeService
                        .GetPreValuesCollectionByDataTypeId(propertyType.DataTypeDefinitionId)
                        .PreValuesAsDictionary.Any(x => x.Key == "relationMapping");
            }
            catch
            {
                return false;
            }
        }

        private string GetRelationTypeAlias(PropertyType propertyType)
        {
            return ApplicationContext
                        .Current
                        .Services
                        .DataTypeService
                        .GetPreValuesCollectionByDataTypeId(propertyType.DataTypeDefinitionId)
                        .PreValuesAsDictionary.Single(x => x.Key == "relationMapping")
                        .Value
                        .Value;
        }

        private static string GetInstanceIdentifier(PropertyType propertyType)
        {
            return "[{\"PropertyTypeId\":" + propertyType.Id.ToString() + "}]";
        }

        /// <summary>
        /// Delete all relations using the content node for a given RelationType
        /// </summary>
        private static void DeleteRelations(LegacyRelationType relationType, int contextId, bool reverseIndexing, string instanceIdentifier)
        {
            //if relationType is bi-directional or a reverse index then we can't get at the relations via the API, so using SQL
            string getRelationsSql = "SELECT id FROM umbracoRelation WHERE relType = " + relationType.Id.ToString() + " AND ";

            if (reverseIndexing || relationType.Dual)
            {
                getRelationsSql += "(";
                getRelationsSql += "childId = " + contextId.ToString();
            }
            if (relationType.Dual) // need to return relations where context node id is used on both sides
            {
                getRelationsSql += " OR ";
            }
            if (!reverseIndexing || relationType.Dual)
            {
                getRelationsSql += "parentId = " + contextId.ToString();
            }

            getRelationsSql += ")";
            getRelationsSql += " AND comment = '" + instanceIdentifier + "'";

            using (var relations = uQuery.SqlHelper.ExecuteReader(getRelationsSql))
            {
                LegacyRelation relation;
                if (relations.HasRecords)
                {
                    while (relations.Read())
                    {
                        relation = new LegacyRelation(relations.GetInt("id"));

                        relation.Delete();
                    }
                }
            }
        }

        private static void CreateRelation(LegacyRelationType relationType, int contextId, int pickedId, bool reverseIndexing, string instanceIdentifier)
        {
            if (reverseIndexing)
            {
                if (uQuery.GetUmbracoObjectType(contextId) == relationType.GetChildUmbracoObjectType() &&
                    uQuery.GetUmbracoObjectType(pickedId) == relationType.GetParentUmbracoObjectType())
                {
                    LegacyRelation.MakeNew(pickedId, contextId, relationType, instanceIdentifier);
                }
            }
            else
            {
                if (uQuery.GetUmbracoObjectType(contextId) == relationType.GetParentUmbracoObjectType() &&
                    uQuery.GetUmbracoObjectType(pickedId) == relationType.GetChildUmbracoObjectType())
                {
                    LegacyRelation.MakeNew(contextId, pickedId, relationType, instanceIdentifier);
                }
            }
        }
    }
}