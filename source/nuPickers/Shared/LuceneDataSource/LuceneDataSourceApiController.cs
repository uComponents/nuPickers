using Umbraco.Examine;

namespace nuPickers.Shared.LuceneDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class LuceneDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetExamineSearchers()
        {
            var configuredSearchers = Examine.ExamineManager.Instance.RegisteredSearchers.Select(x => x.Name).ToList();
            var indexSearchers = Examine.ExamineManager.Instance.Indexes.Select(x => x.GetSearcher().Name).ToList();

            return configuredSearchers.Concat(indexSearchers);
        }
    }
}