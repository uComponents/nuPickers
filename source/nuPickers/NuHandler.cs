
namespace nuPickers
{
    using System;
    using System.IO;
    using System.Web;
    using System.Web.Routing;

    public class NuHandler : IHttpHandler, IRouteHandler
    {
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var appRelativeCurrentExecutionFilePath = context.Request.AppRelativeCurrentExecutionFilePath;
            var fileName = Path.GetFileName(appRelativeCurrentExecutionFilePath);
            var directoryName = Path.GetDirectoryName(appRelativeCurrentExecutionFilePath);

            if (directoryName != null)
            {
                var pos = directoryName.LastIndexOf("\\", StringComparison.Ordinal);
                var folder = directoryName.Substring(pos + 1);

                string resourceName;
                var resourceStream = EmbeddedResourceHelper.GetResource("nuPickers.Shared." + folder + "." + fileName, out resourceName);

                if (resourceStream != null)
                {
                    context.Response.ContentType = MimeMapping.GetMimeMapping(resourceName);
                    resourceStream.CopyTo(context.Response.OutputStream);
                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
    }
}
