namespace nuPickers
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Handles returning embedded resource files (html, css, js, png)
    /// </summary>
    public class EmbeddedResourceController : Controller
    {
        public FileStreamResult GetSharedResource(string folder, string file)
        {
            return this.GetResource("nuPickers.Shared." + folder + "." + file);
        }

        private FileStreamResult GetResource(string resource)
        {
            // get this assembly
            Assembly assembly = typeof(EmbeddedResourceController).Assembly;

            // if resource can be found
            if (assembly.GetManifestResourceNames().Any(x => x == resource))
            {
                return new FileStreamResult(
                                assembly.GetManifestResourceStream(resource),
                                this.GetMimeType(resource));
            }

            return null;
        }

        private string GetMimeType(string resource)
        {
            switch (Path.GetExtension(resource))
            {
                case ".html":   return "text/html";
                case ".css":    return "text/css";
                case ".js":     return "text/javascript";
                case ".png":    return "image/png";
                default:        return "text";
            }
        }
    }
}
