
namespace nuPickers.PropertyValueConverters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EnumPicker : Picker
    {
        internal EnumPicker(int contextId, int dataTypeId, object savedValue) : base(contextId, dataTypeId, savedValue)
        {
        }

        public IEnumerable<Enum> PickedEnums
        {
            get
            {
                return Enumerable.Empty<Enum>();
            }
        }
    }
}
