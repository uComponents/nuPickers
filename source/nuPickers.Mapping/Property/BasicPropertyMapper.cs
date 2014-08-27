using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using umbraco;
using nuPickers.Mapping;
using System.Linq.Expressions;

namespace nuPickers.Mapping.Property
{
    internal class BasicPropertyMapper : PropertyMapperBase
    {
        private Func<object, object> _mapping;
        private Type _sourcePropertyType;

        /// <summary>
        /// Maps a basic property value.
        /// </summary>
        /// <param name="mapping">
        /// The mapping for the property value.  Cannot be null.
        /// </param>
        /// <param name="sourcePropertyType">
        /// The type of the first parameter being supplied to <paramref name="mapping"/>.
        /// Cannot be <c>null</c>.
        /// </param>
        /// <param name="sourcePropertyAlias">
        /// The alias of the node property to map from.  Required.
        /// </param>
        /// <param name="nodeMapper"></param>
        /// <param name="destinationProperty"></param>
        public BasicPropertyMapper(
            Func<object, object> mapping,
            Type sourcePropertyType,
            NodeMapper nodeMapper,
            PropertyInfo destinationProperty,
            string sourcePropertyAlias
            )
            :base(nodeMapper, destinationProperty)
        {
            if (sourcePropertyType == null && mapping != null)
            {
                throw new ArgumentNullException("sourcePropertyType", "Source property type must be specified when using a mapping");
            }

            if (sourcePropertyAlias == null)
            {
                sourcePropertyAlias = NodeMapper.GetPropertyAlias(destinationProperty);

                if (sourcePropertyAlias == null)
                {
                    throw new PropertyAliasNotFoundException(sourcePropertyType, destinationProperty, sourcePropertyAlias);
                }
            }

            SourcePropertyAlias = sourcePropertyAlias;
            RequiresInclude = false;
            AllowCaching = true;
            _mapping = mapping;
            _sourcePropertyType = sourcePropertyType;
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

                if (_mapping == null)
                {
                    // Map straight to property
                    value = GetSourcePropertyValue(node, DestinationInfo.PropertyType);
                }
                else
                {
                    // Custom mapping
                    var sourceValue = GetSourcePropertyValue(node, _sourcePropertyType);
                    value = _mapping(sourceValue);
                }

                if (AllowCaching
                    && Engine.CacheProvider != null)
                {
                    Engine.CacheProvider.InsertPropertyValue(context.Id, DestinationInfo.Name, value);
                }
            }

            return value;
        }
    }
}
