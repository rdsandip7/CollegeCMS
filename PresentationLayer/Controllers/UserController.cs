using System;
using BusinessLayer;
using CoreLayer;
using PresentationLayer.Models;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/
        UserService userService = new UserService();
        EmployeeService employeeService = new EmployeeService();
        private static RNGCryptoServiceProvider m_cryptoServiceProvider = null;
        private const int SALT_SIZE = 24;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveUser(UserViewModel userDetail)
        {
            int userId = 0;
            User user;
            if (ModelState.IsValid)
            {
                User userDataTable = userService.GetUserByUsername(userDetail.Username);
                Employee employee = employeeService.GetEmployeeByEmail(userDetail.Email);
                if (userDataTable != null)
                {
                    ModelState.AddModelError("UserName", "User Name is Already Exist Please. Try Another one ");
                    return View("Register", userDetail);
                }
                else
                {
                    user = new User()
                    {
                        Username = userDetail.Username,
                        Password = userDetail.Password,
                        InsertedDate = DateTime.Now
                    };
                    userId = userService.SaveUser(user);
                }

                if (employee != null)
                {
                    if (employee.UserId != null)
                    {
                        ModelState.AddModelError("Email", "Email is Already Exist Please. Try Another one ");
                        return View("Register", userDetail);
                    }
                    else
                    {
                        employee.UserId = userId;
                        employeeService.UpdateEmployee(employee);
                    }

                }

            }
            return View(userDetail);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLoginViewModel userLoginData)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Login data is incorrect!");
                return RedirectToAction("Index", "Home", null);
            }
            User userDataSavedInDataBase = userService.GetUserByUsername(userLoginData.UserName);
            if (userDataSavedInDataBase != null)
            {
                if (string.Equals(userDataSavedInDataBase.Password, userLoginData.Password))
                {
                    FormsAuthentication.SetAuthCookie(userLoginData.UserName, userLoginData.RememberMe);
                    return RedirectToAction("Dashboard", "Home", null);
                }
            }

            return RedirectToAction("Index", "Home", null);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            //if (Request.Cookies["user"] != null)
            //{
            //    HttpCookie user = new HttpCookie("user");
            //    user.Expires = DateTime.Now.AddDays(-1d);
            //    Response.Cookies.Add(user);
            //    Session.Clear();
            //}
            //else
            //{
            //    Session.Clear();
            //}
            return RedirectToAction("Index", "Home", null);
        }

        private String getEncryptedPassword(string password)
        {
            //string salt = getSaltString();
            string hashedString = getEncryptedPassword(password);
            //string encryptedPassword = string.Concat(salt, hashedString);
            //return encryptedPassword;
            return hashedString;

        }

        private string getEncryptedString(string text)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytD2e = uEncode.GetBytes(text);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(bytD2e);
            return Convert.ToBase64String(hash);
        }
    }
}
