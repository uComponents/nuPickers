
namespace nuComponents.DataTypes.Interfaces
{
    using System.Collections.Generic;
    using nuComponents.DataTypes.Shared.Picker;

    /// <summary>
    /// every picker calls an api controller to get it's data 
    /// </summary>
    internal interface IPickerApiController
    {        
        /// <summary>
        /// this is called by the PickerResource.js that's used by all picker editors
        /// </summary>
        /// <param name="contextId">the id of the current content / media or member being edited</param>
        /// <param name="config">the full $scope.model.config as saved by Umbraco for the current datatype instance</param>
        /// <returns></returns>
        IEnumerable<PickerEditorOption> GetEditorOptions(int contextId, dynamic config);
    }
}
