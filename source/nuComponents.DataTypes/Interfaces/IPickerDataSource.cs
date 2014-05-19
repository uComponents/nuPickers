
namespace nuComponents.DataTypes.Interfaces
{
    using System.Collections.Generic;
    using nuComponents.DataTypes.Shared.Picker;

    internal interface IPickerDataSource
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contextId">the id of the current content / media or member being edited</param>
        /// <returns></returns>
        IEnumerable<PickerEditorOption> GetEditorOptions(int contextId);
    }
}
