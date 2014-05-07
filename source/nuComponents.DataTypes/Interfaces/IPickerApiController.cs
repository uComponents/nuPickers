
namespace nuComponents.DataTypes.Interfaces
{
    using System.Collections.Generic;
    using nuComponents.DataTypes.Shared.Core;

    /// <summary>
    /// every picker calls an api controller to get it's data 
    /// </summary>
    internal interface IPickerApiController
    {
        /// <summary>
        /// this is called by the PickerResource.js that's used by all picker editors
        /// </summary>
        /// <param name="config">the full $scope.model.config as saved by Umbraco for the current datatype instance</param>
        /// <returns>a collection of </returns>        
        IEnumerable<PickerEditorOption> GetEditorOptions(dynamic config);
    }
}
