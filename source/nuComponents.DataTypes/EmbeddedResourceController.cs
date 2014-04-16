namespace nuComponents.DataTypes
{
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
                return new FileStreamResult(assembly.GetManifestResourceStream(resource), this.GetMIMEType(resource));
            }

            return null;
        }

        private string GetMIMEType(string resource)
        {
            if (resource.EndsWith(".js"))
            {
                return "text/javascript";
            }

            if (resource.EndsWith(".html"))
            {
                return "text/html";
            }

            if (resource.EndsWith(".css"))
            {
                return "text/stylesheet";
            }

            return "text";
        }
    }
}
