using Newtonsoft.Json;
using nuPickers.PropertyEditors;

namespace nuPickers.Shared.DataSource
{
    internal static class DataSource
    {
        internal static IDataSource GetDataSource(string propertyEditorAlias, string dataSourceConfig)
        {
            switch (propertyEditorAlias)
            {
                case PropertyEditorConstants.DotNetCheckBoxPickerAlias:
                case PropertyEditorConstants.DotNetDropDownPickerAlias:
                case PropertyEditorConstants.DotNetLabelsAlias:
                case PropertyEditorConstants.DotNetPagedListPickerAlias:
                case PropertyEditorConstants.DotNetPrefetchListPickerAlias:
                case PropertyEditorConstants.DotNetRadioButtonPickerAlias:
                case PropertyEditorConstants.DotNetTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<DotNetDataSource.DotNetDataSource>(dataSourceConfig);

                case PropertyEditorConstants.EnumCheckBoxPickerAlias:
                case PropertyEditorConstants.EnumDropDownPickerAlias:
                case PropertyEditorConstants.EnumLabelsAlias:
                case PropertyEditorConstants.EnumPrefetchListPickerAlias:
                case PropertyEditorConstants.EnumRadioButtonPickerAlias:
                    return JsonConvert.DeserializeObject<EnumDataSource.EnumDataSource>(dataSourceConfig);

                case PropertyEditorConstants.JsonCheckBoxPickerAlias:
                case PropertyEditorConstants.JsonDropDownPickerAlias:
                case PropertyEditorConstants.JsonLabelsAlias:
                case PropertyEditorConstants.JsonPrefetchListPickerAlias:
                case PropertyEditorConstants.JsonRadioButtonPickerAlias:
                case PropertyEditorConstants.JsonTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<JsonDataSource.JsonDataSource>(dataSourceConfig);

                case PropertyEditorConstants.LuceneCheckBoxPickerAlias:
                case PropertyEditorConstants.LuceneDropDownPickerAlias:
                case PropertyEditorConstants.LuceneLabelsAlias:
                case PropertyEditorConstants.LucenePrefetchListPickerAlias:
                case PropertyEditorConstants.LuceneRadioButtonPickerAlias:
                case PropertyEditorConstants.LuceneTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<LuceneDataSource.LuceneDataSource>(dataSourceConfig);

                case PropertyEditorConstants.RelationLabelsAlias:
                    return JsonConvert.DeserializeObject<RelationDataSource.RelationDataSource>(dataSourceConfig);

                case PropertyEditorConstants.SqlCheckBoxPickerAlias:
                case PropertyEditorConstants.SqlDropDownPickerAlias:
                case PropertyEditorConstants.SqlLabelsAlias:
                case PropertyEditorConstants.SqlPrefetchListPickerAlias:
                case PropertyEditorConstants.SqlRadioButtonPickerAlias:
                case PropertyEditorConstants.SqlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<SqlDataSource.SqlDataSource>(dataSourceConfig);

              /*  case PropertyEditorConstants.XmlCheckBoxPickerAlias:
                case PropertyEditorConstants.XmlDropDownPickerAlias:
                case PropertyEditorConstants.XmlLabelsAlias:
                case PropertyEditorConstants.XmlPrefetchListPickerAlias:
                case PropertyEditorConstants.XmlRadioButtonPickerAlias:
                case PropertyEditorConstants.XmlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<XmlDataSource.XmlDataSource>(dataSourceConfig);*/
            }

            return null;
        }
    }
}