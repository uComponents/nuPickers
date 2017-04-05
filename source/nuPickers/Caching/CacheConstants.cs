namespace nuPickers.Caching
{
    internal static class CacheConstants
    {
        /// <summary>
        /// key appended with a dataType id - stores all the prevalues for a picker
        /// </summary>
        public const string DataTypePreValuesPrefix = "dataTypePreValues_";

        /// <summary>
        /// key appended with a context id and property alias - stores picked keys (only applies when picker is in relations only save format)
        /// </summary>
        public const string PickedKeysPrefix = "pickedKeys_";
    }
}