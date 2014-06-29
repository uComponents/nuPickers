namespace nuPickers.PropertyValueConverters
{
    using nuPickers.Shared.SaveFormat;
    using System.Collections.Generic;
    using Umbraco.Core;
    using Umbraco.Core.Models;

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
                return SaveFormat.GetSavedKeys(this.SavedValue.ToString());
            }
        }
    }
}
