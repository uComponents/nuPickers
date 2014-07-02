
namespace nuPickers.Shared.DotNetDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class DotNetDataSourceApiController : UmbracoAuthorizedJsonController
    {
        public IEnumerable<object> GetAssemblyNames()
        {
            return Helper.GetAssemblyNames();
        }

        public IEnumerable<object> GetClassNames([FromUri]string assemblyName)
        {
            return null;
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromBody] dynamic data)
        {
            DotNetDataSource dotNetDataSource = ((JObject)data.config.dataSource).ToObject<DotNetDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = dotNetDataSource.GetEditorDataItems();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId);

            return customLabel.ProcessEditorDataItems(editorDataItems);
        }

    }
}
