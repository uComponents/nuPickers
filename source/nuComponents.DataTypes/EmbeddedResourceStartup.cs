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

            // using seperate routes to ensure /Shared/ or /PropertEditors/ in the path
            RouteTable.Routes.MapRoute(
                name: "nuComponentsShared",
                url: "Umbraco/App_Plugins/nuComponents/DataTypes/Shared/{folder}/{file}",
                defaults: new
                {
                    controller = "EmbeddedResource",
                    action = "GetSharedResource"
                }
            );

            //RouteTable.Routes.MapRoute(
            //    name: "nuComponentsPropertyEditors",
            //    url: "Umbraco/App_Plugins/nuComponents/DataTypes/PropertyEditors/{folder}/{file}",
            //    defaults: new
            //    {
            //        controller = "EmbeddedResource",
            //        action = "GetPropertyEditorResource"
            //    }
            //);
        }
    }
}
