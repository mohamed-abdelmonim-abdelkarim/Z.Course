using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Courses.Models
{
    public class TrainerModel
    {
        
            public int ID { get; set; }

            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }

            [Required(ErrorMessage = "Email is required")]
            [EmailAddress(ErrorMessage = "Invalid email format")]
            public string Email { get; set; }

            public string Description { get; set; }

            [Url(ErrorMessage = "Invalid URL format")]
            public string Website { get; set; }

            public DateTime? Creation_Dtae { get; set; }  // Nullable DateTime to avoid validation issues if null
        

    }
}