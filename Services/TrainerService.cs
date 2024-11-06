using Courses.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Courses.Services
{
    public interface ITrainerService
    {
        int Create(Trainer newTrainer);
        int Update(Trainer updatedTrainer);
        Trainer ReadById(int id);
        List<Trainer> ReadAll();
        bool Delete(int id);
    }

    public class TrainerService : ITrainerService
    {
        private readonly Cources_DbEntities _dbEntities;

        public TrainerService()
        {
            _dbEntities = new Cources_DbEntities();
        }

        public int Create(Trainer newTrainer)
        {
            try
            {
                var emailExists = _dbEntities.Trainers.Any(x => x.Email.ToLower() == newTrainer.Email.ToLower());
                if (emailExists) return -2; // Email already exists

                _dbEntities.Trainers.Add(newTrainer);
                return _dbEntities.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                string fullErrorMessage = string.Join("; ", errorMessages);
                throw new DbEntityValidationException(fullErrorMessage, ex.EntityValidationErrors);
            }


        }

        public int Update(Trainer updatedTrainer)
        {
            // Validate if the email already exists for a different trainer
            var emailExists = _dbEntities.Trainers
                .Where(x => x.ID != updatedTrainer.ID)
                .Any(x => x.Email.ToLower() == updatedTrainer.Email.ToLower());
            if (emailExists) return -2; // Email exists for another trainer

            // Ensure Creation_Date is within the SQL datetime range (post 1753-01-01)
            if (updatedTrainer.Creation_Dtae < new DateTime(1753, 1, 1))
            {
                updatedTrainer.Creation_Dtae = DateTime.Now; // Set to a default valid value
            }

            // Attach and update the trainer
            _dbEntities.Trainers.Attach(updatedTrainer);
            _dbEntities.Entry(updatedTrainer).State = System.Data.Entity.EntityState.Modified;

            return _dbEntities.SaveChanges();
        }


        public Trainer ReadById(int id)
        {
            return _dbEntities.Trainers.Find(id);
        }

        public List<Trainer> ReadAll()
        {
            return _dbEntities.Trainers.ToList();
        }

        public bool Delete(int id)
        {
            var trainer = ReadById(id);
            if (trainer != null)
            {
                _dbEntities.Trainers.Remove(trainer);
                return _dbEntities.SaveChanges() > 0;
            }
            return false;
        }

    }
}
