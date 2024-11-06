using Courses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Services
{
    public interface ITraineeCourseService
    {
        IEnumerable<Trainee_Courses> GetTrainees(int? course_id=null);
    }
    public class TraineeCourseService:ITraineeCourseService
    {
        private readonly Cources_DbEntities _db;
        public TraineeCourseService()
        {
                _db = new Cources_DbEntities();
        }

        public IEnumerable<Trainee_Courses> GetTrainees(int? course_id=null)
        {
          return  _db.Trainee_Courses.Where(t =>course_id==null|| t.Course_id == course_id);

        }
    }
}