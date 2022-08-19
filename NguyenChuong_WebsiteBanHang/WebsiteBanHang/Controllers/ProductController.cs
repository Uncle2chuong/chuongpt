using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Controllers
{
    public class ProductController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanHangEntities = new WebsiteBanhangEntities1();
        // GET: Product
        public ActionResult Detail(int id)
        {
            var objProduct = objWebsiteBanHangEntities.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }
    }
}