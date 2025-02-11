using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unity.AspNet.Mvc;

namespace MVCStudentManagementCRUD
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Use Unity.MVC for Dependency Injection. (Get's rid of the MissingMethodException)
            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));
        }
    }
}
