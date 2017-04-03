namespace nuPickers.EmbeddedResource
{
    using System.Web;
    using System.Web.Mvc;

    /// <summary>
    /// Handles returning embedded resource files (html, css, js, png)
    /// </summary>
    public class EmbeddedResourceController : Controller
    {
        public ActionResult GetSharedResource(string folder, string file)
        {
            string resourceName;
            var resourceStream = EmbeddedResourceHelper.GetResource("nuPickers.Shared." + folder + "." + file, out resourceName);
            
            if (resourceStream != null)
            {
                return new FileStreamResult(resourceStream, GetMimeType(resourceName)); ;
            }

            return this.HttpNotFound();
        }

        private string GetMimeType(string resource)
        {
            var mimeType = MimeMapping.GetMimeMapping(resource);
            return mimeType ?? "text";
        }
    }
}
