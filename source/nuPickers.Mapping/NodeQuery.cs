using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using umbraco;
using System.Collections;
using nuPickers.Mapping.Property;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core;
using umbraco.NodeFactory;

namespace nuPickers.Mapping
{
    /// <summary>
    /// Represents a query for mapped Umbraco nodes.  Enumerating the
    /// query gets mapped instances of every node which can be mapped to 
    /// <typeparamref name="TDestination"/>. 
    /// </summary>
    /// <typeparam name="TDestination">
    /// The type which queried nodes will be mapped to.
    /// </typeparam>
    internal class NodeQuery<TDestination> : INodeQuery<TDestination>
        where TDestination : class, new()
    {
        // Cache keys
        private const string _explicitCacheFormat = "Explicit_{0}";
        private const string _allCacheFormat = "All_{0}";

        // The paths included in the query
        private readonly List<string> _paths;
        private readonly Dictionary<string, Func<object, bool>> _propertyFilters;
        private bool _isExplicit = false;

        // The engine which will execute the query
        private readonly NodeMappingEngine _engine;

        public NodeQuery(NodeMappingEngine engine)
        {
            var destinationType = typeof(TDestination);

            if (engine == null)
            {
                throw new ArgumentNullException("engine");
            }
            else if (!engine.NodeMappers.ContainsKey(destinationType))
            {
                throw new MapNotFoundException(destinationType);
            }

            _engine = engine;
            _paths = new List<string>();
            _propertyFilters = new Dictionary<string, Func<object, bool>>();
        }

        public INodeMappingEngine Engine
        {
            get
            {
                return _engine;
            }
        }

        #region Include

        public INodeQuery<TDestination> Include(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("The path cannot be null or empty", "path");
            }

            if (!_paths.Contains(path))
            {
                _paths.Add(path);
            }

            return this;
        }

        public INodeQuery<TDestination> Include(Expression<Func<TDestination, object>> path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            string parsedPath;
            if (!path.Body.TryParsePath(out parsedPath)
                || parsedPath == null)
            {
                throw new ArgumentException(
                    string.Format("Path could not be parsed (got this far: '{0}')", parsedPath),
                    "path"
                    );
            }

            return Include(parsedPath);
        }

        public INodeQuery<TDestination> IncludeMany(string[] paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            foreach (var path in paths)
            {
                this.Include(path);
            }

            return this;
        }

        public INodeQuery<TDestination> IncludeMany(Expression<Func<TDestination, object>>[] paths)
        {
            if (paths == null)
            {
                throw new ArgumentNullException("paths");
            }

            foreach (var path in paths)
            {
                this.Include(path);
            }

            return this;
        }

        #endregion

        #region Filtering

        public INodeQuery<TDestination> Explicit()
        {
            _isExplicit = true;

            return this;
        }

        public INodeQuery<TDestination> WhereProperty<TProperty>(
            Expression<Func<TDestination, TProperty>> property,
            Func<TProperty, bool> predicate
            )
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            else if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            var destinationType = typeof(TDestination);
            var destinationInfo = property.GetPropertyInfo();

            if (!_engine.NodeMappers[destinationType].PropertyMappers
                .Any(x => x.DestinationInfo.Name == destinationInfo.Name))
            {
                throw new PropertyNotMappedException(destinationType, destinationInfo.Name);
            }

            _propertyFilters.Add(destinationInfo.Name, x => predicate((TProperty)x));

