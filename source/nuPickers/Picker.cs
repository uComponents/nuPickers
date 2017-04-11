namespace nuPickers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using nuPickers.Caching;
    using nuPickers.Extensions;
    using nuPickers.Shared.EnumDataSource;
    using nuPickers.Shared.RelationMapping;
    using nuPickers.Shared.SaveFormat;
    using Shared.DataSource;
    using Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    /// <summary>
    /// This class is the main consumer interface
    /// Umbraco Models Builder will return this class automatically (for the current context)
    /// </summary>
    public class Picker
    {        
        /// <summary>
        /// cache var, stores value after querying relations or parsing a save format
        /// </summary>
        private string[] pickedKeys = null;

        #region Constructors

        /// <summary>
        /// public constructor - picker value is calculated from lookup, either the published value from cache, or the latest saved db value 
        /// </summary>
        /// <param name="contextId">the id of the content, media or member</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="usePublishedValue">when true uses the published value, otherwise when false uses the lastest saved value (which may also be the published value)</param>
        public Picker(int contextId, string propertyAlias, bool usePublishedValue = true)
        {
            bool success = false;

            var publishedContent = new UmbracoHelper(UmbracoContext.Current).GetPublishedContent(contextId);

            if (usePublishedValue && publishedContent != null)
            {                
                var property = publishedContent.GetProperty(propertyAlias);

                if (property != null)
                {
                    var propertyType = publishedContent.ContentType.PropertyTypes.Single(x => x.PropertyTypeAlias == property.PropertyTypeAlias);

                    this.ContextId = publishedContent.Id;
                    this.ParentId = (publishedContent.Parent != null) ? publishedContent.Parent.Id : -1;
                    this.PropertyAlias = propertyAlias;
                    this.DataTypeId = propertyType.DataTypeId;
                    this.PropertyEditorAlias = propertyType.PropertyEditorAlias;
                    this.SavedValue = property.Value;

                    success = true;
                }
            }
            else // use unpublished data
            {
                var content = ApplicationContext.Current.Services.ContentService.GetById(contextId);

                if (content != null)
                {
                    var property = content.Properties.SingleOrDefault(x => x.Alias == propertyAlias);

                    if (property != null)
                    {
                        var propertyType = content.PropertyTypes.Single(x => x.Alias == property.Alias);

                        this.ContextId = content.Id;
                        this.ParentId = content.ParentId;
                        this.PropertyAlias = propertyAlias;
                        this.DataTypeId = propertyType.DataTypeDefinitionId;
                        this.PropertyEditorAlias = propertyType.PropertyEditorAlias;
                        this.SavedValue = content.GetValue(PropertyAlias);

                        success = true;
                    }
                }
            }

            if (!success)
            {
                throw new Exception(string.Format("Unable to create Picker object for ContextId: {0}, PropertyAlias: {1}", contextId.ToString(), propertyAlias));
            }
        }

        /// <summary>
        /// internal constructor - picker value is supplied (used by PropertyValueConverter & RelationMappingEvent)
        /// </summary>
        /// <param name="contextId">the id of the content, media or member (-1 means out of context)</param>
        /// <param name="parentId">the partent id of the content or media</param>
        /// <param name="propertyAlias">the property alias for this picker instance</param>
        /// <param name="dataTypeId">the dataType id of this picker</param>
        /// <param name="propertyEditorAlias">the alias of the dataType</param>
        /// <param name="savedValue">the actual value saved</param>
        internal Picker(int contextId, int parentId, string propertyAlias, int dataTypeId, string propertyEditorAlias, object savedValue)
        {
            this.ContextId = contextId;
            this.ParentId = parentId;
            this.PropertyAlias = propertyAlias;
            this.DataTypeId = dataTypeId;
            this.PropertyEditorAlias = propertyEditorAlias;
            this.SavedValue = savedValue;
        }

        #endregion

        #region Properties

        /// <summary>
        /// the value of this picker
        /// </summary>
        //[Obsolete("[v2.0.0] rename to Value (as it might be the saved value, or it might be the published value)")]
        public object SavedValue { get; private set; }

        /// <summary>
        /// returns a collection of all picked keys (regardless as to how / where they are persisted)
        /// </summary>
        public IEnumerable<string> PickedKeys
        {
            get
            {
                if (this.pickedKeys == null)
                {
                    if (this.GetDataTypePreValue("saveFormat").Value == "relationsOnly")
                    {
                        // attempt to find relations data in memory cache
                        this.pickedKeys = Cache.Instance.GetSet(CacheConstants.PickedKeysPrefix + this.ContextId.ToString() + "_" + this.PropertyAlias, () =>
                        {
                            // fallback to hitting database
                            return RelationMapping
                                    .GetRelatedIds(this.ContextId, this.PropertyAlias, this.RelationTypeAlias, true)
                                    .Select(x => x.ToString())
                                    .ToArray();
                        });
                    }
                    else
                    {
                        this.pickedKeys = this.SavedValue != null ? SaveFormat.GetKeys(this.SavedValue.ToString()).ToArray() : new string[]{};
                    }
                }

                return this.pickedKeys;
            }

            // special case - allows relation-mapping event to set here (so preventing them from being taken from the database)
            internal set
            {
                this.pickedKeys = value.ToArray();
            }
        }

        /// <summary>
        /// returns a collection of all picked keys that can be converted into integer ids
        /// </summary>
        internal IEnumerable<int> PickedIds
        {
            get
            {
                return this.PickedKeys
                            .Where(x => { int id; return int.TryParse(x, out id); })
                            .Select(int.Parse);
            }
        }

        /// <summary>
        /// the relation type alias if relation mapping active, or null
        /// </summary>
        internal string RelationTypeAlias
        {
            get
            {
                // not all pickers support relation mapping, so null check required
                PreValue relationMappingPreValue = this.GetDataTypePreValue("relationMapping");
                if (relationMappingPreValue != null && !string.IsNullOrWhiteSpace(relationMappingPreValue.Value))
                {
                    // parse the json config to get a relationType alias
                    return JObject.Parse(relationMappingPreValue.Value).GetValue("relationTypeAlias").ToString();
                }

                return null;
            }
        }

        /// <summary>
        /// the id of the content, media or member item
        /// </summary>
        internal int ContextId { get; set; } // TODO: rename to current

        /// <summary>
        /// 
        /// </summary>
        private int ParentId { get; set; }

        /// <summary>
        /// the alias of this picker on the content, media or member item
        /// </summary>
        internal string PropertyAlias { get; set; }

        /// <summary>
        /// the data-type this picker is using (no association with a specific SaveValue)
        /// </summary>
        private int DataTypeId { get; set; }

        /// <summary>
        /// The alias used to identify a picker type (could be calculated from the dataType id, but this avoids additional queries)
        /// (aka dataType alias)
        /// </summary>
        private string PropertyEditorAlias { get; set; }

        /// <summary>
        /// Property accessor to ensure the query to populate the data-type configruation options is only done once per server
        /// </summary>
        private IDictionary<string, PreValue> DataTypePreValues
        {
            get
            {
                return Cache.Instance.GetSet(CacheConstants.DataTypePreValuesPrefix + this.DataTypeId, () =>
                {
                    return  ApplicationContext
                                .Current
                                .Services
                                .DataTypeService
                                .GetPreValuesCollectionByDataTypeId(this.DataTypeId)
                                .PreValuesAsDictionary;
                });
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all the prevalues for this picker 
        /// </summary>
        /// <returns>collection of all <see cref="PreValue"/> for this datatype</returns>
        public IDictionary<string, PreValue> GetDataTypePreValues()
        {
            return this.DataTypePreValues;
        }

        /// <summary>
        /// Find a specific prevalue for this picker 
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the <see cref="PreValue"/> if found, or null</returns>
        public PreValue GetDataTypePreValue(string key)
        {
            return this.DataTypePreValues.SingleOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

        /// <summary>
        /// Get a collection of all (key/label) items for this picker
        /// NOTE: typeahead pickers return only those items for the supplied typeahead text
        /// WARNING: this method will re-query the data source
        /// </summary>
        /// <param name="typeahead">typeahead text (required for typeahead pickers, ignored for non-typeahead pickers)</param>
        /// <returns>a collection of <see cref="EditorDataItem"/> items</returns>
        public IEnumerable<EditorDataItem> GetEditorDataItems(string typeahead = null)
        {
            return Editor.GetEditorDataItems(
                            this.ContextId,
                            this.ParentId,
                            this.PropertyAlias,
                            DataSource.GetDataSource(this.PropertyEditorAlias, this.GetDataTypePreValue("dataSource").Value),
                            this.GetDataTypePreValue("customLabel").Value,
                            typeahead);
        }

        /// <summary>
        /// Get a collection of the picked (key/label) items
        /// WARNING: for CSV and Relation Only save formats, the label data is missing, so will trigger a query
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EditorDataItem> GetPickedEditorDataItems()
        {
            IEnumerable<EditorDataItem> editorDataItems;

            if (!SaveFormat.TryGetDataEditorItems(this.SavedValue.ToString(), out editorDataItems))
            {
                editorDataItems = Editor.GetEditorDataItems(
                                    this.ContextId,
                                    this.ParentId,
                                    this.PropertyAlias,
                                    DataSource.GetDataSource(this.PropertyEditorAlias, this.GetDataTypePreValue("dataSource").Value),
                                    this.GetDataTypePreValue("customLabel").Value,
                                    this.PickedKeys.ToArray());
            }

            return editorDataItems ?? Enumerable.Empty<EditorDataItem>();
        }

        /// <summary>
        /// Get all picked items, objects may be typed Content, Media or Member (but all returned as IPublishedContent)
        /// </summary>
        /// <returns>a collection of IPublishedContent, or an empty collection</returns>
        //[Obsolete("[v2.0.0] use picker.PickedKeys.AsPublishedContent() instead")]
        public IEnumerable<IPublishedContent> AsPublishedContent()
        {
            return this.PickedKeys.AsPublishedContent();
        }

        /// <summary>
        /// Get all picked items, objects may be typed Content, Media or Member (but all returned as dynamic) 
        /// </summary>
        /// <returns></returns>
        //[Obsolete("[v2.0.0]")]
        public IEnumerable<dynamic> AsDynamicPublishedContent()
        {
            return this.AsPublishedContent().Select(x => x.AsDynamic());
        }

        /// <summary>
        /// get all picked items and attempt to convert them to the supplied enum type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>a collection of Enums of type T, or an empty collection</returns>
        public IEnumerable<T> AsEnums<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }

            List<T> enums = new List<T>();

            foreach (string pickedKey in this.PickedKeys)
            {
                foreach(Enum enumItem in Enum.GetValues(typeof(T)))
                {
                    if (pickedKey == enumItem.GetKey())
                    {
                        Attempt<T> attempt = enumItem.TryConvertTo<T>();
                        if (attempt.Success)
                        {
                            enums.Add(attempt.Result);
                        }
                    }
                }
            }

            return enums;
        }

        /// <summary>
        /// get all picked items as a collection of Enum
        /// </summary>
        /// <returns>a collection of Enums, or empty collection</returns>
        public IEnumerable<Enum> AsEnums()
        {
            List<Enum> enums = new List<Enum>();
            PreValue dataSourceJson = this.GetDataTypePreValue("dataSource");

            if (dataSourceJson != null)
            {
                EnumDataSource enumDataSouce = JsonConvert.DeserializeObject<EnumDataSource>(dataSourceJson.Value);

                Type enumType = Helper.GetAssembly(enumDataSouce.AssemblyName).GetType(enumDataSouce.EnumName);

                foreach(string pickedKey in this.PickedKeys)
                {
                    foreach(Enum enumItem in Enum.GetValues(enumType))
                    {
                       if (pickedKey == enumItem.GetKey())
                       {
                           enums.Add(enumItem);
                       }
                    }
                }
            }

            return enums;
        }

        /// <summary>
        /// Returns the string value for this picker
        /// </summary>
        /// <returns>The SavedValue as a string, otherwise base object.ToString()</returns>
        public override string ToString()
        {
            return this.SavedValue != null ? this.SavedValue.ToString() : base.ToString();
        }

        #endregion
    }
}