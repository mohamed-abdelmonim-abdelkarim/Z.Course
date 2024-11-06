using Courses.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Courses.Services
{
    public interface ITraineeService
    {
        Trainee create(Trainee trainee);
    }
    public class TraineeService : ITraineeService
    {
        private readonly Cources_DbEntities _db;
        public TraineeService()
        {
            _db = new Cources_DbEntities();
        }
        public Trainee create(Trainee trainee)
        {
            _db.Trainees.Add(trainee);
            int sr=_db.SaveChanges();
            if(sr > 0)
            {
                return trainee;
            }
            return null;
        }
       
    }
}