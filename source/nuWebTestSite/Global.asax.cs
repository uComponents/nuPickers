using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using ClientDependency.Core;
using ClientDependency.Core.Mvc;
using Microsoft.Web.Mvc;
using nuPickers;

namespace nuWebTestSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //an extension method to replace the default razor view engine with the CDF view engine
            // instead of using the cdf module since this is only for mvc
            ViewEngines.Engines.ReplaceEngine<FixedRazorViewEngine>(new CdfRazorViewEngine());
            FileWriters.AddWriterForExtension(".nu", new EmbeddedResourceWriter());
        }
    }
}
