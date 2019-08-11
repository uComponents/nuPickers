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

            return Current.Services.MacroService.GetAll()
                        .Select(x => new
                        {
                            name = x.Name,
                            alias = x.Alias
                        });
        }
    }
}
