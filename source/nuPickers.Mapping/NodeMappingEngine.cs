using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Web.Caching;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Core;

namespace nuPickers.Mapping
{
    /// <summary>
    /// Handles the creation of map and the mapping of Umbraco <c>Node</c>s to strongly typed
    /// models.
    /// </summary>
    public class NodeMappingEngine : INodeMappingEngine
    {
        public Dictionary<Type, NodeMapper> NodeMappers { get; set; }

        private ICacheProvider _cacheProvider;
        private content.DocumentCacheEventHandler _documentCacheEventHandler;

        /// <summary>
        /// Instantiates a new NodeMappingEngine
        /// </summary>
        public NodeMappingEngine()
        {
            NodeMappers = new Dictionary<Type, NodeMapper>();
        }

        #region Caching

        /// <summary>
        /// Instantiates a new NodeMappingEngine using a web cache.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="cache"/> is null.
        /// </exception>
        public NodeMappingEngine(Cache cache)
            : base()
        {
            if (cache == null)
            {
                throw new ArgumentNullException("cache");
            }

            SetCacheProvider(new DefaultCacheProvider(cache));
        }

        /// <see cref="INodeMappingEngine.IsCachingEnabled" />
        public bool IsCachingEnabled
        {
            get
            {
                return _cacheProvider != null;
            }
        }

        internal ICacheProvider CacheProvider
        {
            get
            {
                return _cacheProvider;
            }
        }

        /// <see cref="INodeMappingEngine.SetCacheProvider"/>
        public void SetCacheProvider(ICacheProvider cacheProvider)
        {
            if (_cacheProvider != null)
            {
                _cacheProvider.Clear();

                content.AfterUpdateDocumentCache -= _documentCacheEventHandler;
                content.AfterClearDocumentCache -= _documentCacheEventHandler;

                _documentCacheEventHandler = null;
            }

            if (cacheProvider != null)
            {
                _documentCacheEventHandler = (sender, e) => cacheProvider.Clear();

                // TODO test this

                content.AfterUpdateDocumentCache += _documentCacheEventHandler;
                content.AfterClearDocumentCache += _documentCacheEventHandler;

                // TODO Support the below for custom mappings which use the cache, you never know
                //Media.AfterSave += _documentCacheEventHandler;
                //Media.AfterDelete += _documentCacheEventHandler;
                //CMSNode.AfterMove += _documentCacheEventHandler;
                //Member.AfterSave += _documentCacheEventHandler;
                //Member.AfterDelete += _documentCacheEventHandler;
            }

            _cacheProvider = cacheProvider;
        }

        #endregion

        #region Creating maps

        /// <summary>
        /// Creates a map to a strong type from an Umbraco document type
        /// using the unqualified class name of <typeparamref name="TDestination"/> 
        /// as the document type alias.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <returns>Further mapping configuration</returns>
        /// <exception cref="DocumentTypeNotFoundException">
        /// If the document type with an alias of <typeparamref name="TDestination"/>'s
        /// class name could not be found
        /// </exception>
        public INodeMappingExpression<TDestination> CreateMap<TDestination>()
            where TDestination : class, new()
        {
            var destinationType = typeof(TDestination);

            return this.CreateMap<TDestination>(destinationType.Name);
        }

        /// <summary>
        /// Creates a map to a strong type from an Umbraco document type.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <param name="documentTypeAlias">The document type alias to map from.</param>
        /// <returns>Further mapping configuration</returns>
        /// <exception cref="DocumentTypeNotFoundException">
        /// If the <paramref name="documentTypeAlias"/> could not be found
        /// </exception>
        public INodeMappingExpression<TDestination> CreateMap<TDestination>(string documentTypeAlias)
            where TDestination : class, new()
        {
            var destinationType = typeof(TDestination);

            // Remove current mapping if any
            if (NodeMappers.ContainsKey(destinationType))
            {
                NodeMappers.Remove(destinationType);
            }

            // Get document type
            var docType = ApplicationContext.Current.Services.ContentTypeService.GetContentType(documentTypeAlias);

            if (docType == null)
            {
                throw new DocumentTypeNotFoundException(documentTypeAlias);
            }

            var nodeMapper = new NodeMapper(this, destinationType, docType);

            NodeMappers[destinationType] = nodeMapper;

            if (_cacheProvider != null)
            {
                _cacheProvider.Clear();
            }

            return new NodeMappingExpression<TDestination>(this, nodeMapper);
        }

        #endregion

