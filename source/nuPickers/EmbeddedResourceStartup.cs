namespace nuPickers
{
    using ClientDependency.Core;
    using System.Web.Routing;
    using Umbraco.Core;

    public class EmbeddedResourceStartup : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);
            RouteBuilder.BuildRoutes(RouteTable.Routes);
            FileWriters.AddWriterForExtension(".nu", new EmbeddedResourceWriter());
        }        
    }
}
