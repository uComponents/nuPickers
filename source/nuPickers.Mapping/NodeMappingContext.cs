using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace nuPickers.Mapping
{
    /// <summary>
    /// Provides a context for a stack of mapping operations.  Lifetime should complete
    /// once the bottom mapping operation is completed.
    /// </summary>
    public class NodeMappingContext
    {
        public NodeMappingContext(int id, string[] paths, NodeMappingContext parent)
        {
            Id = id;
            Paths = paths;
            ParentContext = parent;

            _nodeCache = new List<IPublishedContent>();
        }

        public NodeMappingContext(IPublishedContent node, string[] paths, NodeMappingContext parent)
            : this(node.Id, paths, parent)
        {
            _nodeCache.Add(node);
        }

        /// <summary>
        /// The ID of the node being mapped.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The paths to be mapped.
        /// </summary>
        public string[] Paths { get; set; }

        /// <summary>
        /// The context which spawned this context
        /// </summary>
        public NodeMappingContext ParentContext { get; set; }

        /// <summary>
        /// Gets the node being mapped.
        /// </summary>
        public IPublishedContent GetNode()
        {
            var foundNode = GetNodeFromContextCache(Id);

            if (foundNode == null)
            {

                foundNode = UmbracoContext.Current.ContentCache.GetById(Id);
                
                if (string.IsNullOrEmpty(foundNode.Name))
                {
                    return null;
                }
            }

            AddNodeToContextCache(foundNode);

            return foundNode;
        }

        #region Context cache

        /// <summary>
        /// Stores the nodes which have been cached during this operation.
        /// </summary>
        private readonly List<IPublishedContent> _nodeCache;

        /// <summary>
        /// Looks for a <c>Node</c> in the context tree.
        /// </summary>
        /// <param name="id">The ID of the node.</param>
        /// <returns>The node, or null if not found.</returns>
        public IPublishedContent GetNodeFromContextCache(int id)
        {
            var currentContext = this;

            while (currentContext.ParentContext != null)
            {
                var foundNode = currentContext._nodeCache.FirstOrDefault(n => n.Id == id);
                if (foundNode != null) return foundNode;

                currentContext = currentContext.ParentContext;
            }

            return null;
        }

        /// <summary>
        /// Adds a node to the context cache.
        /// </summary>
        public void AddNodeToContextCache(IPublishedContent node)
        {
            if (node == null
                || string.IsNullOrEmpty(node.Name))
            {
                return;
            }

            _nodeCache.Add(node);
        }

        /// <summary>
        /// Adds a collection of nodes to the context cache.
        /// </summary>
        public void AddNodesToContextCache(IEnumerable<IPublishedContent> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException("nodes");
            }

            foreach (var node in nodes)
            {
                AddNodeToContextCache(node);
            }
        }

        #endregion
    }
}