        #region Mapping

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a <paramref name="destinationType"/>, only including 
        /// specified relationship paths.
        /// </summary>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="destinationType">The type to map to.</param>
        /// <param name="paths">The relationship paths to include, or null to include
        /// all relationship paths at the top level and none below.</param>
        /// <returns>
        /// <c>null</c> if the node does not exist or does not map to <paramref name="destinationType"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">If a suitable map for <paramref name="destinationType"/> has not 
        /// been created with <see cref="CreateMap()" />.</exception>
        public object Map(IPublishedContent sourceNode, Type destinationType, string[] paths)
        {
            if (sourceNode == null
                || string.IsNullOrEmpty(sourceNode.Name))
            {
                return null;
            }

            var context = new NodeMappingContext(sourceNode, paths, null);

            return Map(context, destinationType);
        }

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a <typeparamref name="TDestination"/>, only including 
        /// specified relationship paths.
        /// </summary>
        /// <typeparam name="TDestination">
        /// The type of object that <paramref name="sourceNode"/> maps to.
        /// </typeparam>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="paths">The relationship paths to include.</param>
        /// <returns><c>null</c> if the node does not exist.</returns>
        public TDestination Map<TDestination>(IPublishedContent sourceNode, params string[] paths)
            where TDestination : class, new()
        {
            return (TDestination)Map(sourceNode, typeof(TDestination), paths);
        }

        /// <summary>
        /// Maps a node based on the <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The context which describes the node mapping.</param>
        /// <param name="destinationType">The type to map to.</param>
        /// <returns><c>null</c> if the node does not exist.</returns>
        internal object Map(NodeMappingContext context, Type destinationType)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            else if (!NodeMappers.ContainsKey(destinationType))
            {
                throw new MapNotFoundException(destinationType);
            }

            string sourceNodeTypeAlias = null;

            if (_cacheProvider != null
                && _cacheProvider.ContainsAlias(context.Id))
            {
                sourceNodeTypeAlias = _cacheProvider.GetAlias(context.Id);

                if (sourceNodeTypeAlias == null)
                {
                    // Node does not exist
                    return null;
                }
            }

            if (sourceNodeTypeAlias == null)
            {
                var node = context.GetNode();

                if (node == null || string.IsNullOrEmpty(node.Name))
                {
                    // Node doesn't exist
                    if (_cacheProvider != null)
                    {
                        _cacheProvider.InsertAlias(context.Id, null);
                    }

                    return null;
                }

                if (_cacheProvider != null)
                {
                    _cacheProvider.InsertAlias(context.Id, node.DocumentTypeAlias);
                }

                sourceNodeTypeAlias = node.DocumentTypeAlias;
            }

            var nodeMapper = GetMapper(sourceNodeTypeAlias, destinationType);

            if (nodeMapper == null)
            {
                return null;
            }

            return nodeMapper.MapNode(context);
        }

        /// <summary>
        /// Examines the engine's <see cref="NodeMappers"/> and returns node mapper
        /// which maps to the closest base class of <paramref name="type"/>.
        /// </summary>
        /// <returns>
        /// <c>null</c>  if there are no mappers which map to a base class of 
        /// <paramref name="type"/>.
        /// </returns>
        internal NodeMapper GetBaseNodeMapperForType(Type type)
        {
            var ancestorMappers = new List<NodeMapper>();

            foreach (var nodeMapper in NodeMappers)
            {
                if (nodeMapper.Value.DestinationType.IsAssignableFrom(type)
                    && type != nodeMapper.Value.DestinationType)
                {
                    ancestorMappers.Add(nodeMapper.Value);
                }
            }

            // Sort by inheritance
            ancestorMappers.Sort((x, y) =>
            {
                return x.DestinationType.IsAssignableFrom(y.DestinationType)
                    ? 1
                    : -1;
            });

            return ancestorMappers.FirstOrDefault();
        }

