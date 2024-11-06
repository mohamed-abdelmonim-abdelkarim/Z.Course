using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Models
{
    public class CatogeryModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Category Name Is Required!")]
        [StringLength(100,MinimumLength=4,ErrorMessage ="Category Name Should Be 4 to 100")]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public SelectList maincatrgory { get; set; }
    }
}