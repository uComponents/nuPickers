using System;
using System.ComponentModel;
using System.Linq;
using nuPickers.Caching;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace nuPickers.Components
{
    public class CacheInvalidationComponent : IComponent, Umbraco.Core.Composing.IComponent
    {
        private ContentService _contentService;
        private DataTypeService _dataTypeService;

        public CacheInvalidationComponent(ContentService contentService, DataTypeService dataTypeService)
        {
            _contentService = contentService;
            _dataTypeService = dataTypeService;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;

        public void Initialize()
        {
            ContentService.Saved += this.ContentService_Saved;
            ContentService.Deleted += this.ContentService_Deleted;

            DataTypeService.Saved += this.DataTypeService_Saved;
            DataTypeService.Deleted += this.DataTypeService_Deleted;
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }

        private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            e.SavedEntities.ToList().ForEach(x =>
                Cache.Instance.Remove(y => y.StartsWith(CacheConstants.PickedKeysPrefix + x.Id.ToString())));
        }

        private void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            e.DeletedEntities.ToList().ForEach(x =>
                Cache.Instance.Remove(y => y.StartsWith(CacheConstants.PickedKeysPrefix + x.Id.ToString())));
        }

        private void DataTypeService_Saved(IDataTypeService sender, SaveEventArgs<IDataType> e)
        {
            e.SavedEntities.ToList().ForEach(
                x => Cache.Instance.Remove(CacheConstants.DataTypePreValuesPrefix + x.Id.ToString()));
        }

        private void DataTypeService_Deleted(IDataTypeService sender, DeleteEventArgs<IDataType> e)
        {
            e.DeletedEntities.ToList().ForEach(x =>
                Cache.Instance.Remove(CacheConstants.DataTypePreValuesPrefix + x.Id.ToString()));
        }
    }
}