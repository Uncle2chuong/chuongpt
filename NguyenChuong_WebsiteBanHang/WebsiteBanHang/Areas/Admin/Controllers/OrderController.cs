using WebsiteBanHang.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanhangEntities1 = new WebsiteBanhangEntities1();
        // GET: Admin/Order
        public ActionResult Index()
        {
            var lstOrder = objWebsiteBanhangEntities1.Order.ToList();
            return View(lstOrder);
        }
        public ActionResult Details(int id)
        {
            var objOrder = objWebsiteBanhangEntities1.Order.Where(n => n.Id == id).FirstOrDefault();
            return View(objOrder);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objOrder = objWebsiteBanhangEntities1.Order.Where(n => n.Id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        public ActionResult Delete(Order objod)
        {
            var objOrder = objWebsiteBanhangEntities1.Order.Where(n => n.Id == objod.Id).FirstOrDefault();
            objWebsiteBanhangEntities1.Order.Remove(objOrder);
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}