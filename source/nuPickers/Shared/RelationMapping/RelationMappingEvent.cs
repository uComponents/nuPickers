
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

        /// <summary>
        /// combined event for content / media / member 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="savedEntities"></param>
        private void Saved(IService sender, IEnumerable<IContentBase> savedEntities)
        {
            foreach (IContentBase savedEntity in savedEntities)
            {
                foreach (PropertyType propertyType in savedEntity.PropertyTypes
                                                            .Where(x => PickerPropertyValueConverter.IsPicker(x.PropertyEditorAlias)))
                {
                    // using picker to get at the prevalues TODO: consider passing this picker obj to the UpdateRelationMapping method
                    Picker picker = new Picker(savedEntity.Id, propertyType.Alias, propertyType.DataTypeDefinitionId, savedEntity.GetValue(propertyType.Alias));

                    // not all pickers support relation mapping, so null check required
                    PreValue relationMappingPreValue = picker.GetDataTypePreValue("relationMapping");
                    if (relationMappingPreValue != null && !string.IsNullOrWhiteSpace(relationMappingPreValue.Value))
                    {
                        // parse the json config to get a relationType alias
                        string relationTypeAlias = JObject.Parse(relationMappingPreValue.Value).GetValue("relationTypeAlias").ToString();

                        // if relationType specified
                        if (!string.IsNullOrWhiteSpace(relationTypeAlias))
                        {
                            // is the picker saving as relations only ?
                            // every picker has a save format, so null check NOT required
                            bool relationsOnly = picker.GetDataTypePreValue("saveFormat").Value == "relationsOnly";

                            int pickedId;
                            List<int> pickedIds = new List<int>();
                            foreach(string pickedKey in picker.PickedKeys)
                            {
                                if (int.TryParse(pickedKey, out pickedId))
                                {
                                    pickedIds.Add(pickedId);
                                }
                            }

                            RelationMapping.UpdateRelationMapping(
                                                savedEntity.Id,
                                                propertyType.Alias,
                                                relationTypeAlias,
                                                relationsOnly,
                                                pickedIds.ToArray());
                        }
                    }
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
