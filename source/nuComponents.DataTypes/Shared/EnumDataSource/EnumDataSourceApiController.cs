namespace nuComponents.DataTypes.Shared
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Http;
    using umbraco;
    using umbraco.cms.businesslogic.macro;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;

    [PluginController("nuComponentsDataTypesShared")]
    public class EnumDataSourceApiController : UmbracoAuthorizedJsonController
    {
        /// <summary>
        /// Gets the names of all assemblies and optional AppCode folder
        /// </summary>
        /// <returns></returns>
        public IEnumerable<object> GetAssemblyNames()
        {
            List<string> assemblyNames = new List<string>();

            // check if the App_Code directory exists and has any files
            DirectoryInfo appCode = new DirectoryInfo(HostingEnvironment.MapPath("~/App_Code"));
            if (appCode.Exists && appCode.GetFiles().Length > 0)
            {
                assemblyNames.Add(appCode.Name);
            }

            // add assemblies from the /bin directory
            assemblyNames.AddRange(Directory.GetFiles(HostingEnvironment.MapPath("~/bin"), "*.dll").Select(x => x.Substring(x.LastIndexOf('\\') + 1)));

            return assemblyNames;
        }


        public IEnumerable<object> GetEnumNames([FromUri]string assemblyName)
        {
            Assembly assembly = EnumDataSource.EnumDataSource.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly.GetTypes().Where(x => x.IsEnum).Select(x => x.FullName);
            }

            return null;
        }

    }
}
