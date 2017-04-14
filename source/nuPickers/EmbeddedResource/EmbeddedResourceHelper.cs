namespace nuPickers.EmbeddedResource
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web;
    using Umbraco.Core;

    internal static class EmbeddedResourceHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource">expecting a namespaced string to the resource</param>
        /// <returns></returns>
        internal static bool ResourceExists(string resourceName)
        {
            // get this assembly
            var assembly = typeof(EmbeddedResourceController).Assembly;

            // find the resource name not case sensitive
            var manifestResourceName =
                assembly.GetManifestResourceNames()
                    .FirstOrDefault(
                        x => x.ToLower(CultureInfo.InvariantCulture) == resourceName.ToLower(CultureInfo.InvariantCulture));

            return !string.IsNullOrEmpty(manifestResourceName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource">exepted namespaced resource</param>
        /// <returns>null or the resource stream</returns>
        internal static Stream GetResource(string resourceName)
        {
            // get this assembly
            var assembly = typeof(EmbeddedResourceController).Assembly;

            // find the resource name not case sensitive
            var manifestResourceName =
                assembly.GetManifestResourceNames()
                    .FirstOrDefault(
                        x => x.ToLower(CultureInfo.InvariantCulture) == resourceName.ToLower(CultureInfo.InvariantCulture));

            if (manifestResourceName != null)
            {
                return assembly.GetManifestResourceStream(manifestResourceName);
            }

            return null;
        }


        /// <summary>
        /// Convert a url for an embedded resource, to a namespaced string for an embedded resource
        /// </summary>
        /// <param name="path">string url to embedded resource</param>
        /// <returns>null, or a resource namespaced string (the resource may not actually exist)</returns>
        internal static string GetResourceNameFromPath(string path)
        {
            string resourceName = null;

            if (!VirtualPathUtility.IsAppRelative(path))
            {
                path = VirtualPathUtility.ToAppRelative(path);
            }

            if (path.StartsWith(EmbeddedResource.RootUrlPrefixed))
            {
                resourceName = "nuPickers.Shared." + path.TrimStart(EmbeddedResource.RootUrlPrefixed).Replace("/", ".").TrimEnd(".nu");
            }

            return resourceName;
        }
    }
}