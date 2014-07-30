namespace nuPickers
{
    using System.Globalization;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Handles returning embedded resource files (html, css, js, png)
    /// </summary>
    public class EmbeddedResourceController : Controller
    {
        public ActionResult GetSharedResource(string folder, string file)
        {
            var resource = this.GetResource("nuPickers.Shared." + folder + "." + file);

            if (resource != null)
            {
                return resource;
            }

            return this.HttpNotFound();
        }

        private FileStreamResult GetResource(string resource)
        {
            // get this assembly
            var assembly = typeof(EmbeddedResourceController).Assembly;

            // find the resource name not case sensitive
            var resourceName =
                assembly.GetManifestResourceNames()
                    .FirstOrDefault(
                        x => x.ToLower(CultureInfo.InvariantCulture) == resource.ToLower(CultureInfo.InvariantCulture));

            if (resourceName != null)
            {
                return new FileStreamResult(
                                assembly.GetManifestResourceStream(resourceName),
                                this.GetMimeType(resourceName));
            }

            return null;
        }

        private string GetMimeType(string resource)
        {
            var mimeType = MimeMapping.GetMimeMapping(resource);
            return mimeType ?? "text";
        }
    }
}
