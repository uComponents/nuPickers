using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.Composing;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace nuPickers.Shared.CustomLabel
{
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
