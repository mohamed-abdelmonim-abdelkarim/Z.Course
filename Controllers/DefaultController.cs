using Courses.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Controllers
{
    public class DefaultController : Controller
    {
        private readonly CourseService courseService;
        public DefaultController()
        {
            courseService = new CourseService();
        }
        // GET: Default
        public ActionResult Index(int? id=null)
        {
            var courses = courseService.ReadAll(id);
            var courseList = courses.Select(course => new CourseModel
            {
                ID = course.ID,
                Name = course.Name,
                Creation_Date = course.Creation_Date,
                Description = course.Description,
                Catogery_Id = course.Catogery_Id,
                CatogeryName = course.Catogery?.Name,
                TrainerName = course.Trainer?.Name
            }).ToList();

            return View(courseList);
        }
    }
}