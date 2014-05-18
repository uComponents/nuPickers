
namespace nuComponents.DataTypes.Shared.RelationMatches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using umbraco.cms.businesslogic.relation;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuComponents.DataTypes.Shared.Picker;

    [PluginController("nuComponents")]
    public class RelationMatchesApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            return RelationType.GetAll()
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            key = x.Alias,
                            label = x.Name,
                            biDirectional = x.Dual
                        });
        }

        
        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromBody] dynamic config, [FromUri] int contextId)
        {
            List<PickerEditorOption> pickerEditorOptions = new List<PickerEditorOption>();

            // TODO:

            return pickerEditorOptions;
        }
    }
}
