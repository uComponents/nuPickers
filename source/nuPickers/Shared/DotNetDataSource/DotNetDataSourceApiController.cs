
namespace nuPickers.Shared.DotNetDataSource
{
    using System;
    using Newtonsoft.Json.Linq;
    using nuPickers;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
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
            Assembly assembly = Helper.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly.GetTypes().Where(x => typeof(IDotNetDataSource).IsAssignableFrom(x)).Select(x => x.FullName);
            }
            
            return null;
        }

        /// <summary>
        /// Get a collection of properties that have been marked with the DotNetDataSourceAttribute,
        /// each one of these will be used as a custom property
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public IEnumerable<object> GetProperties([FromUri]string assemblyName, [FromUri]string className)
        {
            Assembly assembly = Helper.GetAssembly(assemblyName);

            if (assembly != null)
            {
                Type type = assembly.GetType(className);
                if (type != null)
                {
                    return type.GetProperties()
                                .Where(x => x.GetCustomAttributes(typeof(DotNetDataSourceAttribute), false).Any())
                                .Select(x => new
                                            {
                                                name = x.Name,
                                                title = ((DotNetDataSourceAttribute)x.GetCustomAttribute(typeof(DotNetDataSourceAttribute))).Title ?? x.Name,
                                                description = ((DotNetDataSourceAttribute)x.GetCustomAttribute(typeof(DotNetDataSourceAttribute))).Description
                                            });
                }
            }

            return null;
        }


        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromBody] dynamic data)
        {
            DotNetDataSource dotNetDataSource = ((JObject)data.config.dataSource).ToObject<DotNetDataSource>();
            dotNetDataSource.Typeahead = (string)data.typeahead;

            IEnumerable<EditorDataItem> editorDataItems = dotNetDataSource.GetEditorDataItems();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId);
            TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);

            // process the labels and then handle any type ahead text
            return typeaheadListPicker.ProcessEditorDataItems(customLabel.ProcessEditorDataItems(editorDataItems));
        }

    }
}
