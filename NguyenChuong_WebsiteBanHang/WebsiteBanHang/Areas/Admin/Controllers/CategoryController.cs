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
    public class CategoryController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanhangEntities1 = new WebsiteBanhangEntities1();
        // GET: Admin/Category
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstCategory = new List<Category>();
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
                lstCategory = objWebsiteBanhangEntities1.Category.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstCategory = objWebsiteBanhangEntities1.Category.ToList();
            }
            ViewBag.currentFilter = SearchString;
            //số lượng items của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm
            lstCategory = lstCategory.OrderByDescending(n => n.Id).ToList();
            return View(lstCategory.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Category objCategory)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (objCategory.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objCategory.Avatar = fileName;
                        objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objCategory.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanhangEntities1.Category.Add(objCategory);
                    objWebsiteBanhangEntities1.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objCategory);
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = objWebsiteBanhangEntities1.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = objWebsiteBanhangEntities1.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objcat)
        {
            var objCategory = objWebsiteBanhangEntities1.Category.Where(n => n.Id == objcat.Id).FirstOrDefault();
            objWebsiteBanhangEntities1.Category.Remove(objCategory);
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = objWebsiteBanhangEntities1.Category.Where(n => n.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category objCategory)
        {
            if (objCategory.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objCategory.ImageUpLoad.FileName);
                fileName += extension;
                objCategory.Avatar = fileName;
                objCategory.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            objWebsiteBanhangEntities1.Entry(objCategory).State = EntityState.Modified;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            var lstCat = new List<Category>();
            lstCat = objWebsiteBanhangEntities1.Category.Where(m => m.Deleted == true).ToList();
            return View(lstCat);
        }
        public ActionResult DelTrash(int Id)
        {
            Category cate = objWebsiteBanhangEntities1.Category.Find(Id);

            cate.Deleted = true;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Category");
        }
        public ActionResult ReTrash(int Id)
        {
            Category cate = objWebsiteBanhangEntities1.Category.Find(Id);
            cate.Deleted = false;
            cate.ShowOnHomePage = false;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Category");
        }
        public ActionResult ShowOnHomePage(int Id, bool flag)
        {
            Category cate = objWebsiteBanhangEntities1.Category.Find(Id);
            cate.ShowOnHomePage = flag;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
    }
}