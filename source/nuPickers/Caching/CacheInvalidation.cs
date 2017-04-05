namespace nuPickers.Caching
{
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Services;
    using Umbraco.Core.Models;

    public class CacheInvalidation : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saved += this.ContentService_Saved;
            ContentService.Deleted += this.ContentService_Deleted;

            DataTypeService.Saved += this.DataTypeService_Saved;
            DataTypeService.Deleted += this.DataTypeService_Deleted;
        }

        private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            e.SavedEntities.ForEach(x => Cache.Instance.Remove(y => y.StartsWith(CacheConstants.PickedKeysPrefix + x.Id.ToString())));
        }

        private void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            e.DeletedEntities.ForEach(x => Cache.Instance.Remove(y => y.StartsWith(CacheConstants.PickedKeysPrefix + x.Id.ToString())));
        }

        private void DataTypeService_Saved(IDataTypeService sender, SaveEventArgs<IDataTypeDefinition> e)
        {
            e.SavedEntities.ForEach(x => Cache.Instance.Remove(CacheConstants.DataTypePreValuesPrefix + x.Id.ToString()));
        }

        private void DataTypeService_Deleted(IDataTypeService sender, DeleteEventArgs<IDataTypeDefinition> e)
        {
            e.DeletedEntities.ForEach(x => Cache.Instance.Remove(CacheConstants.DataTypePreValuesPrefix + x.Id.ToString()));
        }
    }
}