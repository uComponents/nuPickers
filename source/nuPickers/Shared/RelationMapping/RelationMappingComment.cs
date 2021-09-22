using Umbraco.Core.Composing;

namespace nuPickers.Shared.RelationMapping
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Models;

    /// <summary>
    /// Represents the data stored in the comment field of a relation
    /// </summary>
    public class RelationMappingComment
    {
        /// <summary>
        /// The property alias of the picker using relations
        /// </summary>
        public string PropertyAlias { get; private set; }

        /// <summary>
        /// Used to identify a specific dataType instance
        /// </summary>
        public int PropertyTypeId { get; private set; }

        /// <summary>
        /// Used to identify a dataType
        /// </summary>
        public int DataTypeDefinitionId { get; private set; }

        /// <summary>
        /// Initialize a new instance of <see cref="RelationMappingComment"/>
        /// </summary>
        /// <param name="contextId">the id of the content / media or member with the picker</param>
        /// <param name="propertyAlias">the property alias of the picker</param>
        internal RelationMappingComment(int contextId, string propertyAlias)
        {
            PropertyType propertyType = null;

            // is there a better way of getting the property types for an id without having to check content / media / members independently ?
            var content = Current.Services.ContentService.GetById(contextId);

            if (content != null)
            {
                propertyType = content.Properties.SingleOrDefault(x => x.Alias == propertyAlias).PropertyType;
            }
            else
            {
                var media = Current.Services.MediaService.GetById(contextId);

                if (media != null)
                {
                    propertyType = media.Properties.SingleOrDefault(x => x.Alias == propertyAlias).PropertyType;
                }
                else
                {
                    var member = Current.Services.MemberService.GetById(contextId);

                    if (member != null)
                    {
                        propertyType = member.Properties.SingleOrDefault(x => x.Alias == propertyAlias).PropertyType;
                    }
                }
            }

            if (propertyType != null)
            {
                this.PropertyAlias = propertyAlias;
                this.PropertyTypeId = propertyType.Id;
                this.DataTypeDefinitionId = propertyType.DataTypeId;
            }
            else
            {
                throw new Exception(string.Format("Unable to find property type for ContextId: {0}, PropertyAlias: {1}", contextId.ToString(), propertyAlias));
            }
        }

        /// <summary>
        /// Initialize a new instance of <see cref="RelationMappingComment"/>
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

        /// <summary>
        /// Helper to determine if the property is inside an Archetype
        /// </summary>
        /// <returns>flag to indicate if property is inside an Archetype</returns>
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

        /// <summary>
        /// Get the serialized XML comment
        /// </summary>
        /// <returns>String XML fragment</returns>
        internal string GetComment()
        {
            return "<RelationMapping PropertyAlias=\"" + this.PropertyAlias + "\" PropertyTypeId=\"" + this.PropertyTypeId.ToString() + "\" DataTypeDefinitionId=\"" + this.DataTypeDefinitionId.ToString() + "\" />";
        }

    }
}