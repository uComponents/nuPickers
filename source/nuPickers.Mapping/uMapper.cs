using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using umbraco;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Core;
using umbraco.NodeFactory;

namespace nuPickers.Mapping
{
    /// <summary>
    /// Maps Umbraco <c>Node</c>s to strongly typed models.
    /// </summary>
    /// <remarks>
    /// This is a static convenience class which wraps single instance of a <c>NodeMappingEngine</c>.
    /// See http://ucomponents.org/umapper/ for usage examples and pretty pictures.
    /// </remarks>
    public static class uMapper
    {
        private static NodeMappingEngine _engine = new NodeMappingEngine();

        /// <summary>
        /// Gets the <c>INodeMappingEngine</c> being used by uMapper.
        /// </summary>
        public static INodeMappingEngine Engine
        {
            get { return _engine; }
        }

        /// <summary>
        /// Enables or disables caching, using the <c>HttpContext.Current.Cache</c>
        /// object.
        /// 
        /// Caching is disabled by default.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// If <c>HttpContext.Current</c> is not set.
        /// </exception>
        public static bool CachingEnabled
        {
            get
            {
                return _engine.IsCachingEnabled;
            }
            set
            {
                if (value)
                {
                    if (HttpContext.Current == null)
                    {
                        throw new InvalidOperationException("The current HttpContext is not available - caching cannot be enabled");
                    }

                    _engine.SetCacheProvider(new DefaultCacheProvider(HttpContext.Current.Cache));
                }
                else
                {
                    _engine.SetCacheProvider(null);
                }
            }
        }

        /// <summary>
        /// Creates a map to <typeparamref name="TDestination" /> from an Umbraco document type, 
        /// which must have the same alias as the unqualified class name of 
        /// <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <returns>Fluent configuration for the newly created mapping.</returns>
        /// <exception cref="DocumentTypeNotFoundException">If a suitable document type could not be found</exception>
        public static INodeMappingExpression<TDestination> CreateMap<TDestination>()
            where TDestination : class, new()
        {
            return _engine.CreateMap<TDestination>();
        }

        /// <summary>
        /// Creates a map to <typeparamref name="TDestination" /> from an Umbraco document type,
        /// specifying the document type alias.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <param name="documentTypeAlias">The document type alias to map from.</param>
        /// <returns>Fluent configuration for the newly created mapping.</returns>
        /// <exception cref="DocumentTypeNotFoundException">If the document type could not be found.</exception>
        public static INodeMappingExpression<TDestination> CreateMap<TDestination>(string documentTypeAlias)
            where TDestination : class, new()
        {
            return _engine.CreateMap<TDestination>(documentTypeAlias);
        }

        /// <summary>
        /// Maps an Umbraco <c>Node</c> to a new instance of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <param name="sourceNode">The <c>Node</c> to map from.</param>
        /// <param name="includeRelationships">Whether to load relationships.</param>
        /// <returns>
        /// A new instance of <typeparamref name="TDestination"/>, or <c>null</c> if 
        /// <paramref name="sourceNode"/> is <c>null</c> or does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        public static TDestination Map<TDestination>(IPublishedContent sourceNode, bool includeRelationships = true)
            where TDestination : class, new()
        {
            if (sourceNode == null || string.IsNullOrEmpty(sourceNode.Name))
            {
                return null;
            }

            var paths = includeRelationships
                ? null // all
                : new string[0]; // none

            var context = new NodeMappingContext(sourceNode, paths, null);

            return (TDestination)_engine.Map(context, typeof(TDestination));
        }

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a new instance of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type that the <c>Node</c> maps to.</typeparam>
        /// <param name="id">The ID of the <c>Node</c></param>
        /// <param name="includeRelationships">Whether to load all the <c>Node</c>'s relationships</param>
        /// <returns>
        /// <c>null</c> if the <c>Node</c> does not exist or does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        public static TDestination Find<TDestination>(int id, bool includeRelationships = true)
        {
            var paths = includeRelationships
                ? null // all
                : new string[0]; // none

            var context = new NodeMappingContext(id, paths, null);

            return (TDestination)_engine.Map(context, typeof(TDestination));
        }

        /// <summary>
        /// Gets the current Umbraco <c>Node</c> as a new instance of <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type that the current <c>Node</c> maps to.</typeparam>
        /// <param name="includeRelationships">Whether to load all the <c>Node</c>'s relationships</param>
        /// <returns>
        /// <c>null</c> if there is no current <c>Node</c> or it does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        /// <seealso cref="GetSingle(int, bool)"/>
        public static TDestination GetCurrent<TDestination>(bool includeRelationships = true)
            where TDestination : class, new()
        {
            var node = UmbracoContext.Current.ContentCache.GetById(Node.getCurrentNodeId());

            if (node == null)
            {
                return null;
            }

            var paths = includeRelationships
                ? null // all
                : new string[0]; // none

            var context = new NodeMappingContext(node, paths, null);

            return (TDestination)_engine.Map(context, typeof(TDestination));
        }

