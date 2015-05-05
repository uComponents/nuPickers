namespace nuPickers
{
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using Umbraco.Core;

    internal class EmbeddedResourceHelper
    {
        public static bool ResourceExists(string resource)
        {
            if (resource.StartsWith(EmbeddedResource.RootUrl))
            {
                resource = "nuPickers.Shared." + resource.TrimStart(EmbeddedResource.RootUrl).Replace("/", ".").TrimEnd(".nu");
            }

            // get this assembly
            var assembly = typeof(EmbeddedResourceController).Assembly;

            // find the resource name not case sensitive
            var resourceName =
                assembly.GetManifestResourceNames()
                    .FirstOrDefault(
                        x => x.ToLower(CultureInfo.InvariantCulture) == resource.ToLower(CultureInfo.InvariantCulture));

            return !string.IsNullOrEmpty(resourceName);
        }

        public static Stream GetResource(string resource, out string resourceName)
        {
            if (resource.StartsWith(EmbeddedResource.RootUrl))
            {
                resource = "nuPickers.Shared." + resource.TrimStart(EmbeddedResource.RootUrl).Replace("/", ".").TrimEnd(".nu");
            }
            else if (resource.EndsWith(".nu"))
            {
                resource = resource.TrimEnd(".nu");
            }

            // get this assembly
            var assembly = typeof(EmbeddedResourceController).Assembly;

            // find the resource name not case sensitive
            resourceName =
                assembly.GetManifestResourceNames()
                    .FirstOrDefault(
                        x => x.ToLower(CultureInfo.InvariantCulture) == resource.ToLower(CultureInfo.InvariantCulture));

            if (resourceName != null)
            {
                return assembly.GetManifestResourceStream(resourceName);
            }

            return null;
        }
    }
}