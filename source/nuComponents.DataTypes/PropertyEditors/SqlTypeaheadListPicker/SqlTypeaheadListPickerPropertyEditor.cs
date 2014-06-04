
namespace nuComponents.DataTypes.PropertyEditors.SqlTypeaheadListPicker
{
    using ClientDependency.Core;
    using Umbraco.Core.PropertyEditors;
    using Umbraco.Web.PropertyEditors;
    using nuComponents.DataTypes.PropertyEditors;

    // EDITOR UI
    [PropertyEditor(PropertyEditorConstants.SqlTypeaheadListPickerAlias, "nuComponents: Sql TypeaheadList Picker", EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditor.html", ValueType = "TEXT")]
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditor.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditorController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "ListPicker/ListPickerEditorDirectives.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditorController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "TypeaheadListPicker/TypeaheadListPickerEditorDirectives.js")]

    // RESOURCES
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SaveFormat/SaveFormatResource.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "DataSource/DataSourceResource.js")]

    // CONFIG
    [PropertyEditorAsset(ClientDependencyType.Css, EmbeddedResource.RootUrl + "PropertyEditor/PropertyEditorConfig.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "SqlDataSource/SqlDataSourceConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "CustomLabel/CustomLabelConfigController.js")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, EmbeddedResource.RootUrl + "RelationTypeMapping/RelationTypeMappingConfigController.js")]
    public class SqlTypeaheadListPickerPropertyEditor : BasePropertyEditor
    {
        protected override PreValueEditor CreatePreValueEditor()
        {
            return new SqlTypeaheadListPickerPreValueEditor();
        }
    }
}
