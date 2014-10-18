using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using nuPickers.Mapping.Property;
using Umbraco.Core.Models;
namespace nuPickers.Mapping
{
    /// <summary>
    /// Fluent configuration for an <see cref="INodeMappingEngine"/> mapping
    /// </summary>
    /// <typeparam name="TDestination">The destination model type.</typeparam>
    public interface INodeMappingExpression<TDestination>
    {
        /// <summary>
        /// Sets a mapping for a default <c>Node</c> property (such as "Name" or "CreateDate").
        /// </summary>
        /// <typeparam name="TSourceProperty">
        /// The type of the source property.  For instance, "Name" will be a <c>string</c>,
        /// and "CreateDate" will be a <c>DateTime</c>.
        /// </typeparam>
        /// <typeparam name="TDestinationProperty">
        /// The type of the destination property.  This should be assignable to
        /// <typeparamref name="TSourceProperty"/>.
        /// </typeparam>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="nodeProperty">The node property to map from.</param>
        /// <param name="mapping">
        /// A simple mapping for the source property value, e.g. x => x.Trim()
        /// </param>
        INodeMappingExpression<TDestination> DefaultProperty<TSourceProperty, TDestinationProperty>(
            Expression<Func<TDestination, TDestinationProperty>> destinationProperty,
            Expression<Func<IPublishedContent, TSourceProperty>> nodeProperty,
            Func<TSourceProperty, TDestinationProperty> mapping
            );

        /// <summary>
        /// Sets the node property alias to be used for a non-relational model
        /// property, using the default mapping.  This property will not require an include.
        /// 
        /// Use this method only for properties mapped from a property alias - use
        /// <see cref="DefaultProperty"/> for a default <c>Node</c> property.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="propertyAlias">The new property alias to use.</param>
        INodeMappingExpression<TDestination> BasicProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            string propertyAlias
            );

        /// <summary>
        /// Sets a non-relational mapping for a model property.  This property
        /// will not require an include.
        /// </summary>
        /// <typeparam name="TSourceProperty">
        /// The desired type to inject into <paramref name="mapping"/> (will be 
        /// converted if necessary).  This should be a simple type like <c>string</c>, 
        /// <c>int?</c> or an enum.
        /// </typeparam>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// Maps the node property to the model property.
        /// </param>
        /// <param name="propertyAlias">The property alias to map from.  Optional.</param>
        /// <example>
        /// <code>
        /// uMapper.CreateMap{Dog}()
        ///     .BasicProperty{string}(
        ///         x => x.CollarId,
        ///         collarId => collarId.Trim() // trim whitespace
        ///         );
        ///         
        /// Simply trims the source property value
        /// </code>
        /// </example>
        INodeMappingExpression<TDestination> BasicProperty<TSourceProperty>(
            Expression<Func<TDestination, object>> destinationProperty,
            BasicPropertyMapping<TSourceProperty> mapping,
            string propertyAlias = null
            );

