
namespace nuComponents.DataTypes.Shared.RelationTypeMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using LegacyRelation = umbraco.cms.businesslogic.relation.Relation;
    using LegacyRelationType = umbraco.cms.businesslogic.relation.RelationType;

    public class RelationTypeMappingEventHandler : ApplicationEventHandler
    {
        public RelationTypeMappingEventHandler()
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

                    if (this.SupportsRelationTypeMapping(propertyType))
                    {
                        LegacyRelationType relationType = LegacyRelationType.GetByAlias(this.GetRelationTypeAlias(propertyType));
                        if (relationType != null)
                        {
                            DeleteRelations(relationType, savedEntity.Id, true, string.Empty);

                            if (property.Value != null)
                            {
                                IEnumerable<int> pickedIds = ((string)property.Value).Split(',').Select(int.Parse);

                                foreach (int pickedId in pickedIds)
                                {
                                    CreateRelation(relationType, savedEntity.Id, pickedId, true, string.Empty);
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

                    if (this.SupportsRelationTypeMapping(propertyType))
                    {
                        string relationTypeAlias = this.GetRelationTypeAlias(propertyType);
                        //string[] keys = ((string)property.Value).Split(',');

                        LegacyRelationType relationType = LegacyRelationType.GetByAlias(relationTypeAlias);
                        if (relationType != null)
                        {                            
                            // clean out any relations refering to a deleted item
                            DeleteRelations(relationType, deletedEntity.Id, true, string.Empty);
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

        private bool SupportsRelationTypeMapping(PropertyType propertyType)
        {
            if (propertyType != null && propertyType.Alias == "xmlDropDownPicker")
            {
                return true;
            }

            return false;
        }

        private string GetRelationTypeAlias(PropertyType propertyType)
        {
            return ApplicationContext
                        .Current
                        .Services
                        .DataTypeService
                        .GetPreValuesCollectionByDataTypeId(propertyType.DataTypeDefinitionId)
                        .PreValuesAsDictionary.Single(x => x.Key == "relationTypeMapping")
                        .Value
                        .Value;
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
                getRelationsSql += "childId = " + contextId.ToString();
            }
            if (relationType.Dual) // need to return relations where content node id is used on both sides
            {
                getRelationsSql += " OR ";
            }
            if (!reverseIndexing || relationType.Dual)
            {
                getRelationsSql += "parentId = " + contextId.ToString();
            }

            getRelationsSql += " AND comment = '" + instanceIdentifier + "'";

            using (var relations = uQuery.SqlHelper.ExecuteReader(getRelationsSql))
            {
                //clear data
                LegacyRelation relation;
                if (relations.HasRecords)
                {
                    while (relations.Read())
                    {
                        relation = new LegacyRelation(relations.GetInt("id"));

                        // TODO: [HR] check to see if an instance identifier is used
                        relation.Delete();
                    }
                }
            }
        }

        private static void CreateRelation(LegacyRelationType relationType, int contextId, int pickedId, bool reverseIndexing, string instanceIdentifier)
        {
            if (reverseIndexing)
            {
                LegacyRelation.MakeNew(pickedId, contextId, relationType, instanceIdentifier);
            }
            else
            {
                LegacyRelation.MakeNew(contextId, pickedId, relationType, instanceIdentifier);
            }
        }

    }
}