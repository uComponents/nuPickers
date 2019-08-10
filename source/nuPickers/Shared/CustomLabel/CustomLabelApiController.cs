using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web.Composing;

namespace nuPickers.Shared
{
    using System.Collections.Generic;

    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class CustomLabelApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetMacros()
        {
            //using legacy api as no method on Umbraco.Core.Services.MacroSerivce to get all macros
            return Current.Services.MacroService.GetAll()
                        .Select(x => new
                        {
                            name = x.Name,
                            alias = x.Alias
                        });
        }
    }
}
