using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using App.Core;
using App.Web.Models;

namespace App.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static UnityContainer container;
        public static IUnityContainer Container
        {
            get { return container; }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            ControllerBuilder.Current.DefaultNamespaces.Add("App.Web.Controllers");
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            InitializeContainer();
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
        
        protected void InitializeContainer()
        {
            if (container == null)
            {
                container = new UnityContainer();
                container.RegisterInstance<IUnityContainer>(container);

                container.RegisterType<IContentService, ContentService>();
                container.RegisterType<IFormsAuthentication, FormsAuthenticationService>();
            }
        }
    }
}