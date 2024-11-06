using Courses.Areas.Admin.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account/Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel logininfo) 
        {
            var adminservice = new AdminService();
           var isloggedin= adminservice.login(logininfo.Email, logininfo.Password);
            if(isloggedin)
            {
                return RedirectToAction("Index","Default");
            }
            else
            {
                logininfo.Message = "Email Or Password Incorrect!";
                return View(logininfo);
            }
        }
    }
}