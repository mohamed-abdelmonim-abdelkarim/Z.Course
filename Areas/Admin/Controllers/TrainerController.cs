using AutoMapper;
using Courses.Data;
using Courses.Models;
using Courses.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Courses.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TrainerController : Controller
    {
        private readonly TrainerService _trainerService;
        private readonly IMapper _mapper;

        public TrainerController()
        {
            _trainerService = new TrainerService();
            _mapper = AutoMapperConfig.Mapper;
        }

        // GET: Admin/Trainer
        public ActionResult Index()
        {
            var trainers = _trainerService.ReadAll();
            var trainerList = trainers.Select(trainer => new TrainerModel
            {
                ID = trainer.ID,
                Name = trainer.Name,
                Email = trainer.Email,
                Description = trainer.Description,
                Website = trainer.Website,
                Creation_Dtae = trainer.Creation_Dtae
            }).ToList();

            return View(trainerList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new TrainerModel());
        }

        [HttpPost]
        public ActionResult Create(TrainerModel trainerModel)
        {
            if (ModelState.IsValid)
            {
                var creationResult = _trainerService.Create(new Trainer
                {
                    Name = trainerModel.Name,
                    Email = trainerModel.Email,
                    Description = trainerModel.Description,
                    Website = trainerModel.Website,
                    Creation_Dtae = DateTime.Now
                });

                if (creationResult == -2)
                {
                    ViewBag.Message = "Trainer with this email already exists!";
                    return View(trainerModel);
                }

                return RedirectToAction("Index");
            }

            return View(trainerModel);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return RedirectToAction("Index");
            }

            var trainer = _trainerService.ReadById(id.Value);
            if (trainer == null)
            {
                return HttpNotFound($"Trainer with ID {id} not found!");
            }

            var trainerModel = new TrainerModel
            {
                ID = trainer.ID,
                Name = trainer.Name,
                Email = trainer.Email,
                Description = trainer.Description,
                Website = trainer.Website,
                Creation_Dtae = trainer.Creation_Dtae
            };

            return View(trainerModel);
        }

        [HttpPost]
        public ActionResult Edit(TrainerModel trainerModel)
        {
            if (ModelState.IsValid)
            {
                var result = _trainerService.Update(new Trainer
                {
                    ID = trainerModel.ID,
                    Name = trainerModel.Name,
                    Email = trainerModel.Email,
                    Description = trainerModel.Description,
                    Website = trainerModel.Website,
                    Creation_Dtae = trainerModel.Creation_Dtae ?? DateTime.Now
                });

                if (result == -2)
                {
                    ViewBag.Message = "Email already exists for another trainer!";
                    return View(trainerModel);
                }
                else if (result > 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = "An error occurred while updating the trainer!";
                }
            }

            return View(trainerModel);
        }

      

            // GET: Admin/Trainer/Delete/2
            [HttpGet]
            public ActionResult Delete(int? id)
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                var trainer = _trainerService.ReadById(id.Value);
                if (trainer == null)
                {
                    return HttpNotFound();
                }

                var trainerModel = new TrainerModel
                {
                    ID = trainer.ID,
                    Name = trainer.Name,
                    Email = trainer.Email,
                    Description = trainer.Description,
                    Website = trainer.Website,
                    Creation_Dtae = trainer.Creation_Dtae
                };

                return View(trainerModel);
            }

            // POST: Admin/Trainer/Delete/2
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                var result = _trainerService.Delete(id);
                if (result)
                {
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Delete", new { id });
            }
        }

    
}
