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

            /* THIS WILL BE LEGACY */
            RouteTable.Routes.MapRoute(
                name: "nuComponents",
                url: "Umbraco/App_Plugins/nuComponents/DataTypes/{datatype}/{file}",
                defaults: new { 
                            controller = "EmbeddedResource", 
                            action = "GetResource"
                }
            );


            // NEW
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

            RouteTable.Routes.MapRoute(
                name: "nuComponentsPropertyEditors",
                url: "Umbraco/App_Plugins/nuComponents/DataTypes/PropertyEditors/{folder}/{file}",
                defaults: new
                {
                    controller = "EmbeddedResource",
                    action = "GetPropertyEditorResource"
                }
            );
        }
    }
}
