using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace nuPickers.Mapping.Property
{
    internal class CustomPropertyMapper : PropertyMapperBase
    {
        private CustomPropertyMapping _mapping;

        /// <summary>
        /// Maps a custom property not covered by the other derivations
        /// of <see cref="PropertyMapperBase"/>.
        /// </summary>
        /// <param name="mapping">
        /// The custom mapping.
        /// </param>
        /// <param name="allowCaching">
        /// Whether the property should allow its mapped value to be cached
        /// and reused.
        /// </param>
        /// <param name="requiresInclude"></param>
        /// <param name="destinationProperty"></param>
        /// <param name="nodeMapper"></param>
        public CustomPropertyMapper(
            CustomPropertyMapping mapping,
            bool requiresInclude,
            bool allowCaching,
            NodeMapper nodeMapper,
            PropertyInfo destinationProperty
            )
            :base(nodeMapper, destinationProperty)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException("mapping");
            }

            RequiresInclude = requiresInclude;
            AllowCaching = allowCaching;
            _mapping = mapping;
        }

        public override object MapProperty(NodeMappingContext context)
        {
            object value = null;

            if (AllowCaching 
                && Engine.CacheProvider != null 
                && Engine.CacheProvider.ContainsPropertyValue(context.Id, DestinationInfo.Name))
            {
                value = Engine.CacheProvider.GetPropertyValue(context.Id, DestinationInfo.Name);
            }
            else
            {
                var relativePaths = GetNextLevelPaths(context.Paths);
                value = _mapping(context.Id, relativePaths, Engine.CacheProvider);

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
