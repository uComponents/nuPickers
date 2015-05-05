
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

            if (!string.IsNullOrWhiteSpace(this.RawQuery))
            {
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
                                Key = searchResult.Fields.ContainsKey(this.KeyField) ? searchResult.Fields[this.KeyField] : null,
                                Label = searchResult.Fields.ContainsKey(this.LabelField) ? searchResult.Fields[this.LabelField] : null
                            });
                }
            }
            }

            return editorDataItems;
        }
    }
}
