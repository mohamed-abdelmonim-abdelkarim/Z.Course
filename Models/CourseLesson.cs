using Courses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Models
{
    public class Course_Lesson
    {

        public int Course_Lesson_Id { get; set; } // Primary Key
        public int Course_Id { get; set; } // Foreign Key

        public virtual Course Course { get; set; }
    }
}