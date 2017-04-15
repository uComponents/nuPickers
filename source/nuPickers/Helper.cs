namespace nuPickers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Web;
    using System.Web.Hosting;
    using Umbraco.Web;

    internal static class Helper
    {
        internal static IEnumerable<string> GetAssemblyNames()
        {
            List<string> assemblyNames = new List<string>();

            // try to add App_Code directory
            string appCodePath = HostingEnvironment.MapPath("~/App_Code");
            if (appCodePath != null)
            {
                DirectoryInfo appCode = new DirectoryInfo(appCodePath);

                if (appCode.Exists && Helper.GetAssembly(appCode.Name) != null)
                {
                    assemblyNames.Add(appCode.Name);
                }
            }

            // add any .dll assemblies from the /bin directory
            string binPath = HostingEnvironment.MapPath("~/bin");
            if (binPath != null)
            {
                assemblyNames.AddRange(Directory.GetFiles(binPath, "*.dll").Select(x => x.Substring(x.LastIndexOf('\\') + 1)));
            }

            return assemblyNames;
        }

        /// <summary>
        /// attempts to get an assembly by it's name
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns>an Assembly or null</returns>
        internal static Assembly GetAssembly(string assemblyName)
        {
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
                try
                {
                    // HACK: http://stackoverflow.com/questions/1031431/system-reflection-assembly-loadfile-locks-file
                    return Assembly.Load(File.ReadAllBytes(assemblyFilePath));
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// extension method on Assembly to handle reflection loading exceptions
        /// </summary>
        /// <param name="assembly">the assembly to get types from</param>
        /// <returns>a collection of types found</returns>
        internal static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(x => x != null);
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }

        /// <summary>
        /// uses supplied url to check the file system (if prefixed with ~/) else makes an http query
        /// </summary>
        /// <param name="url">Url to download the resource from</param>
        /// <returns>An empty string, or the string result of either a file or an http response</returns>
        internal static string GetDataFromUrl(string url)
        {
            string data = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {
                if (VirtualPathUtility.IsAppRelative(url)) // starts with ~/
                {
                    bool fileExists = false;

                    if (!url.Contains("?"))
                    {
                        string filePath = HttpContext.Current.Server.MapPath(url);

                        if (File.Exists(filePath))
                        {
                            url = filePath;
                            fileExists = true;
                        }
                    }

                    if (!fileExists)
                    {
                        url = url.Replace("~/", (HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/"));
                    }
                }

                using (WebClient client = new WebClient())
                {
                    data = client.DownloadString(url);
                }
            }

            return data;
        }
    }
}