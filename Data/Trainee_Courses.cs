//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Courses.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Trainee_Courses
    {
        public int trainee_Id { get; set; }
        public int Course_id { get; set; }
        public System.DateTime Registration_Date { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Trainee Trainee { get; set; }
    }
}
