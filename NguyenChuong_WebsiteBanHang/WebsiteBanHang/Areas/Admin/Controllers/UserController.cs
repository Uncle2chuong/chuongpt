using WebsiteBanHang.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebsiteBanHang.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanhangEntities1 = new WebsiteBanhangEntities1();
        // GET: Admin/User
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            var lstUser = new List<Users>();
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
                lstUser = objWebsiteBanhangEntities1.Users.Where(n => n.FirstName.Contains(SearchString)).ToList();
            }
            else
            {
                lstUser = objWebsiteBanhangEntities1.Users.ToList();
            }
            ViewBag.currentFilter = SearchString;
            //số lượng items của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            //sắp xếp theo id sản phẩm
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Users objUser)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var check = objWebsiteBanhangEntities1.Users.FirstOrDefault(s => s.Email == objUser.Email);
                    if (check == null)
                    {
                        objUser.Password = GetMD5(objUser.Password);
                        objWebsiteBanhangEntities1.Configuration.ValidateOnSaveEnabled = false;
                    }
                    else
                    {
                        ViewBag.error = "Email đã tồn tại";
                        return View();
                    }
                    objUser.IsAdmin = false;
                    objWebsiteBanhangEntities1.Users.Add(objUser);
                    objWebsiteBanhangEntities1.SaveChanges();
                    return RedirectToAction("Index", "User");
                }
                catch (Exception)
                {
                    return View();
                }
            }
            return View(objUser);
        }
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
        [HttpGet]
        public ActionResult Details(int id)
        {
            var objUser = objWebsiteBanhangEntities1.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = objWebsiteBanhangEntities1.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(Users objUsr)
        {
            var objUser = objWebsiteBanhangEntities1.Users.Where(n => n.Id == objUsr.Id).FirstOrDefault();
            objWebsiteBanhangEntities1.Users.Remove(objUser);
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = objWebsiteBanhangEntities1.Users.Where(n => n.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Users objUser)
        {
            objWebsiteBanhangEntities1.Entry(objUser).State = EntityState.Modified;
            objWebsiteBanhangEntities1.SaveChanges();
            return RedirectToAction("Index");
        }
        //public ActionResult Trash()
        //{
        //    var lstUser = new List<Users>();
        //    lstUser = objWebsiteBanhangEntities1.Users.Where(m => m.Deleted == true).ToList();
        //    return View(lstUser);
        //}
        //public ActionResult DelTrash(int Id)
        //{
        //    Users User = objWebsiteBanhangEntities1.Users.Find(Id);

        //    User.Deleted = true;
        //    objWebsiteBanhangEntities1.SaveChanges();
        //    return RedirectToAction("Index", "User");
        //}
        //public ActionResult ReTrash(int Id)
        //{
        //    Users User = objWebsiteBanhangEntities1.Users.Find(Id);
        //    User.Deleted = false;
        //    User.ShowOnHomePage = false;
        //    objWebsiteBanhangEntities1.SaveChanges();
        //    return RedirectToAction("Trash", "User");
        //}
        //public ActionResult ShowOnHomePage(int Id, bool flag)
        //{
        //    Users User = objWebASPEntities.Users.Find(Id);
        //    User.ShowOnHomePage = flag;
        //    objWebsiteBanhangEntities1.SaveChanges();
        //    return RedirectToAction("Index", "User");
        //}
    }
}