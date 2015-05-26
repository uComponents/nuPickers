namespace nuPickers.Shared.UmbracoTreeDataSource
{
    using System.Collections.Generic;
    using System.Linq;
    using nuPickers.Shared.DotNetDataSource;

    public class UmbracoTreeDataSource : IDotNetDataSource
    {
        public string SectionAlias { get; set; }

        public string TreeAlias { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GetEditorDataItems(int contextId)
        {
            // TODO: Funnel to Umbraco.Web.Trees.ApplicationTreeController.GetApplicationTrees
            return Enumerable.Empty<KeyValuePair<string, string>>();
        }
    }
}
