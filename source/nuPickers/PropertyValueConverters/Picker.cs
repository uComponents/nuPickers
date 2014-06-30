namespace nuPickers.PropertyValueConverters
{
    using System.Collections.Generic;
    using System.Linq;

    using nuPickers.Shared.SaveFormat;

    using umbraco;

    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Web;
    using Umbraco.Web.Models;

    public class Picker
    {
        protected int ContextId { get; private set; }
        protected int DataTypeId { get; private set; }
        protected object SavedValue { get; private set; }

        private IEnumerable<PreValue> dataTypePreValues = null;
        protected IEnumerable<PreValue> DataTypePreValues
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
                                                .PreValuesAsArray;
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
            var umbHelper = new UmbracoHelper(UmbracoContext.Current);

            var publishedContentList = new List<IPublishedContent>();

            // Return empty so that don't we don't get exceptions if nothing selected
            if (this.PickedKeys == null)
            {
                return Enumerable.Empty<IPublishedContent>();
            }

            foreach (var pickedKey in this.PickedKeys)
            {
                var attemptNodeId = pickedKey.TryConvertTo<int>();
                if (attemptNodeId.Success)
                {
                    var objectType = uQuery.GetUmbracoObjectType(attemptNodeId.Result);

                    switch (objectType)
                    {
                        case uQuery.UmbracoObjectType.Document:
                            publishedContentList.Add(umbHelper.TypedContent(attemptNodeId.Result));
                            break;
                        case uQuery.UmbracoObjectType.Media:
                            publishedContentList.Add(umbHelper.TypedMedia(attemptNodeId.Result));
                            break;
                        case uQuery.UmbracoObjectType.Member:
                            publishedContentList.Add(umbHelper.TypedMember(attemptNodeId.Result));
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
    }
}
