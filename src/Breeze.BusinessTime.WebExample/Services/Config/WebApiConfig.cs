using System.Web.Http;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Breeze.BusinessTime.WebExample.WebApiConfig), "RegisterWebApiPreStart")]
namespace Breeze.BusinessTime.WebExample
{
    public static class WebApiConfig
    {
        public static void RegisterWebApiPreStart()
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: null,
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
