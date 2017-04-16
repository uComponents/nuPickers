namespace nuPickers.Shared.DotNetDataSource
{
    using nuPickers;
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
    }
}