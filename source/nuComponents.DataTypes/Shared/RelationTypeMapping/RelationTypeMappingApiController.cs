
namespace nuComponents.DataTypes.Shared.RelationTypeMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using umbraco.cms.businesslogic.relation;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class RelationTypeMappingApiController : UmbracoAuthorizedJsonController
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
    }
}
