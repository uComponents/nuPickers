
namespace nuPickers.Shared.DotNetDataSource
{
    using Newtonsoft.Json.Linq;
    using nuPickers;
    using nuPickers.Shared.CustomLabel;
    using nuPickers.Shared.Editor;
    using nuPickers.Shared.TypeaheadListPicker;
    using System;
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
            List<string> assemblyNames = new List<string>();

            foreach (string assemblyName in Helper.GetAssemblyNames())
            {
                Assembly assembly = Helper.GetAssembly(assemblyName);

                if (assembly != null)
                {
                    if (assembly.GetLoadableTypes().Any(x => typeof(IDotNetDataSource).IsAssignableFrom(x)))
                    {
                        assemblyNames.Add(assemblyName);
                    }
                }
            }

            return assemblyNames;
        }

        public IEnumerable<object> GetClassNames([FromUri]string assemblyName)
        {
            Assembly assembly = Helper.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly
                        .GetLoadableTypes()
                        .Where(x => typeof(IDotNetDataSource).IsAssignableFrom(x))
                        .Select(x => x.FullName);
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
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] string propertyAlias, [FromBody] dynamic data)
        {
            return GetEditorDataItems(contextId, propertyAlias, null, data);
        }

        [HttpPost]
        public IEnumerable<EditorDataItem> GetEditorDataItems([FromUri] int contextId, [FromUri] string propertyAlias, [FromUri] string ids, [FromBody] dynamic data)
        {
            DotNetDataSource dotNetDataSource = ((JObject)data.config.dataSource).ToObject<DotNetDataSource>();
            // if there are ids then ignore typeahead
            dotNetDataSource.Typeahead = ids != null ? null : (string)data.typeahead;

            IEnumerable<EditorDataItem> editorDataItems = dotNetDataSource.GetEditorDataItems(contextId).ToList();

            CustomLabel customLabel = new CustomLabel((string)data.config.customLabel, contextId, propertyAlias);

            editorDataItems = customLabel.ProcessEditorDataItems(editorDataItems);

            // if the typeahead wasn't handled by the custom data-source, then fallback to default typeahead processing
            // if there are ids then ignore typeahead
            if (ids == null && !dotNetDataSource.HandledTypeahead)
            {
                TypeaheadListPicker typeaheadListPicker = new TypeaheadListPicker((string)data.typeahead);
                editorDataItems = typeaheadListPicker.ProcessEditorDataItems(editorDataItems);                
            }

            // if there are ids then filter by ids
            if (ids != null)
        {
                IEnumerable<string> collectionIds = ids.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).AsEnumerable<string>();
                editorDataItems = editorDataItems.Where(x => collectionIds.Contains(x.Key));
            }

            return editorDataItems;

        }

    }
}
