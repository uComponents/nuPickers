namespace nuPickers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using nuPickers.Caching;
    using nuPickers.Shared.EnumDataSource;
    using nuPickers.Shared.RelationMapping;
    using nuPickers.Shared.SaveFormat;
    using Shared.DataSource;
    using Shared.Editor;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;

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
            this.ContextId = contextId;
            this.PropertyAlias = propertyAlias;

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);
            Picker picker = null;

            // based on type, ask Umbraco for a picker obj (via the PickerPropertyValueConverter)
            switch (Helper.GetUmbracoObjectType(this.ContextId))
            {
                case uQuery.UmbracoObjectType.Document: picker = umbracoHelper.TypedContent(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias);

                    if (!usePublishedValue)
                    {
                        // reset (private) property on returned picker, so it's value is taken from the database
                        picker.SavedValue = ApplicationContext.Current.Services.ContentService.GetById(this.ContextId).GetValue(propertyAlias);
                    }

                    break;

                case uQuery.UmbracoObjectType.Media: picker = umbracoHelper.TypedMedia(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias); break;
                case uQuery.UmbracoObjectType.Member: picker = umbracoHelper.TypedMember(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias);  break;
            }

            this.ParentId = picker.ParentId;
            this.DataTypeId = picker.DataTypeId;
            this.PropertyEditorAlias = picker.PropertyEditorAlias;
            this.SavedValue = picker.SavedValue;
        }

        /// <summary>
        /// internal constructor - picker value is supplied (used by PropertyValueConverter & RelationMappingEvent)
        /// </summary>
        /// <param name="contextId">the id of the content, media or member (-1 means out of context)</param>
        /// <param name="PublishedPropertyType">contains details about the propety editor</param>
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
        /// TODO: breaking change for 2.0.0, rename to Value (as it might be the saved, or it might be the published)
        /// </summary>
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
                        this.pickedKeys = Cache.Instance.GetSet(CacheConstants.PickedKeysPrefix + this.ContextId.ToString() + "_" + this.PropertyAlias, () =>
                        {
                            return RelationMapping
                                    .GetRelatedIds(this.ContextId, this.PropertyAlias, this.RelationTypeAlias, true)
                                    .Select(x => x.ToString())
                                    .ToArray();
                        });
                    }
                    else
                    {
                        this.pickedKeys = this.SavedValue != null ? SaveFormat
                                                                        .GetKeys(this.SavedValue.ToString())
                                                                        .ToArray() : new string[]{};
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
                int pickedId;
                List<int> pickedIds = new List<int>();
                foreach (string pickedKey in this.PickedKeys)
                {
                    if (int.TryParse(pickedKey, out pickedId))
                    {
                        pickedIds.Add(pickedId);
                    }
                }

                return pickedIds;
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
        /// Get all the prevalues (used for testing - is there a consumer use-case ?)
        /// </summary>
        /// <returns>collection of all <see cref="PreValue"/> for this datatype</returns>
        public IDictionary<string, PreValue> GetDataTypePreValues()
        {
            return this.DataTypePreValues;
        }

        /// <summary>
        /// Helper to find a specific PreValue in the DataTypePreValues dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the PreValue if found, or null</returns>
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
        public IEnumerable<EditorDataItem> GetItems(string typeahead = null)
        {
            return Editor.GetEditorDataItems(
                            this.ContextId,
                            this.ParentId,
                            this.PropertyAlias,
                            DataSource.GetDataSource(this.PropertyEditorAlias, this.GetDataTypePreValue("dataSource").Value),
                            this.GetDataTypePreValue("customLabel").Value,
                            typeahead);
        }

        ///// <summary>
        ///// Get a collection of the picked (key/label) items
        ///// WARNING: for CSV and Relation Only save formats, the label data is missing, so will trigger a query
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<EditorDataItem> GetPickedItems()
        //{
        //    throw new NotImplementedException();

        //    return Enumerable.Empty<EditorDataItem>();
        //}

        /// <summary>
        /// get all picked items, objects may be typed Content, Media or Member (but all returned as IPublishedContent)
        /// </summary>
        /// <returns>a collection of IPublishedContent, or an empty collection</returns>
        public IEnumerable<IPublishedContent> AsPublishedContent()
        {
            List<IPublishedContent> publishedContent = new List<IPublishedContent>();

            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            foreach (var pickedKey in this.PickedKeys)
            {
                Attempt<int> attemptNodeId = pickedKey.TryConvertTo<int>();
                if (attemptNodeId.Success)
                {
                    switch (Helper.GetUmbracoObjectType(attemptNodeId.Result))
                    {
                        case uQuery.UmbracoObjectType.Document:
                            publishedContent.Add(umbracoHelper.TypedContent(attemptNodeId.Result));
                            break;

                        case uQuery.UmbracoObjectType.Media:
                            publishedContent.Add(umbracoHelper.TypedMedia(attemptNodeId.Result));
                            break;

                        case uQuery.UmbracoObjectType.Member:
                            publishedContent.Add(umbracoHelper.TypedMember(attemptNodeId.Result));
                            break;
                    }
                }
            }

            return publishedContent.Where(x => x != null);
        }

        /// <summary>
        /// get all picked items, objects may be typed Content, Media or Member (but all returned as dynamic) 
        /// </summary>
        /// <returns></returns>
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
            if (this.SavedValue != null)
            {
                return this.SavedValue.ToString();
            }

            return base.ToString();
        }

        #endregion
    }
}