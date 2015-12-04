
namespace nuPickers.Shared.RelationMapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    public class RelationMappingComment
    {
        public string PropertyAlias { get; private set; }

        // used to identify a specifc datatype instance
        public int PropertyTypeId { get; private set; }

        // used to identify all datatypes using a particular datatype
        // NOTE: could calculate this from the propertyTypeId
        public int DataTypeDefinitionId { get; private set; }

        internal RelationMappingComment(int contextId, string propertyAlias)
        {
            this.PropertyAlias = propertyAlias;

            IEnumerable<PropertyType> propertyTypes = Enumerable.Empty<PropertyType>();

            switch (uQuery.GetUmbracoObjectType(contextId)) // TODO: can this switch be removed ?
            {
                case uQuery.UmbracoObjectType.Document:
                    propertyTypes = ApplicationContext.Current.Services.ContentService.GetById(contextId).PropertyTypes;
                    break;

                case uQuery.UmbracoObjectType.Media:
                    propertyTypes = ApplicationContext.Current.Services.MediaService.GetById(contextId).PropertyTypes;
                    break;

                case uQuery.UmbracoObjectType.Member:
                    propertyTypes = ApplicationContext.Current.Services.MemberService.GetById(contextId).PropertyTypes;
                    break;
            }

            // NOTE: Archetype supplies a virtual propertyAlias
            PropertyType propertyType = propertyTypes.SingleOrDefault(x => x.Alias == propertyAlias);
            if (propertyType != null)
            {
                this.DataTypeDefinitionId = propertyType.DataTypeDefinitionId;
                this.PropertyTypeId = propertyType.Id;
            }
            else
            {
                this.DataTypeDefinitionId = -1;
                this.PropertyTypeId = -1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="comment">serialized string from the db comment field</param>
        internal RelationMappingComment(string comment)
        {
            if (!string.IsNullOrWhiteSpace(comment))
            {
                try
                {
                    XElement xml = XElement.Parse(comment);
                    this.PropertyAlias =  (xml.Attribute("PropertyAlias") != null) ? xml.Attribute("PropertyAlias").Value : string.Empty; // backwards compatable null check (propertyAlias a new value as of v1.1.4)
                    this.PropertyTypeId = int.Parse(xml.Attribute("PropertyTypeId").Value);
                    this.DataTypeDefinitionId = int.Parse(xml.Attribute("DataTypeDefinitionId").Value);
                }
                catch
                {
                    this.PropertyAlias = string.Empty;
                    this.PropertyTypeId = -1;
                    this.DataTypeDefinitionId = -1;
                }
            }
        }

        internal bool IsInArchetype()
        {
            return this.PropertyAlias.StartsWith("archetype-property");
        }

        internal bool MatchesArchetypeProperty(string propertyAlias)
        {
            try
            {
                return this.PropertyAlias.Split('-')[2] == propertyAlias.Split('-')[2];
            }
            catch
            {
                return false;
            }
        }

        internal string GetComment()
        {
            return "<RelationMapping PropertyAlias=\"" + this.PropertyAlias + "\" PropertyTypeId=\"" + this.PropertyTypeId.ToString() + "\" DataTypeDefinitionId=\"" + this.DataTypeDefinitionId.ToString() + "\" />";
        }

    }
}
