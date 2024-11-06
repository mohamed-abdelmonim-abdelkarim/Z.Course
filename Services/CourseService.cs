using Courses.Data;
using Courses.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace Courses.Services
{

    public interface ICourseService
    {
        int Create(Course newCourse);
        Course ReadById(int id);
        List<Course> ReadAll(int? trainerid=null);
       
        List<Trainer> GetAllTrainers();
        int Update(Course courseUpdated);
        bool Delete(int id);
        Course GetCourse(int id);
    }
    public class CourseService: ICourseService
    {
       
            private readonly Cources_DbEntities _dbEntities;
        public List<Trainer> GetAllTrainers()
        {
            return _dbEntities.Trainers.ToList();
        }

        public CourseService()
            {
                _dbEntities = new Cources_DbEntities();
            }

        public int Create(Course newCourse)
        {
            // تحقق إذا كان اسم الدورة موجودًا بالفعل
            var courseNameExists = _dbEntities.Courses
                .Where(x => x.Name.ToLower() == newCourse.Name.ToLower())
                .Any();

            if (courseNameExists)
            {
                return -2; // اسم الدورة موجود بالفعل
            }

            // التحقق من صحة Catogery_Id قبل الحفظ
            //var categoryExists = _dbEntities.Catogeries
            //    .Where(c => c.ID == newCourse.Catogery_Id)
            //    .Any();

            //if (!categoryExists)
            //{
            //    return -3; // الفئة غير موجودة
            //}

            // التحقق من التاريخ إذا كان غير صحيح، اضبطه على التاريخ الحالي
            //if (newCourse.Creation_Date == DateTime.MinValue)
            //{
            //    newCourse.Creation_Date = DateTime.Now;
            //}

            _dbEntities.Courses.Add(newCourse);
            return _dbEntities.SaveChanges();
        }



        public Course ReadById(int id)
            {
                return _dbEntities.Courses.Find(id);
            }

        public List<Course> ReadAll(int? trainerid = null)
        {
            if (trainerid != null)
            {
                // Filter by the trainer ID
                return _dbEntities.Courses.Where(c => c.Trainer_Id == trainerid).ToList();
            }
            else
            {
                // If no trainer ID is provided, return all courses or handle it accordingly
                return _dbEntities.Courses.ToList();
            }
        }



        public int Update(Course courseUpdated)
        {
            var courseName = courseUpdated.Name.ToLower();

            // Check if another course with the same name exists, excluding the current one
            var isNameExists = _dbEntities.Courses
                .Where(x => x.ID != courseUpdated.ID)
                .Any(x => x.Name.ToLower() == courseName);

            if (isNameExists)
            {
                return -2; // The name already exists for another course
            }

            try
            {
                // Fetch the existing course from the database
                var existingCourse = _dbEntities.Courses.Find(courseUpdated.ID);

                if (existingCourse == null)
                {
                    return -1; // Course not found (may have been deleted)
                }

                // Update course properties
                existingCourse.Name = courseUpdated.Name;

                // Save changes and handle concurrency
                return _dbEntities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency exception (retry logic or notify user)
                // Example: Log exception, inform the user about the concurrency issue
                return -3; // Indicates a concurrency conflict
            }
        }


        public bool Delete(int id)
            {
                var course = ReadById(id);
                if (course != null)
                {
                    _dbEntities.Courses.Remove(course);
                    return _dbEntities.SaveChanges() > 0;
                }

                return false;
            }

        public Course GetCourse(int id)
        {
            return _dbEntities.Courses.Find(id);
        }
    }
    }

