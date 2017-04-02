
namespace nuPickers.Shared.LuceneDataSource
{
    using DataSource;
    using Examine;
    using Examine.Providers;
    using Examine.SearchCriteria;
    using nuPickers.Shared.Editor;
    using System;
    using System.Collections.Generic;

    public class LuceneDataSource : IDataSource
    {
        public string ExamineSearcher { get; set; }

        public string RawQuery { get; set; }
        
        public string KeyField { get; set; }
        
        public string LabelField { get; set; }

        public string Typeahead { set { /* ignore */ } } // TODO: Implement token replacement for Lucene queries

        public bool HandledTypeahead { get { return false; } } // TODO: Implement token replacement for Lucene queries

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId)
        {
            return this.GetEditorDataItems(currentId);
        }

        [Obsolete]
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
                                Key = searchResult.Fields.ContainsKey(this.KeyField) ? searchResult.Fields[this.KeyField] : null,
                                Label = searchResult.Fields.ContainsKey(this.LabelField) ? searchResult.Fields[this.LabelField] : null
                            });
                }
            }

            return editorDataItems;
        }
    }
}
