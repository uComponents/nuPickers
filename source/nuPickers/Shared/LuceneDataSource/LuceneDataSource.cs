using Examine.Search;
using Umbraco.Examine;

namespace nuPickers.Shared.LuceneDataSource
{
    using DataSource;
    using Examine;
    using Examine.Providers;
    using nuPickers.Shared.Editor;
    using Paging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class LuceneDataSource : IDataSource
    {

        private readonly IExamineManager _examineManager;

        public LuceneDataSource(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        public string ExamineSearcher { get; set; }

        public string RawQuery { get; set; }

        public string KeyField { get; set; }

        public string LabelField { get; set; }

        bool IDataSource.HandledTypeahead { get { return false; } } // TODO: Implement token replacement for Lucene queries

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems(currentId).Where(x => keys.Contains(x.Key));
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = this.GetEditorDataItems(currentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        private IEnumerable<EditorDataItem> GetEditorDataItems(int contextId)
        {
            List<EditorDataItem> editorDataItems = new List<EditorDataItem>();

            ISearcher searchProvider = _examineManager.TryGetIndex(ExamineSearcher, out var externalIndex) ? externalIndex.GetSearcher() : null;

            if (searchProvider != null)
            {
                IQuery searchCriteria = searchProvider.CreateQuery();
                var query = searchCriteria.NativeQuery(this.RawQuery);
                ISearchResults searchResults = query.Execute();

                foreach (SearchResult searchResult in searchResults)
                {
                    editorDataItems.Add(
                        new EditorDataItem()
                            {
                                Key = searchResult.AllValues.ContainsKey(this.KeyField) ? searchResult.AllValues[this.KeyField][0] : null,
                                Label = searchResult.AllValues.ContainsKey(this.LabelField) ? searchResult.AllValues[this.LabelField][0] : null
                            });
                }
            }

            return editorDataItems;
        }
    }
}