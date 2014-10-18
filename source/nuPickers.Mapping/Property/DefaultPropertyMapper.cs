using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;
using Umbraco.Core.Models;

namespace nuPickers.Mapping.Property
{
    internal class DefaultPropertyMapper : PropertyMapperBase
    {
        private Func<object, object> _mapping;
        private PropertyInfo _nodeProperty;

        public DefaultPropertyMapper(
            NodeMapper nodeMapper,
            PropertyInfo destinationProperty,
            PropertyInfo nodeProperty,
            Func<object, object> mapping
            )
            :base(nodeMapper, destinationProperty)
        {
            if (nodeProperty == null)
            {
                throw new ArgumentNullException("nodeProperty");
            }

            if (!destinationProperty.PropertyType.IsAssignableFrom(nodeProperty.PropertyType))
            {
                throw new DefaultPropertyTypeException(nodeMapper.DestinationType, destinationProperty, nodeProperty);
            }

            RequiresInclude = false;
            AllowCaching = true;
            _mapping = mapping;
            _nodeProperty = nodeProperty;
        }

        public override object MapProperty(NodeMappingContext context)
        {
            object value = null;

            // Check cache
            if (AllowCaching
                && Engine.CacheProvider != null 
                && Engine.CacheProvider.ContainsPropertyValue(context.Id, DestinationInfo.Name))
            {
                value = Engine.CacheProvider.GetPropertyValue(context.Id, DestinationInfo.Name);
            }
            else
            {
                var node = context.GetNode();

                if (node == null || string.IsNullOrEmpty(node.Name))
                {
                    throw new InvalidOperationException("Node cannot be null or empty");
                }

                value = _nodeProperty.GetValue(node, null);

                if (_mapping != null)
                {
                    value = _mapping(value);
                }

                if (AllowCaching
                    && Engine.CacheProvider != null)
                {
                    Engine.CacheProvider.InsertPropertyValue(context.Id, DestinationInfo.Name, value);
                }
            }

            return value;
        }

        /// <summary>
        /// Given a model property name, returns the node property info if
        /// there is a match again the properties of <c>Node</c>.
        /// 
        /// Checks whether the node's property is assignable to the destination
        /// property.
        /// </summary>
        /// <param name="destinationProperty">The model property.</param>
        /// <returns>The property or <c>null</c> if no match was found.</returns>
        public static PropertyInfo GetDefaultMappingForName(PropertyInfo destinationProperty)
        {
            Expression<Func<IPublishedContent, object>> propertyExpression = null;

            switch (destinationProperty.Name.ToLowerInvariant())
            {
                case "createdate":
                    propertyExpression = n => n.CreateDate;
                    break;
                case "creatorid":
                    propertyExpression = n => n.CreatorId;
                    break;
                case "creatorname":
                    propertyExpression = n => n.CreatorName;
                    break;
                case "id":
                    propertyExpression = n => n.Id;
                    break;
                case "level":
                    propertyExpression = n => n.Level;
                    break;
                case "name":
                    propertyExpression = n => n.Name;
                    break;
                case "url":
                case "niceurl":
                    propertyExpression = n => n.Url;
                    break;
                case "nodetypealias":
                case "documenttypealias":
                    propertyExpression = n => n.DocumentTypeAlias;
                    break;
                case "path":
                    propertyExpression = n => n.Path;
                    break;
                case "sortorder":
                    propertyExpression = n => n.SortOrder;
                    break;
                case "template":
                    propertyExpression = n => n.TemplateId;
                    break;
                case "updatedate":
                    propertyExpression = n => n.UpdateDate;
                    break;
                case "urlname":
                    propertyExpression = n => n.UrlName;
                    break;
                case "version":
                    propertyExpression = n => n.Version;
                    break;
                case "writerid":
                    propertyExpression = n => n.WriterId;
                    break;
                case "writername":
                    propertyExpression = n => n.WriterName;
                    break;
            }

            if (propertyExpression != null)
            {
                var nodeProperty = propertyExpression.GetPropertyInfo();

                if (destinationProperty.PropertyType.IsAssignableFrom(nodeProperty.PropertyType))
                {
                    return nodeProperty;
                }
            }

            return null;
        }
    }
}
