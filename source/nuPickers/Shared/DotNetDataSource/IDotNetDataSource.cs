
namespace nuPickers.Shared.DotNetDataSource
{
    using System.Collections.Generic;

    public interface IDotNetDataSource
    {
        /// <returns>
        /// returns a collection of items for the picker (1st string is the key, 2nd string is the label)
        /// TODO: breaking change, replace contextId with currentId & parentId
        /// </returns>
        IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId);
    }
}
