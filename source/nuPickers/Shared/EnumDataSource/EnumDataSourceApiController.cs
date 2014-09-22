namespace nuPickers.Shared.EnumDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuPickers")]
    public class EnumDataSourceApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Gets the names of all assemblies and optional AppCode folder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAssemblyNames()
        {
            return Helper.GetAssemblyNames().Where(x => Helper.GetAssembly(x).GetTypes().Any(y => y.IsEnum));
        }


        public IEnumerable<object> GetEnumNames([FromUri]string assemblyName)
        {
            Assembly assembly = Helper.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly.GetTypes().Where(x => x.IsEnum).Select(x => x.FullName);
            }

            return null;
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            EnumDataSource enumDataSource = ((JObject)data.config.dataSource).ToObject<EnumDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = enumDataSource.GetEditorDataItems();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            return customLabel.ProcessEditorDataItems(editorDataItems);
        }
    }
}
