
using System;

namespace nuComponents.DataTypes.Shared.XmlDataSource
{
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.CustomLabel;
    using nuComponents.DataTypes.Shared.Editor;
    using nuComponents.DataTypes.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponents")]
    public class XmlDataSourceApiController : UmbracoAuthorizedJsonController
    {
        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromBody] dynamic data)
        {
            try
            {
                XmlDataSource xmlDataSource = ((JObject) data.config.dataSource).ToObject<XmlDataSource>();

                IEnumerable<EditorDataItem> editorDataItems = xmlDataSource.GetEditorDataItems(contextId);

                CustomLabel customLabel = new CustomLabel((string) data.config.customLabel, contextId);
                TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string) data.typeahead);

                // process the labels and then handle any type ahead text
                return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
            }  
            catch (Exception e)
            {
                Helper.Logs.LogExeption<XmlDataSourceApiController>("Error getting datasource data", e);
                throw e;
            }
        }
    }
}
