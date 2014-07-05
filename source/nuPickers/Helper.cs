
namespace nuPickers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using umbraco;

    internal static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal static IEnumerable<string> GetAssemblyNames()
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

        /// <summary>
        /// Gets the <see cref="Assembly"/> with the specified name.
        /// </summary>
        /// <remarks>Works in Medium Trust.</remarks>
        /// <param name="assemblyName">The <see cref="Assembly"/> name.</param>
        /// <returns>The <see cref="Assembly"/>.</returns>
        internal static Assembly GetAssembly(string assemblyName)
        {
            AspNetHostingPermissionLevel appTrustLevel = GlobalSettings.ApplicationTrustLevel;
            if (appTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
            {
                AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += (sender, args) =>
                {
                    return Assembly.ReflectionOnlyLoad(args.Name);
                };
            }

            if (string.Equals(assemblyName, "App_Code", StringComparison.InvariantCultureIgnoreCase))
            {
				try
				{
					return Assembly.Load(assemblyName);
				}
				catch (FileNotFoundException)
				{
					return null;
				}
            }

            string assemblyFilePath = HostingEnvironment.MapPath(string.Concat("~/bin/", assemblyName));
            if (!string.IsNullOrEmpty(assemblyFilePath))
            {
                if (appTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
                {
                    return Assembly.ReflectionOnlyLoadFrom(assemblyFilePath);
                }
                else
                {
                    // Medium Trust support
                    return Assembly.LoadFile(assemblyFilePath);
                }
            }

            return null;
        }
    }
}
