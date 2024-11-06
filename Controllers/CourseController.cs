using AutoMapper;
using Courses.Data;
using Courses.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseService courseService;
        private readonly IMapper mapper;
        private readonly Cources_DbEntities dbEntities;
        public CourseController()
        {
            courseService = new CourseService();
            mapper = (Mapper)AutoMapperConfig.Mapper;
            dbEntities = new Cources_DbEntities();
        }
        // GET: Course
        public ActionResult Index(int? id)
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
        public ActionResult Info(int? id)
        {
            if (id == null || id == 0)
            {
                return HttpNotFound("Please enter a correct course number!");
            }

            var courseInfo = courseService.GetCourse(id.Value);

            if (courseInfo == null)
            {
                return HttpNotFound("Course not found. Please enter a correct course number!");
            }

            // Return a list containing a single CourseModel
            var courseList = new List<CourseModel>
            {
                new CourseModel
                {
                    ID = courseInfo.ID,
                    Name = courseInfo.Name,
                    Creation_Date = courseInfo.Creation_Date,
                    Description = courseInfo.Description,
                    Catogery_Id = courseInfo.Catogery_Id,
                    CatogeryName = courseInfo.Catogery?.Name,
                    TrainerName = courseInfo.Trainer?.Name,
                    Image_Id=courseInfo.Image_Id,


                }
            };

                    return View(courseList);
        }
        public ActionResult Subscribe(int? cid)
        {
            if(!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { returnUrl = $"/Course/Subscribe/{cid}" });
            }
            
                if (ModelState.IsValid)
                {
                    // Here, you can process the subscription data.
                    // For example, save the data to a database or send a confirmation email.

                    // Redirect to a thank-you page or display a success message
                    TempData["Message"] = "Thank you for subscribing!";
                return RedirectToAction("Details", new { id = cid });
            }

                // If the data is not valid, return to the form
                return View("Index");
        }

            // GET: ThankYou
            public ActionResult ThankYou()
            {
                ViewBag.Message = TempData["Message"];
                return View();
            }
        public ActionResult Details()
        {
             var courses =dbEntities.Courses.ToList();
            var courseModels = courses.Select(c => new CourseModel
            {
                ID = c.ID,
                Name = c.Name,
                Description = c.Description,
                Image_Id = c.Image_Id,
                // أضف باقي الخصائص المطلوبة هنا
            }).ToList();
            return View(courseModels);
        }
    }


    }
