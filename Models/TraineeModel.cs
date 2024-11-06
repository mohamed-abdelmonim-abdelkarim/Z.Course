using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Models
{
    public class traineeCourseModel
    {
        public int courseId {  get; set; }
        public DateTime registrationdata {  get; set; }
        public TraineeModel trainee { get; set; }   
    }
    public class TraineeModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}