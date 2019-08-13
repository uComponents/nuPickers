using Newtonsoft.Json;
using nuPickers.DataEditors;

namespace nuPickers.Shared.DataSource
{
    internal static class DataSource
    {
        internal static IDataSource GetDataSource(string propertyEditorAlias, string dataSourceConfig)
        {
            switch (propertyEditorAlias)
            {
                case DataEditorConstants.DotNetCheckBoxPickerAlias:
                case DataEditorConstants.DotNetDropDownPickerAlias:
                case DataEditorConstants.DotNetLabelsAlias:
                case DataEditorConstants.DotNetPagedListPickerAlias:
                case DataEditorConstants.DotNetPrefetchListPickerAlias:
                case DataEditorConstants.DotNetRadioButtonPickerAlias:
                case DataEditorConstants.DotNetTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<DotNetDataSource.DotNetDataSource>(dataSourceConfig);

                case DataEditorConstants.EnumCheckBoxPickerAlias:
                case DataEditorConstants.EnumDropDownPickerAlias:
                case DataEditorConstants.EnumLabelsAlias:
                case DataEditorConstants.EnumPrefetchListPickerAlias:
                case DataEditorConstants.EnumRadioButtonPickerAlias:
                    return JsonConvert.DeserializeObject<EnumDataSource.EnumDataSource>(dataSourceConfig);

                case DataEditorConstants.JsonCheckBoxPickerAlias:
                case DataEditorConstants.JsonDropDownPickerAlias:
                case DataEditorConstants.JsonLabelsAlias:
                case DataEditorConstants.JsonPrefetchListPickerAlias:
                case DataEditorConstants.JsonRadioButtonPickerAlias:
                case DataEditorConstants.JsonTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<JsonDataSource.JsonDataSource>(dataSourceConfig);

                case DataEditorConstants.LuceneCheckBoxPickerAlias:
                case DataEditorConstants.LuceneDropDownPickerAlias:
                case DataEditorConstants.LuceneLabelsAlias:
                case DataEditorConstants.LucenePrefetchListPickerAlias:
                case DataEditorConstants.LuceneRadioButtonPickerAlias:
                case DataEditorConstants.LuceneTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<LuceneDataSource.LuceneDataSource>(dataSourceConfig);

                case DataEditorConstants.RelationLabelsAlias:
                    return JsonConvert.DeserializeObject<RelationDataSource.RelationDataSource>(dataSourceConfig);

                case DataEditorConstants.SqlCheckBoxPickerAlias:
                case DataEditorConstants.SqlDropDownPickerAlias:
                case DataEditorConstants.SqlLabelsAlias:
                case DataEditorConstants.SqlPrefetchListPickerAlias:
                case DataEditorConstants.SqlRadioButtonPickerAlias:
                case DataEditorConstants.SqlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<SqlDataSource.SqlDataSource>(dataSourceConfig);

              /*  case DataEditorConstants.XmlCheckBoxPickerAlias:
                case DataEditorConstants.XmlDropDownPickerAlias:
                case DataEditorConstants.XmlLabelsAlias:
                case DataEditorConstants.XmlPrefetchListPickerAlias:
                case DataEditorConstants.XmlRadioButtonPickerAlias:
                case DataEditorConstants.XmlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<XmlDataSource.XmlDataSource>(dataSourceConfig);*/
            }

            return null;
        }
    }
}