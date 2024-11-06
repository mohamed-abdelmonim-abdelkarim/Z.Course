using Courses.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Models
{
    public class CourseModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public System.DateTime Creation_Date { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name="Category")]
        public int Catogery_Id { get; set; }
        public string CatogeryName { get; set; }
        [Required]
        [Display(Name = "Trainer")]
        public Nullable<int> Trainer_Id { get; set; }
        public string TrainerName { get; set; }
        private string _image_iddef {  get; set; }
        public int Course_Id { get; set; } // Primary Key

        public virtual ICollection<Course_Lesson> Course_Lessons { get; set; }
        public string Image_Id {
            set
            {
                _image_iddef = (string.IsNullOrWhiteSpace(value)) ? "empty.jpg" : value;

            }
            get
            {
                return _image_iddef;
            }
        }
        [Required]
        public HttpPostedFileBase imagefile { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public IEnumerable<SelectListItem> Catogery { get; set; }
        public IEnumerable<SelectListItem> Trainer { get; set; }
    }
}