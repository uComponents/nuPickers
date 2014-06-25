
namespace nuComponents.DataTypes.Shared.LuceneDataSource
{
    using Examine;
    using Examine.Providers;
    using Examine.SearchCriteria;
    using nuComponents.DataTypes.Shared.Editor;
    using System.Collections.Generic;

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
    }
}
