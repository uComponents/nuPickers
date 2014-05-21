namespace nuComponents.DataTypes
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Umbraco.Core;

    public class EmbeddedResourceStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            RouteTable.Routes.MapRoute(
                name: "nuComponentsShared",
                url: "Umbraco/App_Plugins/nuComponents/DataTypes/Shared/{folder}/{file}",
                defaults: new
                {
                    controller = "EmbeddedResource",
                    action = "GetSharedResource"
                }
            );
        }
    }
}
