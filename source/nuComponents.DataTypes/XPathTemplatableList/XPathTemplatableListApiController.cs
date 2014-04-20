namespace nuComponents.DataTypes.XPathTemplatableList
{
    using System.Collections.Generic;
    using System.Linq;
    using umbraco.cms.businesslogic.macro;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using Umbraco.Core.Models;

    [PluginController("nuComponents")]
    public class XPathTemplatableListApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetMacros()
        {
            //using legacy api as no method on Umbraco.Core.Services.MacroSerivce to get all macros
            return Macro.GetAll().Select(x => new { 
                                                name = x.Name, 
                                                alias = x.Alias,
                                                valid = x.Properties.Any(y => y.Alias == "id")
            });
        }

        public IEnumerable<object> GetScriptFiles()
        {
            return this.Services
                        .FileService
                        .GetScripts()
                        .Where(x => x.Path.EndsWith(".js"))
                        .Select(x => new {
                            path = x.Path
                        });
        }
    }
}