        /// <summary>
        /// Gets all Umbraco <c>Node</c>s which map to <typeparamref name="TDestination"/> (including nodes which
        /// map to a class which derives from <typeparamref name="TDestination"/>).
        /// </summary>
        /// <typeparam name="TDestination">The type for the <c>Node</c>s to map to.</typeparam>
        /// <param name="includeRelationships">Whether to load all the <c>Node</c>s' relationships</param>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        public static IEnumerable<TDestination> GetAll<TDestination>(bool includeRelationships = true)
            where TDestination : class, new()
        {
            var destinationType = typeof(TDestination);

            if (!_engine.NodeMappers.ContainsKey(destinationType))
            {
                throw new MapNotFoundException(destinationType);
            }

            var paths = includeRelationships
                ? _engine.NodeMappers[destinationType]
                    .PropertyMappers
                    .Where(x => x.RequiresInclude)
                    .Select(x => x.DestinationInfo.Name)
                    .ToArray() // all
                : new string[0]; // none

            var query = Query<TDestination>();

            foreach (var path in paths)
            {
                query.Include(path);
            }

            return query;
        }

        /// <summary>
        /// Gets a query for nodes which map to <typeparamref name="TDestination"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type to map to.</typeparam>
        /// <returns>A fluent configuration for the query.</returns>
        /// <exception cref="MapNotFoundException">
        /// If a suitable map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        public static INodeQuery<TDestination> Query<TDestination>()
            where TDestination : class, new()
        {
            return _engine.Query<TDestination>();
        }

        #region Legacy

        /// <summary>
        /// Use <c>Find()</c> instead.
        /// </summary>
        [Obsolete("Use Find() instead")]
        public static TDestination GetSingle<TDestination>(int id, bool includeRelationships = true)
            where TDestination : class, new()
        {
            var paths = includeRelationships
                ? null // all
                : new string[0]; // none

            return (TDestination)_engine.Map(UmbracoContext.Current.ContentCache.GetById(id), typeof(TDestination), paths);
        }

        /// <summary>
        /// Gets an Umbraco <c>Node</c> as a new instance of <typeparamref name="TDestination"/>,
        /// only including the specified relationships.
        /// </summary>
        /// <typeparam name="TDestination">The type that the <c>Node</c> maps to.</typeparam>
        /// <param name="id">The ID of the <c>Node</c></param>
        /// <param name="includedRelationships">
        /// The relationships to populate <typeparamref name="TDestination"/> with.
        /// </param>
        /// <returns>
        /// <c>null</c> if the <c>Node</c> does not exist or does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        /// <example>
        /// <code>
        /// var person = uMapper.GetSingle(1234, x => x.Friends, x => x.Parent);
        /// person.Name; // not a relationship and not null
        /// person.Friends; // not null
        /// person.Parent; // not null
        /// person.Colleagues; // null
        /// </code>
        /// </example>
        [Obsolete("Use node queries with paths instead, via uMapper.Query()")]
        public static TDestination GetSingle<TDestination>(int id, params Expression<Func<TDestination, object>>[] includedRelationships)
            where TDestination : class, new()
        {
            return _engine.Map<TDestination>(UmbracoContext.Current.ContentCache.GetById(id), includedRelationships);
        }

        /// <summary>
        /// Gets the current Umbraco <c>Node</c> as a new instance of <typeparamref name="TDestination"/>,
        /// only including the specified relationships.
        /// </summary>
        /// <typeparam name="TDestination">The type that the current <c>Node</c> maps to.</typeparam>
        /// <param name="includedRelationships">
        /// The relationships to populate <typeparamref name="TDestination"/> with.
        /// </param>
        /// <returns>
        /// <c>null</c> if there is no current <c>Node</c> or it does not map to 
        /// <typeparamref name="TDestination"/>.
        /// </returns>
        [Obsolete("Use node queries with paths instead, via uMapper.Query()")]
        public static TDestination GetCurrent<TDestination>(params Expression<Func<TDestination, object>>[] includedRelationships)
            where TDestination : class, new()
        {
            return _engine.Map<TDestination>(UmbracoContext.Current.ContentCache.GetById(Node.getCurrentNodeId()), includedRelationships);
        }

        /// <summary>
        /// Gets all Umbraco <c>Node</c>s which map to <typeparamref name="TDestination"/> (including nodes which
        /// map to a class which derives from <typeparamref name="TDestination"/>),
        /// only including the specified relationships.
        /// </summary>
        /// <typeparam name="TDestination">The type for the <c>Node</c>s to map to.</typeparam>
        /// <param name="includedRelationships">
        /// The relationships to populate the <typeparamref name="TDestination"/>s with.
        /// </param>
        /// <exception cref="MapNotFoundException">
        /// If a map for <typeparamref name="TDestination"/> has not 
        /// been created with <see cref="CreateMap()" />.
        /// </exception>
        [Obsolete("Use node queries with paths instead, via uMapper.Query()")]
        public static IEnumerable<TDestination> GetAll<TDestination>(params Expression<Func<TDestination, object>>[] includedRelationships)
            where TDestination : class, new()
        {
            var paths = includedRelationships
                .Select(e => (e.Body as MemberExpression).Member as PropertyInfo)
                .Select(x => x.Name)
                .ToArray();

            var query = Query<TDestination>();

            foreach (var path in paths)
            {
                query.Include(path);
            }

            return query.AsEnumerable();
        }

        #endregion
    }
}
