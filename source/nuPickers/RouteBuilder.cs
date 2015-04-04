namespace nuPickers
{
    using System.Web.Mvc;
    using System.Web.Routing;

    public static class RouteBuilder
    {
        public static void BuildRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                name: "nuPickersShared",
                url: "App_Plugins/nuPickers/Shared/{folder}/{file}",
                defaults: new
                {
                    controller = "EmbeddedResource",
                    action = "GetSharedResource"
                },
                namespaces: new[] { "nuPickers" });
        }
    }
}