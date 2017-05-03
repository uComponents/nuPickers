namespace nuPickers.Shared.RelationDataSource
{
    using DataSource;
    using Editor;
    using Paging;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Web;

    public class RelationDataSource : IDataSource
    {
        public string RelationType { get; set; }

        bool IDataSource.HandledTypeahead {  get { return false; } }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId, parentId);
        }

        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return this.GetEditorDataItems(currentId, parentId).Where(x => keys.Contains(x.Key));
        }
        IEnumerable<EditorDataItem> IDataSource.GetEditorDataItems(int currentId, int parentId, PageMarker pageMarker, out int total)
        {
            var editorDataItems = this.GetEditorDataItems(currentId, parentId);

            total = editorDataItems.Count();

            return editorDataItems.Skip(pageMarker.Skip).Take(pageMarker.Take);
        }

        private IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId)
        {
            var relationService = ApplicationContext.Current.Services.RelationService;

            return relationService.GetEntitiesFromRelations(
                                                relationService.GetByRelationTypeAlias(this.RelationType)
                                                .Where(r => r.ParentId == currentId))
                                                .Select(x => new EditorDataItem()
                                                {
                                                    Key = x.Item2.Id.ToString(),
                                                    Label = x.Item2.Name.ToString()
                                                });
        }
    }
}