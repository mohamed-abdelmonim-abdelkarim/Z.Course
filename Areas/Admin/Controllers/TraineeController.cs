using AutoMapper;
using Courses.Data;
using Courses.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TraineeController : Controller
    {
        private readonly TraineeCourseService traineecourseService;
        private readonly IMapper mapper;
        public TraineeController()
        {
            traineecourseService = new TraineeCourseService();
            mapper = AutoMapperConfig.Mapper;
        }
        // GET: Admin/Trainee
        public ActionResult Index(int? c_id)
        {
            if (c_id == null) 
            {
                return RedirectToAction("Index", "Default", new {area="admin"});
            }
            var trainnes = traineecourseService.GetTrainees(c_id.Value);
           var ctrainees= mapper.Map<IEnumerable<Trainee_Courses>,IEnumerable<traineeCourseModel>>(trainnes);
            return View(ctrainees);
        }
    }
}