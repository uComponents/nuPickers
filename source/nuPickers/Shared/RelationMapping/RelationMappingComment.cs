
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

    [Serializable()]
    public class RelationMappingComment
    {
        // used to identify all datatypes using a particular datatype
        [XmlAttribute("DataTypeDefinitionId")] // NOTE: could calculate this from the propertyTypeId
        public int DataTypeDefinitionId { get; private set; }

        // used to identify a specifc datatype instance
        [XmlAttribute("PropertyTypeId")]
        public int PropertyTypeId { get; private set; }

        internal RelationMappingComment(int contextId, string propertyAlias) 
        {
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

            PropertyType propertyType = propertyTypes.Single(x => x.Alias == propertyAlias);
            this.DataTypeDefinitionId = propertyType.DataTypeDefinitionId;
            this.PropertyTypeId = propertyType.Id;            
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
                    this.DataTypeDefinitionId = int.Parse(xml.Attribute("DataTypeDefinitionId").Value);
                    this.PropertyTypeId = int.Parse(xml.Attribute("PropertyTypeId").Value);
                }
                catch
                {
                    this.DataTypeDefinitionId = -1;
                    this.PropertyTypeId = -1;
                }

            }
        }

        internal string GetComment()
        {
            return "<RelationMapping DataTypeDefinitionId=\"" + this.DataTypeDefinitionId.ToString() + "\" PropertyTypeId=\"" + this.PropertyTypeId.ToString() + "\" />";
        }

    }
}
