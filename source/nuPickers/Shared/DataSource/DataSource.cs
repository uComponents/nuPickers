namespace nuPickers.Shared.DataSource
{
    using Newtonsoft.Json;
    using nuPickers.PropertyEditors;
    using nuPickers.Shared.DotNetDataSource;
    using nuPickers.Shared.EnumDataSource;
    using nuPickers.Shared.JsonDataSource;
    using nuPickers.Shared.LuceneDataSource;
    using nuPickers.Shared.RelationDataSource;
    using nuPickers.Shared.SqlDataSource;
    using nuPickers.Shared.XmlDataSource;

    internal static class DataSource
    {
        internal static IDataSource GetDataSource(string propertyEditorAlias, string dataSourceConfig)
        {            
            switch (propertyEditorAlias)
            {
                case PropertyEditorConstants.DotNetCheckBoxPickerAlias:
                case PropertyEditorConstants.DotNetDropDownPickerAlias:
                case PropertyEditorConstants.DotNetLabelsAlias:
                case PropertyEditorConstants.DotNetPrefetchListPickerAlias:
                case PropertyEditorConstants.DotNetRadioButtonPickerAlias:
                case PropertyEditorConstants.DotNetTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<DotNetDataSource>(dataSourceConfig);

                case PropertyEditorConstants.EnumCheckBoxPickerAlias:
                case PropertyEditorConstants.EnumDropDownPickerAlias:
                case PropertyEditorConstants.EnumLabelsAlias:
                case PropertyEditorConstants.EnumPrefetchListPickerAlias:
                case PropertyEditorConstants.EnumRadioButtonPickerAlias:
                    return JsonConvert.DeserializeObject<EnumDataSource>(dataSourceConfig);

                case PropertyEditorConstants.JsonCheckBoxPickerAlias:
                case PropertyEditorConstants.JsonDropDownPickerAlias:
                case PropertyEditorConstants.JsonLabelsAlias:
                case PropertyEditorConstants.JsonPrefetchListPickerAlias:
                case PropertyEditorConstants.JsonRadioButtonPickerAlias:
                case PropertyEditorConstants.JsonTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<JsonDataSource>(dataSourceConfig);

                case PropertyEditorConstants.LuceneCheckBoxPickerAlias:
                case PropertyEditorConstants.LuceneDropDownPickerAlias:
                case PropertyEditorConstants.LuceneLabelsAlias:
                case PropertyEditorConstants.LucenePrefetchListPickerAlias:
                case PropertyEditorConstants.LuceneRadioButtonPickerAlias:
                case PropertyEditorConstants.LuceneTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<LuceneDataSource>(dataSourceConfig);

                case PropertyEditorConstants.RelationLabelsAlias:
                    return JsonConvert.DeserializeObject<RelationDataSource>(dataSourceConfig);                   

                case PropertyEditorConstants.SqlCheckBoxPickerAlias:
                case PropertyEditorConstants.SqlDropDownPickerAlias:
                case PropertyEditorConstants.SqlLabelsAlias:
                case PropertyEditorConstants.SqlPrefetchListPickerAlias:
                case PropertyEditorConstants.SqlRadioButtonPickerAlias:
                case PropertyEditorConstants.SqlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<SqlDataSource>(dataSourceConfig);

                case PropertyEditorConstants.XmlCheckBoxPickerAlias:
                case PropertyEditorConstants.XmlDropDownPickerAlias:
                case PropertyEditorConstants.XmlLabelsAlias:
                case PropertyEditorConstants.XmlPrefetchListPickerAlias:
                case PropertyEditorConstants.XmlRadioButtonPickerAlias:
                case PropertyEditorConstants.XmlTypeaheadListPickerAlias:
                    return JsonConvert.DeserializeObject<XmlDataSource>(dataSourceConfig);
            }

            return null;
        }
    }
}