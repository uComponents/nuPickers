
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

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            return Editor.GetEditorDataItems(
                            currentId,
                            parentId,
                            propertyAlias,
                            ((JObject)data.config.dataSource).ToObject<LuceneDataSource>(),
                            (string)data.config.customLabel,
                            (string)data.typeahead);
        }
    }
}
