using AutoMapper;
using Courses.Data;
using Courses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses
{
    public static class AutoMapperConfig
    {
        public static IMapper Mapper { get;private set; }
        public static void init()
        {
            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<Catogery, CatogeryModel>()
                    .ForMember(dst => dst.Id, src => src.MapFrom(x => x.ID))
                    .ForMember(dst => dst.ParentId, src => src.MapFrom(x => x.Catogery2.Parent_Id))
                    .ForMember(dst => dst.ParentName, src => src.MapFrom(x => x.Name))
                .ReverseMap();
                c.CreateMap<Trainer, TrainerModel>()
                  .ReverseMap();
                c.CreateMap<Course, CourseModel>()
                .ForMember(d => d.TrainerName, s => s.MapFrom(e => e.Trainer.Name))
                .ForMember(d => d.CatogeryName, s => s.MapFrom(e => e.Catogery.Name))
                .ReverseMap();
                c.CreateMap<Trainee, TraineeModel>().ReverseMap();
                c.CreateMap<Trainee_Courses, traineeCourseModel>()
                .ForMember(d => d.courseId, s => s.MapFrom(e => e.Course_id))
                 .ForMember(d => d.registrationdata, s => s.MapFrom(e => e.Registration_Date))
                  .ForMember(d => d.trainee, s => s.MapFrom(e => e.Trainee)).ReverseMap();



            });
           
            
           
            Mapper =cfg.CreateMapper();
        }
    }
}