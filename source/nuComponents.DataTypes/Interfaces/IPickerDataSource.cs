
namespace nuComponents.DataTypes.Interfaces
{
    using System.Collections.Generic;
    using nuComponents.DataTypes.Shared.Picker;

    interface IPickerDataSource
    {
        /// <summary>
        /// all pickers require a collection to select from
        /// </summary>
        /// <returns></returns>
        IEnumerable<PickerEditorOption> GetEditorOptions();
    }
}
