using _23DH111065_TaTrungHieu_KtraFE_TH_.Models;
using _23DH111065_TaTrungHieu_KtraFE_TH_.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23DH111065_TaTrungHieu_KtraFE_TH_.Controllers
{
    public class OrderController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        //GET: Order
        //public ActionResult Checkout()
        //{
        //    return View();
        //}

        //GET: Order/
        [Authorize]
        public ActionResult Checkout()
        {
            //Kiểm tra giỏ hàng trong session
            //Nếu giỏ hàng rỗng hoặc không  có sản phẩm thì chuyển về trang chủ
            var cart = Session["Cart"] as List<CartItem>;
            if (cart == null || !cart.Any())
            {

                return RedirectToAction("Index", "Home");
            }
            //xác thực người dùng đã đăng nhập chưa
            var user = db.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null) { return RedirectToAction("Login", "Account"); }
           //CHƯA LÀM //lấy thông tin khách hàng từ csdl nếu chưa có chuyển tới trang đăng nhập
           var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
            if (customer == null) { return RedirectToAction("Login", "Account"); }

            var model = new CheckoutVM
            {
                CartItems = cart,
                TotalAmount = cart.Sum(item => item.TotalPrice),
                OrderDate = DateTime.Now,
                ShippingAddress = customer.CustomerAddress,
                CustomerID = customer.CustomerID,
                Username = customer.Username,
            };
            return View(model);
        }


        //POST: Order/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Checkout(CheckoutVM model)
        {
            if (ModelState.IsValid)
            {
                var cart = Session["Cart"] as List<CartItem>;

                if (cart == null || !cart.Any())
                {
                    return RedirectToAction("Index", "Home");
                }

                var user = db.Users.SingleOrDefault(u=>u.Username == User.Identity.Name);
                if (user == null) { return RedirectToAction("Login", "Account"); }
                var customer = db.Customers.SingleOrDefault(c => c.Username == user.Username);
                if (customer == null) { return RedirectToAction("Login", "Account"); }
                if (model.PaymentMethod == "Paypal")
                {
                    return RedirectToAction("PaymentWithPaypal", "Paypal", model);
                }

                //Thiết lập paymetnstatus

                string paymentStatus = "Chưa thanh toán";
                switch(model.PaymentMethod)
                {
                    case "Tiền mặt": paymentStatus = "Thanh toán tiền mặt"; break;
                    case "Paypal": paymentStatus = "Thanh toán Paypal"; break;
                    case "Trả góp": paymentStatus = "Trả góp"; break;
                    default: paymentStatus = "Chưa thanh toán"; break;

                }

                //Tạo đơn hàng và chi tiết liên quan
                var order = new Order
                {
                    CustomerID = customer.CustomerID,
                    OrderDate = model.OrderDate,
                    TotalAmount = model.TotalAmount,
                    PaymentStatus = paymentStatus,
                    PaymentMethod = model.PaymentMethod,
                    DeliveryMethod = model.DeliveryMethod,
                    ShippingAddress = model.ShippingAddress,
                    OrderDetails = cart.Select(item => new OrderDetail
                    {
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        TotalPrice = item.TotalPrice
                    }).ToList(),
                };
                //Lưu vào CSDL
                db.Orders.Add(order);
                db.SaveChanges();
                //Xoá giỏ hàng sau khi đặt thành công
                Session["Cart"] = null;
                return RedirectToAction("OrderSuccess", new {id=order.OrderID});
            }
            return View(model);
        }
    }
}