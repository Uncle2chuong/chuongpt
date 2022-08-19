using WebsiteBanHang.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static WebsiteBanHang.Common;
using WebsiteBanHang.Models;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanhangEntities1 = new WebsiteBanhangEntities1();
        // GET: Admin/Product
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            //var lstProduct = objWebsiteBanhangEntities1.Product.ToList();
            //var lstProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Name.Contains(SearchString)).ToList();
            var lstProduct = new List<Product>();
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
                lstProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                lstProduct = objWebsiteBanhangEntities1.Product.ToList();
            }
            ViewBag.currentFilter = SearchString;
            //số lượng items của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            this.LoadData();
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            this.LoadData();
            if (ModelState.IsValid)
            {
                try
                {
                    if (objProduct.ImageUpLoad != null)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                        string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                        fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                        objProduct.Avatar = fileName;
                        objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                    }
                    objProduct.CreatedOnUtc = DateTime.Now;
                    objWebsiteBanhangEntities1.Product.Add(objProduct);
                    objWebsiteBanhangEntities1.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View(objProduct);
        }
        void LoadData()
        {
            Common objCommon = new Common();
            //lấy danh sách danh mục dưới csdl
            var lstcat = objWebsiteBanhangEntities1.Category.ToList();
            //convert sang select list dang value, list
            ListtoDataTableConverter converter = new ListtoDataTableConverter();
            DataTable dtCategory = converter.ToDataTable(lstcat);
            ViewBag.ListCategory = objCommon.ToSelectList(dtCategory, "Id", "Name");

            //lấy danh sách thương hiệu dưới csdl
            var lstBrand = objWebsiteBanhangEntities1.Brand.ToList();
            //convert sang select list dang value, list
            DataTable dtBrand = converter.ToDataTable(lstBrand);
            ViewBag.ListBrand = objCommon.ToSelectList(dtBrand, "Id", "Name");

            //
            List<ProductType> lstProductType = new List<ProductType>();
            ProductType objProductType = new ProductType();
            objProductType.Id = 01;
            objProductType.Name = "Giảm giá sốc";
            lstProductType.Add(objProductType);

            objProductType = new ProductType();
            objProductType.Id = 02;
            objProductType.Name = "Đề xuất";
            lstProductType.Add(objProductType);
            DataTable dataProductType = converter.ToDataTable(lstProductType);
            ViewBag.ListProductType = objCommon.ToSelectList(dataProductType, "Id", "Name");
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPrd)
        {
            var objProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Id == objPrd.Id).FirstOrDefault();
            objWebsiteBanhangEntities1.Product.Remove(objProduct);
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = objWebsiteBanhangEntities1.Product.Where(n => n.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product objProduct)
        {
            if (objProduct.ImageUpLoad != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpLoad.FileName);
                string extension = Path.GetExtension(objProduct.ImageUpLoad.FileName);
                fileName = fileName + "_" + long.Parse(DateTime.Now.ToString("yyyyMMddhhmmss")) + extension;
                objProduct.Avatar = fileName;
                objProduct.ImageUpLoad.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
            }
            objWebsiteBanhangEntities1.Entry(objProduct).State = EntityState.Modified;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Trash()
        {
            var lstPrd = new List<Product>();
            lstPrd = objWebsiteBanhangEntities1.Product.Where(m => m.Deleted == true).ToList();
            return View(lstPrd);
        }
        public ActionResult DelTrash(int Id)
        {
            Product Prd = objWebsiteBanhangEntities1.Product.Find(Id);

            Prd.Deleted = true;

            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Product");
        }
        public ActionResult ReTrash(int Id)
        {
            Product Prd = objWebsiteBanhangEntities1.Product.Find(Id);
            Prd.Deleted = false;
            Prd.ShowOnHomePage = false;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Trash", "Product");
        }
        public ActionResult ShowOnHomePage(int Id, bool flag)
        {
            Product Prd = objWebsiteBanhangEntities1.Product.Find(Id);
            Prd.ShowOnHomePage = flag;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}