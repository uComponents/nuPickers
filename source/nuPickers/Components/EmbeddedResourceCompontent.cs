using System;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.Routing;
using ClientDependency.Core;
using nuPickers.EmbeddedResource;
using Umbraco.Core.Logging;

namespace nuPickers.Components
{
    public class EmbeddedResourceCompontent : IComponent, Umbraco.Core.Composing.IComponent
    {
        private readonly IProfilingLogger _logger;

        public EmbeddedResourceCompontent(IProfilingLogger profilingLogger)
        {
            _logger = profilingLogger;
        }


        public void Dispose()
        {

        }

        public ISite Site { get; set; }
        public event EventHandler Disposed;

        public void Initialize()
        {
            RouteTable
                .Routes
                .MapRoute(
                    name: "nuPickersShared",
                    url: EmbeddedResource.EmbeddedResource.ROOT_URL.TrimStart("~/".ToCharArray()) + "{folder}/{file}",
                    defaults: new
                    {
                        controller = "EmbeddedResource",
                        action = "GetSharedResource"
                    },
                    namespaces: new[] {"nuPickers.EmbeddedResource"});

            FileWriters.AddWriterForExtension(EmbeddedResource.EmbeddedResource.FILE_EXTENSION,
                new EmbeddedResourceVirtualFileWriter(_logger));
        }

        public void Terminate()
        {

        }
    }
}