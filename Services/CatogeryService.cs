using Courses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Services
{
    public interface ICatogeryService
    {
        int Updatee(Catogery catogeryupdated);
        Catogery readbyid(int id);
        List<Catogery> ReadAll();
        int Create(Catogery newcat);
        bool Delete(int id);
    }
    public class CatogeryService : ICatogeryService
    {
        private readonly Cources_DbEntities _dbEntities;
        public CatogeryService()
        {
            _dbEntities = new Cources_DbEntities();
        }

        public int Create(Catogery newcat)
        {
            
           var catnameexit= _dbEntities.Catogeries.Where(x => x.Name.ToLower() ==newcat.Name.ToLower()).Any();
            if (catnameexit) { return -2; }
           
                _dbEntities.Catogeries.Add(newcat);
           return _dbEntities.SaveChanges();
           
        }

        //public bool Delete(int id)
        //{
        //    var cat=readbyid(id);
        //    if(cat!=null)
        //    {
        //        _dbEntities.Catogeries.Remove(cat);
        //        return _dbEntities.SaveChanges() > 0 ? true :false;
        //    }
        //    return false;
        //}
        public bool Delete(int id)
        {
            var cat = readbyid(id);
            if (cat != null)
            {
                // البحث عن الفئات الفرعية المرتبطة
                var subCategories = _dbEntities.Catogeries.Where(c => c.Parent_Id == id).ToList();

                if (subCategories.Any())
                {
                    // تعيين Parent_Id للفئات الفرعية إلى null إذا كانت الفئة الحالية هي الأب
                    foreach (var subCat in subCategories)
                    {
                        subCat.Parent_Id = null; // تعيين Parent_Id إلى null
                    }
                }

                // حذف الفئة الرئيسية بعد تحديث الفئات الفرعية أو إذا لم يكن لها فئات فرعية
                _dbEntities.Catogeries.Remove(cat);

                // حفظ التعديلات في قاعدة البيانات
                return _dbEntities.SaveChanges() > 0;
            }

            return false;
        }



        public List<Catogery> ReadAll()
        {
          return  _dbEntities.Catogeries.ToList();
        }

        public Catogery readbyid(int id)
        {
            return _dbEntities.Catogeries.Find(id);
        }

        public int Updatee(Catogery catogeryupdated)
        {
            var catname = catogeryupdated.Name.ToLower();

            // التحقق من وجود فئة أخرى بنفس الاسم مع استثناء الفئة الحالية التي نقوم بتعديلها
            var isNameExists = _dbEntities.Catogeries
                .Where(x => x.ID != catogeryupdated.ID) // استثناء الفئة الحالية من التحقق
                .Any(x => x.Name.ToLower() == catname);

            if (isNameExists)
            {
                return -2; // الاسم موجود بالفعل لفئة أخرى
            }

            // تحديث الفئة في قاعدة البيانات
            _dbEntities.Catogeries.Attach(catogeryupdated);
            _dbEntities.Entry(catogeryupdated).State = System.Data.Entity.EntityState.Modified;

            return _dbEntities.SaveChanges();
        }

    }
}