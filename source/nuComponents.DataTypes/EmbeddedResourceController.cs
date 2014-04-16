namespace nuComponents.DataTypes
{
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;

    /// <summary>
    /// Handles returning the embedded resource files
    /// </summary>
    public class EmbeddedResourceController : Controller
    {
        public FileStreamResult GetResource(string datatype, string file)
        {
            string resource = "nuComponents.DataTypes." + datatype + "." + file;

            // get this assembly
            Assembly assembly = typeof(EmbeddedResourceController).Assembly;
            
            // if resource can be found
            if (assembly.GetManifestResourceNames().Any(x => x == resource))
            {
                return new FileStreamResult(assembly.GetManifestResourceStream(resource), this.GetMimeType(resource));
            }

            return null;
        }

        private string GetMimeType(string resource)
        {
            switch (Path.GetExtension(resource))
            {
                case ".js":
                    return "text/javascript";

                case ".html":
                    return "text/html";

                case ".css":
                    return "text/stylesheet";

                default:
                    return "text";
            }
        }
    }
}
