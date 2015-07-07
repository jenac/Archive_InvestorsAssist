using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using System;
using System.Configuration;
using System.Web.Http;

namespace InvestorsAssist._WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {

            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.IgnoreRoute("JS", "js/{*pathInfo}");
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            appBuilder.UseWebApi(config);

            /*
            // Configure Static Contents, map js to root
            var fileSystem = new PhysicalFileSystem(@".\js");
            var options = new FileServerOptions()
            {
                EnableDirectoryBrowsing = true,
                FileSystem = fileSystem,
            };

            appBuilder.UseFileServer(options);*/

        }
    }
}
