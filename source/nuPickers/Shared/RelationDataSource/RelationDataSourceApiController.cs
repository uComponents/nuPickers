namespace nuPickers.Shared.RelationDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using umbraco.cms.businesslogic.relation;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class RelationDataSourceApiController : UmbracoAuthorizedJsonController
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