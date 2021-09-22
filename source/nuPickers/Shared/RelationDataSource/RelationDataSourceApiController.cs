using Umbraco.Core.Composing;
using Umbraco.Core.Models;

namespace nuPickers.Shared.RelationDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class RelationDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetRelationTypes()
        {
            var relationService = Current.Services.RelationService;
            return relationService.GetAllRelationTypes()
                        .OrderBy(x => x.Name)
                        .Select(x => new
                        {
                            key = x.Alias,
                            label = x.Name,
                            biDirectional = x.IsBidirectional
                        });
        }
    }
}