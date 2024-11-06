using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Data
{
    public class CoursesIdentityDbContext:IdentityDbContext<MyIdentityUser>
    {
        public CoursesIdentityDbContext():base("Courses_Connection")
        {
            
        }
    }
    public class MyIdentityUser:IdentityUser
    {

    }
}