using MVCStudentManagementCRUD.Models.Genders;
using MVCStudentManagementCRUD.Services.Genders;
using MVCStudentManagementCRUD.ViewModels.Genders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Genders.Controllers
{
    public class GenderController : Controller
    {
        private readonly IGenderService _genderService;

        public GenderController(IGenderService genderService)
        {
            _genderService = genderService;
        }
        // GET: Genders/Gender
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetGenders()
        {
            var genders = _genderService.GetAllGenders();
            var genderVM = new GenderViewModel
            {
                Genders = genders
            };
            return Json(new { data = genders }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GenderList()
        {
            var genders = _genderService.GetAllGenders();
            return PartialView("_GenderList", genders);
        }

        [HttpGet]
        //Default value of int is 0
        public ActionResult Upsert(int id = 0)
        {
            if (id == 0)
            {
                return PartialView("_Upsert", new GenderBO());
            }
            else
            {
                var existingGender = _genderService.GetGenderById(id);
                if (existingGender == null)
                {
                    return Json(new { success = "false", message = "Gender not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                return PartialView("_Upsert", existingGender);
            }
        }

        [HttpPost]
        public ActionResult Upsert(GenderBO gender)
        {
            var result = _genderService.UpsertGender(gender);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _genderService.DeleteGender(id);
            return Json(new { success = result.Success, message = result.Message}, JsonRequestBehavior.AllowGet);            
        }
    }
}