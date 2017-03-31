﻿
namespace nuPickers.Shared.DotNetDataSource
{
    using System.Collections.Generic;

    public interface IDotNetDataSourceV2
    {
        /// <returns>
        /// 1st string is the key
        /// 2nd string is the label
        /// </returns>
        IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId, int parentId);
    }
}
