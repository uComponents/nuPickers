using System;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using ClientDependency.Core;
using nuPickers.EmbeddedResource;

namespace nuPickers.Compontents
{
    public class EmbeddedResourceCompontent : IComponent, Umbraco.Core.Composing.IComponent
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;
        public void Initialize()
        {

            RouteTable
                .Routes
                .MapRoute(
                    name: "nuPickersShared",
                    url:  EmbeddedResource.ROOT_URL.TrimStart("~/") + "{folder}/{file}",
                    defaults: new
                    {
                        controller = "EmbeddedResource",
                        action = "GetSharedResource"
                    },
                    namespaces: new[] { "nuPickers.EmbeddedResource" });

            FileWriters.AddWriterForExtension(EmbeddedResource.FILE_EXTENSION, new EmbeddedResourceVirtualFileWriter());
        }

        public void Terminate()
        {
            throw new NotImplementedException();
        }
    }
}