        /// <summary>
        /// Sets the node property alias to be used for a single relationship
        /// property, using the default mapping.  This property will require an include.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="propertyAlias">The new property alias to use.</param>
        INodeMappingExpression<TDestination> SingleProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            string propertyAlias
            );

        /// <summary>
        /// Sets a mapping for a single node to be used for the model
        /// property, based on the identity of the node being mapped.  
        /// This property will require an include.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// A mapping which take the ID of the node being mapped, and returns 
        /// the ID of the node to map to the property.
        /// </param>
        INodeMappingExpression<TDestination> SingleProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            Func<int, int?> mapping
            );

        /// <summary>
        /// Sets a mapping for a single node to be used for the model
        /// property, based on the value of a node property.  
        /// This property will require an include.
        /// </summary>
        /// <typeparam name="TSourceProperty">
        /// The desired type to inject into <paramref name="mapping"/> (will be 
        /// converted if necessary).  This should be a simple type like <c>string</c> or 
        /// <c>int?</c>.
        /// </typeparam>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// A mapping which takes the value of a node property, and returns 
        /// the ID of the node to map to the property.
        /// </param>
        /// <param name="propertyAlias">
        /// An optional node property alias override.
        /// </param>
        /// <example>
        /// <code>
        /// // "SearchAllDogsForId" is an external method which searches through all 
        /// // dogs based on a name property.
        /// 
        /// uMapper.CreateMap{Dog}()
        ///     .SingleProperty{string}(
        ///         x => x.BestFriend,
        ///         name => SearchAllDogsForId(name), // must return an `int?`
        ///         "bestFriendName" // required as "BestFriend" !~ "bestFriendName"
        ///         );
        ///         
        /// </code>
        /// </example>
        INodeMappingExpression<TDestination> SingleProperty<TSourceProperty>(
            Expression<Func<TDestination, object>> destinationProperty,
            SinglePropertyMapping<TSourceProperty> mapping,
            string propertyAlias = null
            );

        /// <summary>
        /// Sets the node property alias to be used for a collection relationship
        /// property, using the default mapping.  This property will require an include.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="propertyAlias">The new property alias to use.</param>
        INodeMappingExpression<TDestination> CollectionProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            string propertyAlias
            );

        /// <summary>
        /// Sets a mapping for a collection node to be used for the model
        /// property, based on the identity of the node being mapped.  
        /// This property will require an include.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// A mapping which take the ID of the node being mapped, and returns 
        /// the collection of IDs.
        /// </param>
        INodeMappingExpression<TDestination> CollectionProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            Func<int, IEnumerable<int>> mapping
            );

        /// <summary>
        /// Sets a mapping for a collection to be used for the model
        /// property.  This property will require an include.
        /// </summary>
        /// <typeparam name="TSourceProperty">
        /// The desired type to inject into <paramref name="mapping"/> (will be 
        /// converted if necessary).  This should be a simple type like <c>string</c>, 
        /// <c>int?</c> or <c>int[]</c>.
        /// </typeparam>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// A mapping which retrieves the IDs of the items in the collection.
        /// </param>
        /// <param name="propertyAlias">
        /// An optional node property alias override.
        /// </param>
        /// <example>
        /// <code>
        /// // This will restrict the number of favourite treats mapped to 3.
        /// 
        /// uMapper.CreateMap{Dog}()
        ///     .CollectionProperty{int[]}(
        ///         x => x.FavouriteTreats,
        ///         ids => ids.Take(3)
        ///         );
        /// </code>
        /// </example>
        INodeMappingExpression<TDestination> CollectionProperty<TSourceProperty>(
            Expression<Func<TDestination, object>> destinationProperty,
            CollectionPropertyMapping<TSourceProperty> mapping,
            string propertyAlias = null
            );

        /// <summary>
        /// Sets a custom mapping to be used for a model property.
        /// </summary>
        /// <param name="destinationProperty">The property to map to.</param>
        /// <param name="mapping">
        /// A mapping which retrieves the value of the property.
        /// </param>
        /// <param name="requiresInclude">
        /// Whether the property requires an explicit include.
        /// </param>
        /// <param name="allowCaching">
        /// If set to true, the result of <paramref name="mapping"/> will be cached.
        /// While caching is disabled on the <see cref="INodeMappingEngine"/>, no caching will occur.
        /// </param>
        INodeMappingExpression<TDestination> CustomProperty(
            Expression<Func<TDestination, object>> destinationProperty,
            CustomPropertyMapping mapping,
            bool requiresInclude,
            bool allowCaching
            );

        /// <summary>
        /// Removes the mapping for a property, if any exists.
        /// </summary>
        /// <param name="destinationProperty">The property on the model to NOT map to</param>
        INodeMappingExpression<TDestination> RemoveMappingForProperty(
            Expression<Func<TDestination, object>> destinationProperty
            );

        #region Legacy

        /// <summary>
        /// Sets a custom mapping to be used for a the model property.
        /// </summary>
        /// <param name="destinationProperty">The member of the destination model
        /// to map to.</param>
        /// <param name="propertyMapping">
        /// The new mapping function.  Arguments are the node being mapped, 
        /// and an array of paths relative to the property being mapped
        /// (when mapping a relationship).
        /// </param>
        /// <param name="requiresInclude">
        /// Whether the property requires an explicit include to be mapped.
        /// </param>
        [Obsolete("Use BasicProperty, SingleProperty, CollectionProperty or CustomProperty instead")]
        INodeMappingExpression<TDestination> ForProperty<TProperty>(
            Expression<Func<TDestination, TProperty>> destinationProperty,
            Func<IPublishedContent, string[], object> propertyMapping,
            bool requiresInclude
            );

        /// <summary>
        /// Sets a custom mapping to be used for a the model property.
        /// </summary>
        /// <param name="destinationProperty">The member of the destination model
        /// to map to.</param>
        /// <param name="propertyMapping">
        /// The new mapping function.
        /// </param>
        /// <param name="isRelationship">Whether the property should be deemed a relationship
        /// or not.</param>
        [Obsolete("Use BasicProperty, SingleProperty, CollectionProperty or CustomProperty instead")]
        INodeMappingExpression<TDestination> ForProperty<TProperty>(
            Expression<Func<TDestination, TProperty>> destinationProperty,
            Func<IPublishedContent, object> propertyMapping,
            bool isRelationship
            );

        /// <summary>
        /// Sets a custom property alias to be set for a the model property.
        /// </summary>
        /// <param name="destinationProperty">The member of the destination model
        /// to map to.</param>
        /// <param name="nodeTypeAlias">The property alias to map from.</param>
        [Obsolete("Use BasicProperty, SingleProperty or CollectionProperty instead")]
        INodeMappingExpression<TDestination> ForProperty<TProperty>(
            Expression<Func<TDestination, TProperty>> destinationProperty,
            string nodeTypeAlias
            );

        #endregion
    }
}
