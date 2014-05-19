
namespace nuComponents.DataTypes.Shared.RelationMatches
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using umbraco.cms.businesslogic.relation;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuComponents.DataTypes.Shared.Picker;
    using umbraco;
    using CustomLabel;

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
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromUri] int contextId, [FromBody] dynamic config)
        {
            IEnumerable<PickerEditorOption> pickerEditorOptions = RelationType.GetByAlias((string)config.relationMatches)
                                                                                .GetRelations(contextId)
                                                                                .Select(x => new PickerEditorOption()  { 
                                                                                                        Key = x.Child.Id.ToString(), 
                                                                                                        Label = x.Child.Text 
                                                                                                    })
                                                                                .ToList();

            CustomLabel customLabel = new CustomLabel((string)config.customLabel, contextId);

            return customLabel.ProcessPickerEditorOptions(pickerEditorOptions);
        }
    }
}
