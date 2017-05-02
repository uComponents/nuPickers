namespace nuPickers.Shared.LuceneDataSource
{
    using DataSource;
    using Examine;
    using Examine.Providers;
    using Examine.SearchCriteria;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;

    public class LuceneDataSource : IDataSource
    {
        public string ExamineSearcher { get; set; }

        public string RawQuery { get; set; }
        
        public string KeyField { get; set; }
        
        public string LabelField { get; set; }

        public bool HandledTypeahead { get { return false; } } // TODO: Implement token replacement for Lucene queries

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems(currentId).Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, int skip, int take, out int total)
        {
            var editorDataItems = this.GetEditorDataItems(currentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(skip).Take(take);
        }

        private IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
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