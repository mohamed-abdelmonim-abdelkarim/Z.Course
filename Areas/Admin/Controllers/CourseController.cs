using AutoMapper;
using Courses.Data;
using Courses.Models;
using Courses.Services;
using System;

using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly Mapper mapper;
        public CourseController()
        {
            _courseService = new CourseService();
            mapper = (Mapper)AutoMapperConfig.Mapper;
        }

        // GET: Admin/Course
        public ActionResult Index(int? ID=null)
        {
            var courses = _courseService.ReadAll(ID);
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

        // GET: Admin/Course/Create
        public ActionResult Create()
        {
            var courseModel = new CourseModel
            {
                Trainer = _courseService.GetAllTrainers().Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(),
                    Text = t.Name
                }).ToList(),

                Catogery = _courseService.ReadAll().Select(c => new SelectListItem
                {
                    Value = c.Catogery_Id.ToString(),
                    Text = c.Catogery.Name
                }).ToList()
            };

            return View(courseModel);
        }

        // POST: Admin/Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseModel model)
        {
            
              
            if (ModelState.IsValid)
            {
                model.Image_Id = saveimagefile(model.imagefile);
                var newCourse = new Course
                {
                    Name = model.Name,
                    Description = model.Description,
                    Catogery_Id = model.Catogery_Id,
                    Trainer_Id = model.Trainer_Id,
                    Creation_Date = DateTime.Now
                };

                var result = _courseService.Create(newCourse);

                if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "A course with the same name already exists.");
                }
            }

            model.Trainer = _courseService.GetAllTrainers().Select(t => new SelectListItem
            {
                Value = t.ID.ToString(),
                Text = t.Name
            }).ToList();

            model.Catogery = _courseService.ReadAll().Select(c => new SelectListItem
            {
                Value = c.Catogery_Id.ToString(),
                Text = c.Catogery.Name
            }).ToList();

            return View(model);
        }

        // GET: Admin/Course/Edit/{id}
        public ActionResult Edit(int id)
        {
            // جلب الدورة بناءً على المعرّف (ID)
            var existingCourse = _courseService.ReadById(id);

            if (existingCourse == null)
            {
                // في حال لم يتم العثور على الدورة
                return HttpNotFound(); // أو يمكنك عرض رسالة مخصصة للمستخدم
            }

            // إعداد النموذج لعرضه في واجهة المستخدم
            var courseModel = new CourseModel
            {
                Catogery_Id = existingCourse.Catogery_Id,
                Name = existingCourse.Name,
                Description = existingCourse.Description,
                Trainer_Id = existingCourse.Trainer_Id,
                Image_Id = existingCourse.Image_Id,

                // تعبئة قائمة المدربين
                Trainer = _courseService.GetAllTrainers().Select(t => new SelectListItem
                {
                    Value = t.ID.ToString(),
                    Text = t.Name
                }).ToList(),

                // تعبئة قائمة التصنيفات
                Catogery = _courseService.ReadAll().Select(c => new SelectListItem
                {
                    Value = c.Catogery_Id.ToString(),
                    Text = c.Catogery.Name
                }).ToList()
            };

            return View(courseModel);
        }



        // POST: Admin/Course/Create
        [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit(CourseModel model)
{
    if (model.imagefile == null || model.imagefile.ContentLength == 0)
        return View(model);

    if (ModelState.IsValid)
    {
        // جلب الدورة الأصلية من قاعدة البيانات
        var existingCourse = _courseService.ReadById(model.ID);

        if (existingCourse == null)
        {
            ModelState.AddModelError("", "The course no longer exists.");
            return View(model);
        }

        // تحديث البيانات
        existingCourse.Name = model.Name;
        existingCourse.Description = model.Description;
        existingCourse.Catogery_Id = model.Catogery_Id;
        existingCourse.Trainer_Id = model.Trainer_Id;

        // حفظ الصورة إذا كانت مرفوعة
        existingCourse.Image_Id = saveimagefile(model.imagefile, model.Image_Id);

        // تحديث الدورة في قاعدة البيانات
        var result = _courseService.Update(existingCourse);

        if (result >= 1)
        {
            return RedirectToAction("Index");
        }
        else if (result == -2)
        {
            ModelState.AddModelError("", "A course with the same name already exists.");
        }
    }

    // إعادة تعبئة القوائم عند عرض الأخطاء
    model.Trainer = _courseService.GetAllTrainers().Select(t => new SelectListItem
    {
        Value = t.ID.ToString(),
        Text = t.Name
    }).ToList();

    model.Catogery = _courseService.ReadAll().Select(c => new SelectListItem
    {
        Value = c.Catogery_Id.ToString(),
        Text = c.Catogery.Name
    }).ToList();

    return View(model);
}



        // GET: Admin/Course/Delete/{id}
        public ActionResult Delete(int id)
        {
            var course = _courseService.ReadById(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var courseModel = new CourseModel
            {
                ID = course.ID,
                Name = course.Name,
                Description = course.Description,
                CatogeryName = course.Catogery?.Name
            };

            return View(courseModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id != null)
            {
                var deleted = _courseService.Delete(id.Value);
                if (deleted)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id = id });
            }
            return HttpNotFound();
        }
        private string saveimagefile(HttpPostedFileBase imagefile,string currimageid="")
        {
            if (imagefile != null)
            {
                if(!string.IsNullOrEmpty(currimageid))
                {
                    string oldfilepath = Server.MapPath($"~/Uploads/Courses/{currimageid}");
                    System.IO.File.Delete(oldfilepath);
                }
                //var filename = "";
                var fileextenison = Path.GetExtension(imagefile.FileName);
                var ImageGuid = Guid.NewGuid().ToString();
                string imageid = ImageGuid + fileextenison;
                //saving files
                string filepath = Server.MapPath($"~/Uploads/Courses/{imageid}");
                imagefile.SaveAs(filepath);
                //delete old file
                if (!string.IsNullOrEmpty(currimageid))
                {
                    string oldfilepath = Server.MapPath($"~/Uploads/Courses/{currimageid}");
                    System.IO.File.Delete(oldfilepath);
                }
                return imageid;
            }
            return currimageid;
        }

    }
}
