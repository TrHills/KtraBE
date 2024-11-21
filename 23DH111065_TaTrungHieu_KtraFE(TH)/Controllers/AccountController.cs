using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _23DH111065_TaTrungHieu_KtraFE_TH_.Models;
using _23DH111065_TaTrungHieu_KtraFE_TH_.Models.ViewModel;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace _23DH111065_TaTrungHieu_KtraFE_TH_.Controllers
{
    public class AccountController : Controller
    {
        private MyStoreEntities db = new MyStoreEntities();
        // GET: Account
        public ActionResult Register()
        {
            return View();
        }
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // kiểm tra xem tên đăng nhập tồn tại chưa
                var existingUser = db.Users.SingleOrDefault(u => u.Username == model.Username);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại!");
                    return View(model);
                }

                var user = new User
                {
                    Username = model.Username,
                    Password = model.Password,    // Lưu ý: nên mã hoá mật khẩu trước
                    UserRole = "Customer"
                };
                db.Users.Add(user); // tạo bảo ghi
                var customer = new Customer
                {
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    CustomerPhone = model.CustomerPhone,
                    CustomerAddress = model.CustomerAddress,
                    Username = model.Username,
                };

                db.Customers.Add(customer); // lưu tài khoản và thông tin vào CSDL

                db.SaveChanges();

                return RedirectToAction("Index", "Home");
                
            }
            
            return View(model);
        }
        
        public ActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.SingleOrDefault(u => u.Username == model.Username
                && u.Password == model.Password
                && u.UserRole == "Customer");
                
                if (user != null)
                {
                    //Lưu trạng thía đăng nhập vào session
                    Session["Username"] = user.Username;
                    Session["UserRole"] = user.UserRole;

                    //Lưu thông tin xác thuccjw
                    FormsAuthentication.SetAuthCookie(user.Username, false);
                    ViewBag.Username = user.Username;
                    return RedirectToAction("Index", "Home");
                }
                
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập không đúng.");
                }
                
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session["User"] = null;
            return RedirectToAction("Index", "Home");
        }
        
        
    }
}