using System.Web.Mvc;

namespace WebsiteBanHang.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdminLogin",
                "Admin/Login",
                new { Controller = "Auth", action = "Login", id = UrlParameter.Optional },
                 new[] { "WebsiteBanHang.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "AdminLogout",
                "Admin/Logout",
                new { Controller = "Auth", action = "Logout", id = UrlParameter.Optional },
                 new[] { "WebsiteBanHang.Areas.Admin.Controllers" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new[] { "WebsiteBanHang.Areas.Admin.Controllers" }
            );


        }

    }
}