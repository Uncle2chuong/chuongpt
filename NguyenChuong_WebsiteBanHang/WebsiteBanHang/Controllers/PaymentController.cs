using WebsiteBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteBanHang.Context;

namespace WebsiteBanHang.Controllers
{
    public class PaymentController : Controller
    {
        WebsiteBanhangEntities1 objWebsiteBanHangEntities = new WebsiteBanhangEntities1();
        // GET: Payment
        public ActionResult Index()
        {
            if (Session["IdUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                //lấy thông tin từ giỏ hàng từ biến session
                var lstCart = (List<CartModel>)Session["cart"];
                //gán dữ liệu cho order
                Order objOrder = new Order();
                objOrder.Name = "Donhang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.UserId = int.Parse(Session["idUser"].ToString());
                objOrder.CreatedOnUtc = DateTime.Now;

                objOrder.Status = 1;
                objWebsiteBanHangEntities.Order.Add(objOrder);

                objWebsiteBanHangEntities.SaveChanges();

                //lấy orderid vừa mới tạo lưu vào orderdetail
                int intOrderId = objOrder.Id;
                List<OrderDetail> lstOrderDetail = new List<OrderDetail>();

                foreach (var item in lstCart)
                {
                    OrderDetail objOrderdetail = new OrderDetail();
                    objOrderdetail.Quantity = item.Quantity;
                    objOrderdetail.OrderId = intOrderId;
                    objOrderdetail.Productid = item.Product.Id;
                    lstOrderDetail.Add(objOrderdetail);
                }
                objWebsiteBanHangEntities.OrderDetail.AddRange(lstOrderDetail);
                objWebsiteBanHangEntities.SaveChanges();
            }
            return View();
        }
    }
}