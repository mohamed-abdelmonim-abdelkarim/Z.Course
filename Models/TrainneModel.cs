using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Courses.Models
{
    public class TrainneModel
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Please ,Write Email Address Correct Format!")]
        public string Email { get; set; }
        public string Password { get; set; }
        [Url(ErrorMessage = "Please ,Write Url Correct Format!")]
        public string website { get; set; } 
        public System.DateTime Creation_Date { get; set; }
        public Nullable<bool> Is_Active { get; set; }
    }
}