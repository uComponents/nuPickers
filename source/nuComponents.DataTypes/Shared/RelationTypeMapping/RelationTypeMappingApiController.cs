namespace nuComponents.DataTypes.Shared.RelationTypeMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class RelationTypeMappingApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            return ApplicationContext.Services.RelationService.GetAllRelationTypes()
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            key = x.Alias,
                            label = x.Name,
                            bidirectional = x.IsBidirectional                           
                        });
        }
    }
}
