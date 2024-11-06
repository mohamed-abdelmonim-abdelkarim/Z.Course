using Courses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Services
{
    public interface IAdminServise
    {
        bool login(string Email, string Password);
        bool ChangePassword(string Email, string Password);
        bool ForgetPassword(string Email);
    }
    public class AdminService : IAdminServise
    {
        public Cources_DbEntities context {  get; set; }
        public AdminService()
        {
            context = new Cources_DbEntities();
        }
        public bool login(string Email, string Password)
        {
            return context.Admins.Where(z => z.Email == Email && z.Password == Password)
                 .Any();
        }
        public bool ChangePassword(string Email, string Password)
        {
            throw new NotImplementedException();
        }

        public bool ForgetPassword(string Email)
        {
            throw new NotImplementedException();
        }


    }
}