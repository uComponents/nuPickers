namespace nuPickers.PropertyValueConverters
{
    using Newtonsoft.Json;
    using nuPickers.Shared.EnumDataSource;
    using nuPickers.Shared.SaveFormat;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Hosting;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;

    public class Picker
    {
        private int ContextId { get; set; }
        private int DataTypeId { get; set; }
        private object SavedValue { get; set; }

        private IDictionary<string, PreValue> dataTypePreValues = null;
        public IDictionary<string, PreValue> DataTypePreValues
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
        /// 
        /// </summary>
        /// <param name="contextId">the id of the (content, media or member) item being edited</param>
        /// <param name="dataTypeId">the id of the datatype (a property editor instance) this allows access to all prevalues</param>
        /// <param name="savedValue">the actual value saved</param>
        internal Picker(int contextId, int dataTypeId, object savedValue)
        {
            this.ContextId = contextId;
            this.DataTypeId = dataTypeId;
            this.SavedValue = savedValue;
        }

        public IEnumerable<string> PickedKeys
        {
            get
            {
                // if saveFormat (config) is relationsOnly...

                // ignore the specified saved format, and let save format try and restore collection directly from the saved value
                return this.SavedValue != null ? SaveFormat.GetSavedKeys(this.SavedValue.ToString()) : null;
            }
        }

        public IEnumerable<IPublishedContent> AsPublishedContent()
        {
            UmbracoHelper umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            List<IPublishedContent> publishedContentList = new List<IPublishedContent>();

            // Return empty so that don't we don't get exceptions if nothing selected
            if (this.PickedKeys == null)
            {
                return publishedContentList;
            }

            foreach (var pickedKey in this.PickedKeys)
            {
                Attempt<int> attemptNodeId = pickedKey.TryConvertTo<int>();
                if (attemptNodeId.Success)
                {
                    switch (uQuery.GetUmbracoObjectType(attemptNodeId.Result))
                    {
                        case uQuery.UmbracoObjectType.Document:
                            publishedContentList.Add(umbracoHelper.TypedContent(attemptNodeId.Result));
                            break;
                        case uQuery.UmbracoObjectType.Media:
                            publishedContentList.Add(umbracoHelper.TypedMedia(attemptNodeId.Result));
                            break;
                        case uQuery.UmbracoObjectType.Member:
                            publishedContentList.Add(umbracoHelper.TypedMember(attemptNodeId.Result));
                            break;
                    }
                }
            }

            return publishedContentList.Where(x => x != null);
        }

        public IEnumerable<dynamic> AsDynamicPublishedContent()
        {
            return this.AsPublishedContent().Select(x => x.AsDynamic());
        }

        public IEnumerable<T> AsEnum<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum");
            }

            List<T> enums = new List<T>();

            foreach (var pickedKey in this.PickedKeys)
            {
                Attempt<T> attemptEnum = pickedKey.TryConvertTo<T>();
                if (attemptEnum.Success)
                {
                    enums.Add(attemptEnum.Result);
                }
            }

            return enums;
        }

        public IEnumerable<Enum> AsEnum()
        {
            PreValue dataSourceJson = this.DataTypePreValues.FirstOrDefault(x => string.Equals(x.Key, "dataSource", StringComparison.InvariantCultureIgnoreCase)).Value;

            if (dataSourceJson != null)
            {
                EnumDataSource enumDataSouce = JsonConvert.DeserializeObject<EnumDataSource>(dataSourceJson.Value);
                Assembly assembly = Assembly.LoadFrom(HostingEnvironment.MapPath("~/bin/" + enumDataSouce.AssemblyName));
                Type enumType = assembly.GetType(enumDataSouce.EnumName);

                return this.PickedKeys.Select(pickedKey => (Enum)Enum.Parse(enumType, pickedKey)).ToArray();
            }

            return null;
        }
    }
}
