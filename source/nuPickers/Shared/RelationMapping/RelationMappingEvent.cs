
namespace nuPickers.Shared.RelationMapping
{
    using System.Collections.Generic;
    using System.Linq;
    using Umbraco.Core;
    using Umbraco.Core.Events;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;

    /// <summary>
    /// server side event to update any relation mapping on change of any content / media / member using a nuPicker with Relations
    /// </summary>
    public class RelationMappingEvent : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Saved += ContentService_Saved;
            MediaService.Saved += MediaService_Saved;
            MemberService.Saved += MemberService_Saved;

            ContentService.Deleting += ContentService_Deleting;
            MediaService.Deleting += MediaService_Deleting;
            MemberService.Deleting += MemberService_Deleting;
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

        // combined event for content / media / member
        private void Saved(IService sender, IEnumerable<IContentBase> savedEntities)
        {
            foreach (IContentBase savedEntity in savedEntities)
            {
                foreach (PropertyType propertyType in savedEntity.PropertyTypes.Where(x => x.PropertyEditorAlias.StartsWith("nuPickers.")))
                {
                    // does this property type support relations ?
                }
            }
        }

        private void ContentService_Deleting(IContentService sender, DeleteEventArgs<IContent> e)
        {
        }

        private void MediaService_Deleting(IMediaService sender, DeleteEventArgs<IMedia> e)
        {
        }

        private void MemberService_Deleting(IMemberService sender, DeleteEventArgs<IMember> e)
        {
        }
    }
}
