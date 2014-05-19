namespace nuComponents.DataTypes.Shared.EnumDataSource
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Hosting;
    using System.Web.Http;
    using Umbraco.Web.Editors;
    using Umbraco.Web.Mvc;
    using nuComponents.DataTypes.Interfaces;
    using nuComponents.DataTypes.Shared.Picker;
    using Newtonsoft.Json.Linq;
    using nuComponents.DataTypes.Shared.CustomLabel;

    [PluginController("nuComponents")]
    public class EnumDataSourceApiController : UmbracoAuthorizedJsonController, IPickerApiController
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
            Assembly assembly = EnumDataSource.GetAssembly(assemblyName);

            if (assembly != null)
            {
                return assembly.GetTypes().Where(x => x.IsEnum).Select(x => x.FullName);
            }

            return null;
        }

        [HttpPost]
        public IEnumerable<PickerEditorOption> GetEditorOptions([FromUri] int contextId, [FromBody] dynamic config)
        {
            EnumDataSource enumDataSource = ((JObject)config.dataSource).ToObject<EnumDataSource>();

            IEnumerable<PickerEditorOption> pickerEditorOptions = enumDataSource.GetEditorOptions();

            CustomLabel customLabel = new CustomLabel((string)config.customLabel);

            return customLabel.ProcessPickerEditorOptions(contextId, pickerEditorOptions);
        }
    }
}
