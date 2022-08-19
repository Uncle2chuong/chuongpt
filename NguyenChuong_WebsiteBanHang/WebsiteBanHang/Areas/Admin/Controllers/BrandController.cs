using WebsiteBanHang.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanhangEntities1 = new WebsiteBanhangEntities1();
        // GET: Admin/Brand
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {

            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }
            if (!string.IsNullOrEmpty(SearchString))
            {
                lstBrand = objWebsiteBanhangEntities1.Brand.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstBrand = objWebsiteBanhangEntities1.Brand.ToList();
            }
            ViewBag.currentFilter = SearchString;
            //số lượng items của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (objBrand.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objBrand.Avatar = fileName;
                        objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objBrand.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanhangEntities1.Brand.Add(objBrand);
                    objWebsiteBanhangEntities1.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objBrand);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = objWebsiteBanhangEntities1.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = objWebsiteBanhangEntities1.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objBrd)
        {
            var objBrand = objWebsiteBanhangEntities1.Brand.Where(n => n.Id == objBrd.Id).FirstOrDefault();
            objWebsiteBanhangEntities1.Brand.Remove(objBrand);
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = objWebsiteBanhangEntities1.Brand.Where(n => n.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Brand objBrand)
        {
            if (objBrand.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objBrand.ImageUpLoad.FileName);
                fileName += extension;
                objBrand.Avatar = fileName;
                objBrand.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            
            objWebsiteBanhangEntities1.Entry(objBrand).State = EntityState.Modified;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            var lstBrand = new List<Brand>();
            lstBrand = objWebsiteBanhangEntities1.Brand.Where(m => m.Deleted == true).ToList();
            return View(lstBrand);
        }
        public ActionResult DelTrash(int Id)
        {
            Brand Brand = objWebsiteBanhangEntities1.Brand.Find(Id);

            Brand.Deleted = true;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Brand");
        }
        public ActionResult ReTrash(int Id)
        {
            Brand Brand = objWebsiteBanhangEntities1.Brand.Find(Id);
            Brand.Deleted = false;
            Brand.ShowOnHomePage = false;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Brand");
        }
        public ActionResult ShowOnHomePage(int Id, bool flag)
        {
            Brand Brand = objWebsiteBanhangEntities1.Brand.Find(Id);
            Brand.ShowOnHomePage = flag;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index", "Brand");
        }
    }

}