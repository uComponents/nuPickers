using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using nuPickers.Caching;
using nuPickers.Shared.RelationMapping;
using nuPickers.Shared.SaveFormat;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.Services;
using Umbraco.Core.Services.Implement;

namespace nuPickers.Components
{
    public class RelationMappingComponent
    {
          private ContentService _contentService;
        private DataTypeService _dataTypeService;

        public RelationMappingComponent(ContentService contentService, DataTypeService dataTypeService)
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
            MediaService.Saved += this.MediaService_Saved;
            MemberService.Saved += this.MemberService_Saved;
        }

        public void Terminate()
        {

        }

       private void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            this.Saved((IService)sender, e.SavedEntities);
        }

        private void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            this.Saved((IService)sender, e.SavedEntities);
        }

        private void MemberService_Saved(IMemberService sender, SaveEventArgs<IMember> e)
        {
            this.Saved((IService)sender, e.SavedEntities);
        }

        /// <summary>
        /// combined event for content / media / member
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="savedEntities"></param>
        private void Saved(IService sender, IEnumerable<IContentBase> savedEntities)
        {
            foreach (IContentBase savedEntity in savedEntities)
            {
                // for each property
                foreach (IPublishedProperty propertyType in savedEntity.Properties.Where(x => PickerPropertyValueConverter.IsPicker(x.PropertyType.PropertyEditorAlias)))
                {
                    // create picker supplying all values
                    Picker picker = new Picker(
                                            savedEntity.Id,
                                            savedEntity.ParentId,
                                            propertyType.PropertyType.Alias,
                                            propertyType.PropertyType.DataType.Id,
                                            propertyType.PropertyType.EditorAlias,
                                            savedEntity.GetValue(propertyType.Alias));

                    if (!string.IsNullOrWhiteSpace(picker.RelationTypeAlias))
                    {
                        bool isRelationsOnly = picker.GetEditorDataItems().GetType().GetProperty("saveFormat").ToString() == "relationsOnly";

                        if (isRelationsOnly)
                        {
                            if (picker.SavedValue == null)
                            {
                                picker.PickedKeys = new string[] { };
                            }
                            else
                            {
                                // manually set on picker obj, so it doesn't then attempt to read picked keys from the database
                                picker.PickedKeys = SaveFormat.GetKeys(picker.SavedValue.ToString()).ToArray();

                                // delete saved value (setting it to null)
                                savedEntity.SetValue(propertyType.Alias, null);

                                if (sender is IContentService)
                                {
                                    ((IContentService)sender).Save((IContent)savedEntity, 0, false);
                                }
                                else if (sender is IMediaService)
                                {
                                    ((IMediaService)sender).Save((IMedia)savedEntity, 0, false);
                                }
                                else if (sender is IMemberService)
                                {
                                    ((IMemberService)sender).Save((IMember)savedEntity, false);
                                }
                            }
                        }

                        // update database
                        RelationMapping.UpdateRelationMapping(
                                                picker.ContextId,           // savedEntity.Id
                                                picker.PropertyAlias,       // propertyType.Alias
                                                picker.RelationTypeAlias,
                                                isRelationsOnly,
                                                picker.PickedIds.ToArray());
                    }
                }
            }
        }
    }
}