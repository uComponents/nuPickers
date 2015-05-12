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
        /// 
        /// </summary>
        /// <returns>a collection of assembly names that have enums in them</returns>
        public IEnumerable<object> GetAssemblyNames()
        {
            List<string> assemblyNames = new List<string>();

            foreach(string assemblyName in Helper.GetAssemblyNames())
            {
                Assembly assembly = Helper.GetAssembly(assemblyName);

                if (assembly != null)
                {
                    if (assembly.GetLoadableTypes().Any(x => x.IsEnum))
                    {
                        assemblyNames.Add(assemblyName);
                    }
                }
            }

            return assemblyNames;
        }

        public IEnumerable<object> GetEnumNames([FromUri]string assemblyName)
        {
            Assembly assembly = Helper.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly
                        .GetLoadableTypes()
                        .Where(x => x.IsEnum)
                        .Select(x => x.FullName);
            }

            return null;
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int currentId, [FromUri] int parentId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            int contextId = currentId;

            EnumDataSource enumDataSource = ((JObject)data.config.dataSource).ToObject<EnumDataSource>();

            IEnumerable<EditorDataItem> editorDataItems = enumDataSource.GetEditorDataItems();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            return customLabel.ProcessEditorDataItems(editorDataItems);
        }
    }
}
