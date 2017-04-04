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
            DataTypeService.Saved += DataTypeService_Saved;
            DataTypeService.Deleted += DataTypeService_Deleted;
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