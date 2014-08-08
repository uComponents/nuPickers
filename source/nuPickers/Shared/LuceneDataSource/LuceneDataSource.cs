
namespace nuPickers.Shared.LuceneDataSource
{
    using Examine;
    using Examine.Providers;
    using Examine.SearchCriteria;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;

    public class LuceneDataSource
    {
        public string ExamineSearcher { get; set; }

        public string RawQuery { get; set; }
        
        public string KeyField { get; set; }
        
        public string LabelField { get; set; }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            BaseSearchProvider searchProvider = ExamineManager.Instance.SearchProviderCollection[this.ExamineSearcher];

            if (searchProvider != null)
            {
                ISearchCriteria searchCriteria = searchProvider.CreateSearchCriteria().RawQuery(this.RawQuery);
                ISearchResults searchResults = searchProvider.Search(searchCriteria);

                foreach (SearchResult searchResult in searchResults)
                {
                    editorDataItems.Add(
                        new EditorDataItem() 
                            { 
                                Key = searchResult.Fields[this.KeyField],
                                Label = searchResult.Fields[this.LabelField]
                            });
                }
            }

            return editorDataItems;
        }

        public IEnumerable<EditorDataItem> GetEditorDataItemsFilteredByIds(int contextId, string ids)
        {
            List<EditorDataItem> result = new List<EditorDataItem>();
            if (ids != null)
            {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                result = GetEditorDataItems(contextId).Where(x => ids.Contains(x.Key)).ToList<EditorDataItem>();
            }
            return result;
        }


    }
}
