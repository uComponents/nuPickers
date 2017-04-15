namespace nuPickers.EmbeddedResource
{
    using ClientDependency.Core;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Umbraco.Core;

    public class EmbeddedResourceStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            RouteTable
                .Routes
                .MapRoute(
                    name: "nuPickersShared",
                    url: "App_Plugins/nuPickers/Shared/{folder}/{file}",
                    defaults: new
                    {
                        controller = "EmbeddedResource",
                        action = "GetSharedResource"
                    },
                    namespaces: new[] { "nuPickers" });

            FileWriters.AddWriterForExtension(EmbeddedResource.FILE_EXTENSION, new EmbeddedResourceWriter());
        }

        protected override bool ExecuteWhenApplicationNotConfigured
        {
            get { return true; }
        }
    }
}
