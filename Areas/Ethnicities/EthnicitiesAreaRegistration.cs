using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Ethnicities
{
    public class EthnicitiesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Ethnicities";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Ethnicities_default",
                "Ethnicities/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}