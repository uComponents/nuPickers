namespace nuPickers.Shared.LuceneDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using UmbracoExamine;

    [PluginController("nuPickers")]
    public class LuceneDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetExamineSearchers()
        {
            return Examine.ExamineManager.Instance.SearchProviderCollection.OfType<UmbracoExamineSearcher>().Select(x => x.Name);            
        }
    }
}