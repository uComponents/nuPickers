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
                name: "nuComponents",
                url: "Umbraco/App_Plugins/nuComponents/DataTypes/{datatype}/{file}",
                defaults: new { 
                            controller = "EmbeddedResource", 
                            action = "GetResource"
                }
            );
        }
    }
}
