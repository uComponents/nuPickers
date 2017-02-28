
namespace nuPickers.Shared.LuceneDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System;
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
            return GetEditorDataItems(currentId, parentId, propertyAlias, null, data);
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromUri] string ids, [FromBody] dynamic data)
        {

            int contextId = currentId;

            LuceneDataSource luceneDataSource = ((JObject)data.config.dataSource).ToObject<LuceneDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = luceneDataSource.GetEditorDataItems(contextId);

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            // if there are ids then ignore typeahead
            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => collectionIds.Contains(x.Key)).OrderBy(x => Array.FindIndex(collectionIds.ToArray(), y => y == x.Key));
                editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
            }
            else
            {
                // check whether typeahead should query the dataItems after processing them with the custom label
                bool isTypeaheadQueryOnCustomLabels = false;
                if (data.config.typeaheadListPicker != null && data.config.typeaheadListPicker.queryOnCustomLabels != null)
                {
                    bool.TryParse((string)data.config.typeaheadListPicker.queryOnCustomLabels, out isTypeaheadQueryOnCustomLabels);
                }

                if (isTypeaheadQueryOnCustomLabels)
                {
                    editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
                }

                // handle type ahead text
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);
                editorDataItems = typeaheadListPicker.ProcessEditorDataItems(editorDataItems, isTypeaheadQueryOnCustomLabels);

                if (!isTypeaheadQueryOnCustomLabels)
                {
                    editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);
                }

            }

            return editorDataItems;
        }
    }
}
