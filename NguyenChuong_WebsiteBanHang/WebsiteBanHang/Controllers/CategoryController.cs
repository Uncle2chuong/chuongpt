using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Controllers
{
    public class CategotyController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanHangEntities = new WebsiteBanhangEntities1();
        // GET: Login
        public ActionResult Index()
        {
            var lstCategory = objWebsiteBanHangEntities.Category.ToList();
            return View(lstCategory);
        }

        public ActionResult ProductCategory(int Id)
        {
            var lstprd = objWebsiteBanHangEntities.Product.Where(n => n.CategoryId == Id).ToList();
            return View(lstprd);
        }
    }
}