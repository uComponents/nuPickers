namespace nuComponents.DataTypes.Shared
{
    using System.Collections.Generic;
    using System.Linq;
    using umbraco.cms.businesslogic.macro;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class CustomLabelApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetMacros()
        {
            //using legacy api as no method on Umbraco.Core.Services.MacroSerivce to get all macros
            return Macro.GetAll()
                        .Where(x => x.Properties.Any(y => y.Alias == "key"))
                        .Select(x => new
                        {
                            name = x.Name,
                            alias = x.Alias
                        });
        }
    }
}