        /// <summary>
        /// Gets a node mapper which maps from <paramref name="sourceNodeTypeAlias"/>
        /// to <paramref name="destinationType"/> or some class derived from 
        /// <paramref name="destinationType"/>.
        /// </summary>
        /// <param name="sourceNodeTypeAlias">Node type alias to map from.</param>
        /// <param name="destinationType">The type which the mapped model must
        /// cast to.</param>
        /// <returns>The node mapper, or null if a suitable mapper could not be found</returns>
        internal NodeMapper GetMapper(string sourceNodeTypeAlias, Type destinationType)
        {
            foreach (var nodeMapper in NodeMappers)
            {
                if (destinationType.IsAssignableFrom(nodeMapper.Value.DestinationType)
                    && nodeMapper.Value.SourceDocumentType.Alias == sourceNodeTypeAlias)
                {
                    return nodeMapper.Value;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all node type aliases which can map to <paramref name="destinationType"/>.
        /// </summary>
        internal string[] GetCompatibleNodeTypeAliases(Type destinationType)
        {
            var compatibleAliases = new List<string>();

            foreach (var nodeMapper in NodeMappers)
            {
                if (destinationType.IsAssignableFrom(nodeMapper.Value.DestinationType))
                {
                    compatibleAliases.Add(nodeMapper.Value.SourceDocumentType.Alias);
                }
            }

            return compatibleAliases.Distinct().ToArray();
        }

        #endregion

        #region Querying

        /// <summary>
        /// Gets a query for nodes which map to <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <returns>A fluent configuration for the query.</returns>
        /// <exception cref="MapNotFoundException">If a suitable map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.</exception>
        public INodeQuery<TDestination> Query<TDestination>()
            where TDestination : class, new()
        {
            return new NodeQuery<TDestination>(this);
        }

        #endregion

        #region Legacy

        /// <summary>
        /// Maps an Umbraco <c>Node</c> as a strongly typed object.
        /// </summary>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="destinationType">The type to map to.</param>
        /// <param name="includedRelationships">The relationship properties to include, or <c>null</c> to 
        /// include all relationships.</param>
        /// <returns>
        /// <c>null</c> if the node does not exis or does not map to <paramref name="destinationType"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">If a suitable map for <paramref name="destinationType"/> has not 
        /// been created with <see cref="CreateMap()" />.</exception>
        [Obsolete("Use Map with paths instead")]
        public object Map(IPublishedContent sourceNode, Type destinationType, PropertyInfo[] includedRelationships)
        {
            return Map(
                sourceNode,
                destinationType,
                includedRelationships.Select(x => x.Name).ToArray()
                );
        }

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">
        /// The type of object that <paramref name="sourceNode"/> maps to.
        /// </typeparam>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="includeRelationships">Whether to include the <c>Node</c>'s relationships</param>
        /// <returns>
        /// <c>null</c> if the <c>Node</c> does not exist or does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">If a suitable map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.</exception>
        [Obsolete("Use Map with paths instead")]
        public TDestination Map<TDestination>(IPublishedContent sourceNode, bool includeRelationships)
            where TDestination : class, new()
        {
            return (TDestination)Map(
                sourceNode,
                typeof(TDestination),
                includeRelationships
                    ? null // all relationships
                    : new string[0] // no relationships
                );
        }

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a <typeparamref name="TDestination"/>, only including 
        /// specified relationships.
        /// </summary>
        /// <typeparam name="TDestination">
        /// The type of object that <paramref name="sourceNode"/> maps to.
        /// </typeparam>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="includedRelationships">The relationship properties to include.</param>
        /// <returns>
        /// <c>null</c> if the node does not exist or does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">If a suitable map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.</exception>
        [Obsolete("Use paths instead")]
        public TDestination Map<TDestination>(IPublishedContent sourceNode, params Expression<Func<TDestination, object>>[] includedRelationships)
            where TDestination : class, new()
        {
            // Get properties from included relationships expression
            var properties = includedRelationships.Select(e => (e.Body as MemberExpression).Member as PropertyInfo).ToArray();

            return (TDestination)Map(
                sourceNode,
                typeof(TDestination),
                properties.Select(x => x.Name).ToArray()
                );
        }

        #endregion
    }

    #region Exceptions

    /// <summary>
    /// The node type alias was not found in the current Umbraco instance.
    /// </summary>
    public class DocumentTypeNotFoundException : Exception
    {
        /// <param name="nodeTypeAlias">The node type alias requested and not found</param>
        public DocumentTypeNotFoundException(string nodeTypeAlias)
            : base(string.Format(@"The document type with alias '{0}' could not be found.  
Consider using the overload of CreateMap which specifies a document type alias", nodeTypeAlias))
        {
        }
    }

    /// <summary>
    /// No map exists for this engine for the destination type
    /// </summary>
    public class MapNotFoundException : Exception
    {
        /// <param name="destinationType">The requested and unfound destination type</param>
        public MapNotFoundException(Type destinationType)
            : base(string.Format(@"No map could be found for type '{0}'.  Remember
to run CreateMap for every model type you are using.", destinationType.FullName))
        {
        }
    }

    /// <summary>
    /// A collection which cannot be instiatated/populated by the mapping engine
    /// is used.
    /// </summary>
    public class CollectionTypeNotSupportedException : Exception
    {
        /// <param name="type">The unsupported collection type.</param>
        public CollectionTypeNotSupportedException(Type type)
            : base(string.Format(
            @"Could not map to collection of type '{0}'.  
The property type must be assignable from IEnumerable<{1}>
or have a constructor which takes a single argument of IEnumerable<{1}>
(such as a List<{1}>).",
            type.FullName,
            type.Name))
        {
        }
    }

    #endregion
}
