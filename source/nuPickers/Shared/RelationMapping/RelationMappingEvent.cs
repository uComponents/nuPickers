
namespace nuPickers.Shared.RelationMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// server side event to update relations on change of any content / media / member using a nuPicker with relation mapping
    /// </summary>
    public class RelationMappingEvent : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saved += this.ContentService_Saved;
            MediaService.Saved += this.MediaService_Saved;
            MemberService.Saved += this.MemberService_Saved;

            //ContentService.Trashed += this.ContentService_Trashed;
            //MediaService.Trashed += this.MediaService_Trashed;
            // Members can't be trashed
            
            // NOTE: all relations to an id are automatically deleted when emptying the recycle bin
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
                foreach (PropertyType propertyType in savedEntity.PropertyTypes.Where(x => PickerPropertyValueConverter.IsPicker(x.PropertyEditorAlias)))
                {
                    // create picker supplying all values
                    Picker picker = new Picker(savedEntity.Id, 
                                                propertyType.Alias, 
                                                propertyType.DataTypeDefinitionId, 
                                                savedEntity.GetValue(propertyType.Alias));

                    if (!string.IsNullOrWhiteSpace(picker.RelationTypeAlias))
                    {
                        RelationMapping.UpdateRelationMapping(
                                                picker.ContextId,           // savedEntity.Id
                                                picker.PropertyAlias,       // propertyType.Alias
                                                picker.RelationTypeAlias,
                                                picker.GetDataTypePreValue("saveFormat").Value == "relationsOnly",
                                                picker.PickedIds.ToArray());
                    }
                }
            }
        }

        //private void MediaService_Trashed(IMediaService sender, MoveEventArgs<IMedia> e)
        //{
        //    this.Trashed((IService)sender, e.Entity);
        //}

        //private void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        //{
        //    this.Trashed((IService)sender, e.Entity);
        //}

        ///// <summary>
        ///// combined event for content / media
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="entity"></param>
        //private void Trashed(IService sender, IContentBase trashedEntity)
        //{
        //    // for each property
        //    foreach (PropertyType propertyType in trashedEntity.PropertyTypes.Where(x => PickerPropertyValueConverter.IsPicker(x.PropertyEditorAlias)))
        //    {
        //        // create picker supplying all values
        //        Picker picker = new Picker(trashedEntity.Id,
        //                                    propertyType.Alias,
        //                                    propertyType.DataTypeDefinitionId,
        //                                    trashedEntity.GetValue(propertyType.Alias));

        //        if (!string.IsNullOrWhiteSpace(picker.RelationTypeAlias))
        //        {
        //            // trigger update with no items picked - causes relations to be deleted
        //            // (relations can be recreated after a delete, by resaving after restore)
        //            RelationMapping.UpdateRelationMapping(
        //                                    picker.ContextId,
        //                                    picker.PropertyAlias,
        //                                    picker.RelationTypeAlias,
        //                                    picker.GetDataTypePreValue("saveFormat").Value == "relationsOnly",
        //                                    new int[] {});
        //        }
        //    }
        //}
    }
}
