using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using umbraco;
using Umbraco.Web;

namespace nuPickers.Mapping.Property
{
    internal class SinglePropertyMapper : PropertyMapperBase
    {
        private Func<NodeMappingContext, object, int?> _mapping; // context, source property value, single relationship ID
        private Type _sourcePropertyType;

        /// <summary>
        /// Maps a single relationship.
        /// </summary>
        /// <param name="mapping">
        /// Mapping to a nullable node ID.  Takes the context and source property value
        /// as parameters.
        /// If <c>null</c>, the mapping will be deduced from the other parameters.
        /// </param>
        /// <param name="sourcePropertyType">
        /// The type of object being supplied to <paramref name="mapping"/>.
        /// Will be set to <c>int?</c> if <paramref name="mapping"/> is specified.
        /// </param>
        /// <param name="sourcePropertyAlias">
        /// The alias of the node property to map from.  If null, the closest ancestor which is 
        /// compatible with <paramref name="destinationProperty"/> will be mapped instead.
        /// </param>
        /// <param name="nodeMapper"></param>
        /// <param name="destinationProperty"></param>
        public SinglePropertyMapper(
            Func<NodeMappingContext, object, int?> mapping,
            Type sourcePropertyType,
            NodeMapper nodeMapper,
            PropertyInfo destinationProperty,
            string sourcePropertyAlias
            )
            : base(nodeMapper, destinationProperty)
        {
            if (sourcePropertyType == null && mapping != null)
            {
                // Default source property type
                sourcePropertyType = typeof(int?);
            }
            
            if (sourcePropertyAlias == null
                && mapping != null
                && !typeof(int?).IsAssignableFrom(sourcePropertyType))
            {
                throw new ArgumentException("If specifying a mapping for a single model with no property alias, the source property type must be assignable to Nullable<int>.");
            }

            SourcePropertyAlias = sourcePropertyAlias;
            RequiresInclude = true;
            AllowCaching = true;
            _mapping = mapping;
            _sourcePropertyType = sourcePropertyType;
        }

        public override object MapProperty(NodeMappingContext context)
        {
            int? id = null;

            // Get ID
            if (AllowCaching
                && Engine.CacheProvider != null
                && Engine.CacheProvider.ContainsPropertyValue(context.Id, DestinationInfo.Name))
            {
                id = Engine.CacheProvider.GetPropertyValue(context.Id, DestinationInfo.Name) as int?;
            }
            else
            {
                var node = context.GetNode();

                if (node == null || string.IsNullOrEmpty(node.Name))
                {
                    throw new InvalidOperationException("Node cannot be null or empty");
                }

                if (string.IsNullOrEmpty(SourcePropertyAlias))
                {
                    // Get closest parent
                    var aliases = Engine
                        .GetCompatibleNodeTypeAliases(DestinationInfo.PropertyType)
                        .ToArray();
                    var ancestorNode = node.Ancestors()
                        .FirstOrDefault(x => aliases.Contains(x.DocumentTypeAlias));

                    if (ancestorNode != null)
                    {
                        // Found one
                        id = ancestorNode.Id;

                        context.AddNodeToContextCache(ancestorNode);
                    }

                    if (_mapping != null)
                    {
                        id = _mapping(context, id);
                    }
                }
                else
                {
                    if (_mapping == null)
                    {
                        // Map ID from node property
                        id = GetSourcePropertyValue<int?>(node);
                    }
                    else
                    {
                        // Custom mapping
                        id = _mapping(context, GetSourcePropertyValue(node, _sourcePropertyType));
                    }
                }

                if (AllowCaching
                    && Engine.CacheProvider != null)
                {
                    Engine.CacheProvider.InsertPropertyValue(context.Id, DestinationInfo.Name, id);
                }
            }

            if (!id.HasValue)
            {
                // Not found
                return null;
            }

            // Map to model
            var childPaths = GetNextLevelPaths(context.Paths);
            var childContext = new NodeMappingContext(id.Value, childPaths, context);

            return Engine.Map(childContext, DestinationInfo.PropertyType);
        }
    }
}
