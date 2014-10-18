using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace nuPickers.Mapping.Property
{
    /// <summary>
    /// An immutable mapper for Umbraco Node properties to strongly typed model properties
    /// </summary>
    public abstract class PropertyMapperBase
    {
        // Stores the GetProperty<> method for casting node property values
        private readonly static MethodInfo _getNodePropertyMethod = typeof(NodeExtensions).GetMethod("GetProperty", new Type[] { typeof(IPublishedContent), typeof(String) });

        /// <summary>
        /// Whether the property mapper should allow its mapped value to be cached.
        /// </summary>
        protected bool AllowCaching { get; set; }

        protected NodeMappingEngine Engine { get; set; }
        protected NodeMapper NodeMapper { get; set; }
        protected string SourcePropertyAlias { get; set; }

        public PropertyInfo DestinationInfo { get; private set; }
        public bool RequiresInclude { get; protected set; }

        /// <param name="destinationProperty">
        /// Describes the model property being mapped to.
        /// </param>
        /// <param name="nodeMapper">
        /// The node mapper using this property mapper.
        /// </param>
        public PropertyMapperBase(
            NodeMapper nodeMapper,
            PropertyInfo destinationProperty
            )
        {
            if (nodeMapper == null)
            {
                throw new ArgumentNullException("nodeMapper");
            }
            else if (destinationProperty == null)
            {
                throw new ArgumentNullException("destinationProperty");
            }

            NodeMapper = nodeMapper;
            Engine = nodeMapper.Engine;
            DestinationInfo = destinationProperty;
        }

        /// <summary>
        /// Maps a node property.
        /// </summary>
        /// <param name="context">The context describing the mapping.</param>
        /// <returns>The strongly typed, mapped property.</returns>
        public abstract object MapProperty(NodeMappingContext context);

        /// <summary>
        /// Gets the paths relative to the property being mapped.
        /// </summary>
        protected string[] GetNextLevelPaths(string[] paths)
        {
            if (paths == null)
            {
                // No paths
                return new string[0];
            }

            var pathsForProperty = new List<string>();

            foreach (var path in paths)
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new ArgumentException("No path can be null or empty", "paths");
                }

                var segments = path.Split('.');

                if (segments.First() == DestinationInfo.Name
                    && segments.Length > 1)
                {
                    pathsForProperty.Add(string.Join(".", segments.Skip(1)));
                }
            }

            return pathsForProperty.ToArray();
        }

        /// <summary>
        /// Gets a Node's property as a certain type.
        /// </summary>
        /// <param name="sourcePropertyType">The type to get the property as (should be a system type or enum)</param>
        /// <param name="node">The node to get the property value of</param>
        public object GetSourcePropertyValue(IPublishedContent node, Type sourcePropertyType)
        {
            if (node == null || string.IsNullOrEmpty(node.Name))
            {
                throw new ArgumentException("Node cannot be null or empty", "node");
            }
            else if (sourcePropertyType == null)
            {
                throw new ArgumentNullException("sourcePropertyType");
            }
            else if (string.IsNullOrEmpty(SourcePropertyAlias))
            {
                throw new InvalidOperationException("SourcePropertyAlias cannot be null or empty");
            }

            // CSV of IDs
            // TODO: can GetProperty<> handle this type?
            if ((typeof(IEnumerable<int>)).IsAssignableFrom(sourcePropertyType))
            {
                var csv = node.GetPropertyValue<string>(SourcePropertyAlias);
                var ids = new List<int>();

                if (!string.IsNullOrWhiteSpace(csv))
                {
                    foreach (var idString in csv.Split(','))
                    {
                        // Ensure this is actually a list of node IDs
                        int id;
                        if (!int.TryParse(idString.Trim(), out id))
                        {
                            throw new RelationPropertyFormatNotSupportedException(csv, DestinationInfo.DeclaringType);
                        }

                        ids.Add(id);
                    }
                }

                return ids;
            }

            // Other type
            var getPropertyMethod = _getNodePropertyMethod.MakeGenericMethod(sourcePropertyType);
            return getPropertyMethod.Invoke(null, new object[] { node, SourcePropertyAlias });
        }

        /// <summary>
        /// Shorthand for <see cref="GetSourcePropertyValue"/>
        /// </summary>
        public T GetSourcePropertyValue<T>(IPublishedContent node)
        {
            return (T)GetSourcePropertyValue(node, typeof(T));
        }
    }

    /// <summary>
    /// The value of a node property which is being mapped by the mapping engine
    /// cannot be parsed to IDs.
    /// </summary>
    public class RelationPropertyFormatNotSupportedException : Exception
    {
        /// <param name="propertyValue">The unsupported value of the property.</param>
        /// <param name="destinationPropertyType">The destination property type.</param>
        public RelationPropertyFormatNotSupportedException(string propertyValue, Type destinationPropertyType)
            : base(string.Format(@"Could not parse '{0}' into integer IDs for destination type '{1}'.  
Trying storing your relation properties as CSV (e.g. '1234,2345,4576')", propertyValue, destinationPropertyType.FullName))
        {
        }
    }

    /// <summary>
    /// A property was not mapped, but was assumed to be.
    /// </summary>
    public class PropertyNotMappedException : Exception
    {
        /// <param name="type">The model type being mapped to</param>
        /// <param name="propertyName">The name of the property which is not mapped.</param>
        public PropertyNotMappedException(Type type, string propertyName)
            :base(string.Format(@"The property '{0}' on type '{1}' does not have a mapping.
Use the mapping expression returned by INodeMappingEngine.CreateMap() to ensure one exists.", propertyName, type))
        {
        }
    }

    internal static class TypeExtensions
    {
        /// <summary>
        /// Gets the classification for a mapped property type as far is mapping is concerted.
        /// </summary>
        public static MappedPropertyType GetMappedPropertyType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var isString = (type == typeof(string));
            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
            var isDictionary = type.FullName.StartsWith(typeof(IDictionary).FullName)
                || type.FullName.StartsWith(typeof(IDictionary<,>).FullName)
                || type.FullName.StartsWith(typeof(Dictionary<,>).FullName);
            var isClr = (type.Module.ScopeName == "CommonLanguageRuntimeLibrary");

            if (!isString && !isDictionary && isEnumerable)
            {
                return MappedPropertyType.Collection;
            }
            else if (type.Module.ScopeName == "CommonLanguageRuntimeLibrary" || type.IsEnum)
            {
                return MappedPropertyType.SystemOrEnum;
            }

            // Fallback when class is not recognised.
            return MappedPropertyType.Model;
        }
    }

    internal enum MappedPropertyType
    {
        SystemOrEnum,
        Model,
        Collection
    }
}
