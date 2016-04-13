namespace nuPickers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.EnumDataSource;
    using nuPickers.Shared.RelationMapping;
    using nuPickers.Shared.SaveFormat;
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
        /// cache var, set once, stores the configuration options for the data-type this picker is using
        /// </summary>
        private IDictionary<string, PreValue> dataTypePreValues = null;

        /// <summary>
        /// cache var, stores value after querying relations or parsing a save format
        /// </summary>
        private string[] pickedKeys = null;

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
            Picker picker;

            switch (Helper.GetUmbracoObjectType(this.ContextId))
            {
                case uQuery.UmbracoObjectType.Document:
                    picker = umbracoHelper.TypedContent(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias);
                    this.DataTypeId = picker.DataTypeId;

                    if (usePublishedValue)
                    {
                        this.SavedValue = picker.SavedValue;
                    }
                    else
                    {
                        this.SavedValue = ApplicationContext.Current.Services.ContentService.GetById(this.ContextId).GetValue(propertyAlias);
                    }

                    break;

                case uQuery.UmbracoObjectType.Media:
                    picker = umbracoHelper.TypedMedia(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias);
                    this.DataTypeId = picker.DataTypeId;
                    this.SavedValue = picker.SavedValue;
                    break;

                case uQuery.UmbracoObjectType.Member:
                    picker = umbracoHelper.TypedMember(this.ContextId).GetPropertyValue<Picker>(this.PropertyAlias);
                    this.DataTypeId = picker.DataTypeId;
                    this.SavedValue = picker.SavedValue;
                    break;
            }
        }

        /// <summary>
        /// internal constructor - picker value is supplied (used by PropertyValueConverter & RelationMappingEvent)
        /// </summary>
        /// <param name="contextId">the id of the content, media or member (-1 means out of context)</param>
        /// <param name="propertyAlias">the property alias</param>
        /// <param name="dataTypeId">the id of the datatype (to access to prevalues)</param>
        /// <param name="savedValue">the actual value saved</param>
        internal Picker(int contextId, string propertyAlias, int dataTypeId, object savedValue)
        {
            this.ContextId = contextId;
            this.PropertyAlias = propertyAlias;
            this.DataTypeId = dataTypeId;
            this.SavedValue = savedValue;
        }

        /// <summary>
        /// the value of this picker
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
                        this.pickedKeys = RelationMapping
                                            .GetRelatedIds(this.ContextId, this.PropertyAlias, this.RelationTypeAlias, true)
                                            .Select(x => x.ToString())
                                            .ToArray();
                    }
                    else
                    {
                        this.pickedKeys = this.SavedValue != null ? SaveFormat
                                                                        .GetSavedKeys(this.SavedValue.ToString())
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
        internal int ContextId { get; set; }

        /// <summary>
        /// the alias of this picker on the content, media or member item
        /// </summary>
        internal string PropertyAlias { get; set; }

        /// <summary>
        /// the data-type this picker is using (no association with a specific SaveValue)
        /// </summary>
        private int DataTypeId { get; set; }

        /// <summary>
        /// property accessor to ensure the query to populate the data-type configruation options is only done once
        /// </summary>
        private IDictionary<string, PreValue> DataTypePreValues
        {
            get
            {
                if (this.dataTypePreValues == null)
                {
                    this.dataTypePreValues = ApplicationContext
                                                .Current
                                                .Services
                                                .DataTypeService
                                                .GetPreValuesCollectionByDataTypeId(this.DataTypeId)
                                                .PreValuesAsDictionary;
                }

                return this.dataTypePreValues;
            }
        }

        /// <summary>
        /// Helper for DataTypePreValues dictionary collection
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the PreValue if found, or null</returns>
        public PreValue GetDataTypePreValue(string key)
        {
            return this.DataTypePreValues.SingleOrDefault(x => string.Equals(x.Key, key, StringComparison.InvariantCultureIgnoreCase)).Value;
        }

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
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this.SavedValue != null)
            {
                return this.SavedValue.ToString();
            }

            return base.ToString();
        }
    }
}
