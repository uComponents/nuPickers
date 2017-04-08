namespace nuPickers.Shared.RelationDataSource
{
    using DataSource;
    using Editor;
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Web;

    public class RelationDataSource : IDataSource
    {
        public string RelationType { get; set; }

        public bool HandledTypeahead {  get { return false; } }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string typeahead)
        {
            return this.GetEditorDataItems(currentId, parentId);
        }

        public IEnumerable<EditorDataItem> GetEditorDataItems(int currentId, int parentId, string[] keys)
        {
            return Enumerable.Empty<EditorDataItem>();
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