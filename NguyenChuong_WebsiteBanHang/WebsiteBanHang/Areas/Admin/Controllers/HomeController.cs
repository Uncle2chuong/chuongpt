using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanHangEntities = new WebsiteBanhangEntities1();
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }   
    }
}