            return this;
        }

        /// <summary>
        /// Filters a collection of mapping contexts based on the predicates in <see cref="_propertyFilters"/>.
        /// </summary>
        /// <param name="contexts">The mapping contexts to filter</param>
        /// <returns>The filtered subset of <paramref name="contexts"/>.</returns>
        private IEnumerable<NodeMappingContext> FilterSet(IEnumerable<NodeMappingContext> contexts)
        {
            if (contexts == null)
            {
                throw new ArgumentNullException();
            }

            var filteredContexts = new List<NodeMappingContext>(contexts);

            foreach (var filter in _propertyFilters)
            {
                var propertyMapper = _engine.NodeMappers[typeof(TDestination)]
                    .PropertyMappers
                    .Single(x => x.DestinationInfo.Name == filter.Key);

                filteredContexts.RemoveAll(context =>
                    {
                        var property = propertyMapper.MapProperty(context);
                        return !filter.Value(property);
                    });
            }

            return filteredContexts;
        }

        #endregion

        #region Execute

        public TDestination Map(IPublishedContent node)
        {
            if (node == null
                || string.IsNullOrEmpty(node.Name))
            {
                return null;
            }

            var context = new NodeMappingContext(node, _paths.ToArray(), null);

            return (TDestination)_engine.Map(
                context,
                typeof(TDestination)
                );
        }

        [Obsolete("Use Find() instead")]
        public TDestination Single(int id)
        {
            return Find(id);
        }

        public TDestination Find(int id)
        {
            var context = new NodeMappingContext(id, _paths.ToArray(), null);

            return (TDestination)_engine.Map(
                context,
                typeof(TDestination)
                );
        }

        public TDestination Current()
        {
            return Map(UmbracoContext.Current.ContentCache.GetById(Node.getCurrentNodeId()));
        }

        /// <summary>
        /// Maps a collection of contexts to <typeparamref name="TDestination"/>.
        /// </summary>
        private IEnumerable<TDestination> Many(IEnumerable<NodeMappingContext> contexts)
        {
            if (contexts == null)
            {
                throw new ArgumentNullException();
            }

            var filteredContexts = FilterSet(contexts);

            return filteredContexts.Select(c =>
                {
                    return (TDestination)_engine.Map(c, typeof(TDestination));
                });
        }

        public IEnumerable<TDestination> Many(IEnumerable<int> ids)
        {
            if (ids == null)
            {
                throw new ArgumentNullException();
            }

            var paths = _paths.ToArray();
            var contexts = ids.Select(id => new NodeMappingContext(id, paths, null));

            return Many(contexts);
        }

        public IEnumerable<TDestination> Many(IEnumerable<IPublishedContent> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            var paths = _paths.ToArray();
            var contexts = nodes.Select(n => new NodeMappingContext(n, paths, null));

            return Many(contexts);
        }

        public IEnumerable<TProperty> SelectProperty<TProperty>(
            Expression<Func<TDestination, TProperty>> property
            )
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var sourceSet = EvaluateSourceSet();
            var propertyInfo = property.GetPropertyInfo();
            var propertyMapper = _engine.NodeMappers[typeof(TDestination)]
                .PropertyMappers
                .SingleOrDefault(x => x.DestinationInfo.Name == propertyInfo.Name);

            if (propertyMapper == null)
            {
                throw new PropertyNotMappedException(typeof(TDestination), propertyInfo.Name);
            }

            return sourceSet.Select(c => (TProperty)propertyMapper.MapProperty(c));
        }

        #endregion

        #region Enumeration

        /// <summary>
        /// Gets an enumerable representing the current source set of the query.
        /// </summary>
        private IEnumerable<NodeMappingContext> EvaluateSourceSet()
        {
            var destinationType = typeof(TDestination);
            var paths = _paths.ToArray();
            IEnumerable<NodeMappingContext> sourceSet;

            if (!_engine.NodeMappers.ContainsKey(destinationType))
            {
                throw new MapNotFoundException(destinationType);
            }

            string cacheKey = string.Format(
                _isExplicit ? _explicitCacheFormat : _allCacheFormat,
                destinationType.FullName
                );

            if (_engine.CacheProvider != null
                && _engine.CacheProvider.ContainsKey(cacheKey))
            {
                var ids = _engine.CacheProvider.Get(cacheKey) as int[];
                sourceSet = ids.Select(id => new NodeMappingContext(id, paths, null));
            }
            else
            {
                // Check whether to include derived maps
                var sourceNodeTypeAliases = _isExplicit
                    ? new[] { _engine.NodeMappers[destinationType].SourceDocumentType.Alias }
                    : _engine.GetCompatibleNodeTypeAliases(destinationType);

                var nodes = sourceNodeTypeAliases.SelectMany(alias => ApplicationContext.Current.Services.ContentService.GetContentOfContentType(
                                                                            ApplicationContext.Current.Services.ContentTypeService.GetContentType(alias).Id)
                                                                            .Select(p => UmbracoContext.Current.ContentCache.GetById(p.Id)));

                if (_engine.CacheProvider != null)
                {
                    // Cache the node IDs
                    _engine.CacheProvider.Insert(cacheKey, nodes.Select(n => n.Id).ToArray());
                }

                sourceSet = nodes.Select(n => new NodeMappingContext(n, paths, null));
            }

            return FilterSet(sourceSet);
        }

        /// <summary>
        /// Gets and enumerator of mapped instances of every node which 
        /// can be mapped to <typeparamref name="TDestination"/>. 
        /// </summary>
        public IEnumerator<TDestination> GetEnumerator()
        {
            var sourceSet = EvaluateSourceSet();
            var destinationSet = sourceSet.Select(c =>
                {
                    return (TDestination)_engine.Map(c, typeof(TDestination));
                });

            return destinationSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
