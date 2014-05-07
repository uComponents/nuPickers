
namespace nuComponents.DataTypes.Interfaces
{
    using System.Collections.Generic;
    using nuComponents.DataTypes.Shared.Core;

    interface IPickerDataSource
    {
        IEnumerable<PickerEditorOption> GetEditorOptions();
    }
}
