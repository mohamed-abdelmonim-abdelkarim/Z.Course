using Courses.Data;
using Courses.Models;
using Courses.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Courses.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Data.MyIdentityUser> usermanger;
        private readonly TraineeService traineeService;
        public AccountController()
        {
            var db = new CoursesIdentityDbContext();
            var userstor=new UserStore<Data.MyIdentityUser>(db);
            usermanger=new UserManager<MyIdentityUser>(userstor);
            traineeService=new TraineeService();
        }
        // GET: Account
        [AllowAnonymous]
        public  ActionResult Login(string returnurl="")
        {
          
            return View(new LoginViewModel
            {
                ReturnUrl = returnurl,
            });
        }
        private async Task SignIn(MyIdentityUser myIdentity)
        {
           var identity=await usermanger.CreateIdentityAsync(myIdentity, DefaultAuthenticationTypes.ApplicationCookie);
            var owincontext=Request.GetOwinContext();
            var outhmanger = owincontext.Authentication;
            outhmanger.SignIn(identity);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel logindata)
        {
            if (ModelState.IsValid)
            {
                var userfind = await usermanger.FindAsync(logindata.Email, logindata.Password);
                if (userfind != null)
                {
                    await SignIn(userfind);

                    if (!string.IsNullOrEmpty(logindata.ReturnUrl))
                    {
                        return Redirect(logindata.ReturnUrl);
                    }

                    var userRoles = await usermanger.GetRolesAsync(userfind.Id);
                    var role = userRoles.FirstOrDefault();
                    if (role == "Admin")
                    {
                        return RedirectToAction("Index", "Default", new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Default");
                }
            }
            logindata.Message = "Email or Password Is Incorrect !";
            return View(logindata);
        }


        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< ActionResult> Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                var identityuser = new MyIdentityUser
                {
                    Email = rvm.Email,
                    UserName = rvm.Email,
                };
              var createres=await usermanger.CreateAsync(identityuser, rvm.Password);
                if(createres.Succeeded)
                {
                    var userid=identityuser.Id;
                    createres= usermanger.AddToRole(userid, "Trainee");
                    if (createres.Succeeded)
                    {
                       var savedtrainee= traineeService.create(new Trainee 
                        { 
                            Email=rvm.Email,
                            Name=rvm.Name,
                            Is_Active=true,
                            Creation_Date=DateTime.Now,
                        });
                        if(savedtrainee==null)
                        {
                            rvm.Message = "Error While Creating Account !";
                            return View(rvm);
                        }
                        return RedirectToAction("Index", "Default");
                    }
                   
                 
                }
                var message = createres.Errors.FirstOrDefault();
                rvm.Message = message;
                return View(rvm);
            }
            return View(rvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            var owincontext = Request.GetOwinContext();
            var outhmanger = owincontext.Authentication;
            outhmanger.SignOut("ApplicationCookie");
            Session.Abandon();
            return RedirectToAction("Index", "Default");
        }
    }
}