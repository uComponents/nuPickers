namespace nuPickers.Shared.RelationMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class RelationMappingApiController : UmbracoAuthorizedJsonController
    {
        [HttpGet]
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

        [HttpGet]
        public IEnumerable<int> GetRelatedIds([FromUri] int contextId, [FromUri] string propertyAlias, [FromUri] string relationTypeAlias, [FromUri] bool relationsOnly)
        {
            return RelationMapping.GetRelatedIds(contextId, propertyAlias, relationTypeAlias, relationsOnly);
        }
    }
}
