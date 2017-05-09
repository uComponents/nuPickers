namespace nuPickers.Shared.DotNetDataSource
{
    //using System;
    //using nuPickers.Shared.Editor;
    using System.Collections.Generic;

    /// <summary>
    /// All classes that implement this interface can be used as a data-source for a nuPickers property-editor
    /// </summary>
    public interface IDotNetDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextId">the current content, media, or member id (or the parent if 0)</param>
        /// <returns>a collection of string key, label pairs</returns>
        //[Obsolete("BREAKING CHANGE:This method will be obsoleted by the one supplying both current and parent context")]
        IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId);

        /// <summary>
        /// this method will eventually replace the one above
        /// </summary>
        /// <param name="currentId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        //IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId);
    }
}