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
    public class CategoryController : System.Web.Mvc.Controller
    {
        private readonly CatogeryService _catogeryService;
        private readonly IMapper mapper;
        public CategoryController()
        {
            _catogeryService = new CatogeryService();
            mapper = AutoMapperConfig.Mapper;

        }
        // GET: Admin/Category
        public ActionResult Index()
        {
            var cat = _catogeryService.ReadAll();
            var catlist = cat.Select(item => new CatogeryModel
            {
                Id = item.ID,
                Name = item.Name,
                ParentName = item.Catogery2 != null ? item.Catogery2.Name : "No Parent" // تعيين ParentName بشكل صحيح
            }).ToList();

            return View(catlist);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var cat = new CatogeryModel();
            initmaincat(null,ref cat);
            return View(cat);
        }
        [HttpPost]
        public ActionResult Create(CatogeryModel cat)
        {
            
            
               int creationres= _catogeryService.Create(new Data.Catogery
                {
                   Name = cat.Name,
                   Parent_Id=cat.ParentId
                });
                if (creationres==-2)
                {
                initmaincat(null,ref cat);
                ViewBag.Message = "Category Name is Exists!";
                    return View(cat);
                }
              
            
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            if(id==null || id==0)
            {
                return RedirectToAction("Index", "Home");
            }
            var currentcategory = _catogeryService.readbyid(id.Value);
            if (currentcategory==null)
            {
               return HttpNotFound($"this category {(id)} not found!");
            }
            var cm = new CatogeryModel
            {
                Id = currentcategory.ID,
                Name = currentcategory.Name,
               ParentId=currentcategory.Parent_Id
               
           
            };
            initmaincat(currentcategory.ID,ref cm);
            return View(cm);
        }
        [HttpPost]
        public ActionResult Edit(CatogeryModel cm)
        {
            if (ModelState.IsValid)
            {
                // تحديث الفئة
                var uc = new Catogery
                {
                    ID = cm.Id,
                    Name = cm.Name,
                    Parent_Id = cm.ParentId // تأكد من تضمين ParentId
                };

                var result = _catogeryService.Updatee(uc);

                if (result == -2)
                {
                    ViewBag.Message = "Category Name already exists!";
                    initmaincat(cm.Id, ref cm);
                    return View(cm);
                }
                else if (result > 0)
                {
                    ViewBag.Success = true;
                    ViewBag.Message = $"Category {cm.Id} updated successfully";
                    return RedirectToAction("Index"); // بعد التعديل، يمكنك إعادة التوجيه إلى صفحة أخرى
                }
                else
                {
                    ViewBag.Message = "An error occurred!";
                }
            }

            // إذا كانت هناك أخطاء في النموذج أو التحديث فشل، نعيد النموذج إلى الصفحة
            initmaincat(cm.Id, ref cm);
            return View(cm);
        }

        public ActionResult Delete(int? id)
        {
            if(id !=null)
            {
                var catget = _catogeryService.readbyid(id.Value);
                var catinfo = new CatogeryModel
                {
                    Id=catget.ID,
                    Name=catget.Name,
                    ParentName=catget.Catogery2?.Name
                };
                return View(catinfo);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int? Id)
        { 
            if(Id !=null)
            {
                
              var deleted=  _catogeryService.Delete(Id.Value);
                if(deleted)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Delete", new { id = Id });
            }
            return HttpNotFound();
        }
        private void initmaincat(int? cattoexeculde,ref CatogeryModel catogeryModel)
        {
             var catlist = _catogeryService.ReadAll();
            if(cattoexeculde!=null)
            {
                var cur=catlist.Where(c=>c.ID==cattoexeculde).FirstOrDefault();
                catlist.Remove(cur);
            }
            catogeryModel.maincatrgory = new SelectList(catlist, "Id", "Name");
            
        }
    }
}
