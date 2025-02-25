using MVCStudentManagementCRUD.Models.Ethnicities;
using MVCStudentManagementCRUD.Services.Ethnicities;
using MVCStudentManagementCRUD.ViewModels.Ethnicities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Ethnicities.Controllers
{
    public class EthnicityController : Controller
    {
        private readonly IEthnicityService _ethnicityService;

        public EthnicityController(IEthnicityService ethnicityService)
        {
            _ethnicityService = ethnicityService;
        }

        // GET: Ethnicities/Ethnicity
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetEthnicities()
        {
            var ethnicities = _ethnicityService.GetAllEthnicities();
            var ethnicityVM = new EthnicityViewModel
            {
                Ethnicities = ethnicities
            };
            return Json(new { data = ethnicities }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EthnicityList()
        {
            var ethnicities = _ethnicityService.GetAllEthnicities();
            return PartialView("_EthnicityList", ethnicities);
        }

        [HttpGet]
        //Default value of int is 0
        public ActionResult Upsert(int id = 0)
        {
            if (id == 0)
            {
                return PartialView("_Upsert", new EthnicityBO());
            }
            else
            {
                var existingEthnicity = _ethnicityService.GetEthnicityById(id);
                if (existingEthnicity == null)
                {
                    return Json(new { success = "false", message = "Ethnicity not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                    return PartialView("_Upsert", existingEthnicity);
            }
        }

        [HttpPost]
        public ActionResult Upsert(EthnicityBO ethnicity)
        {
            var result = _ethnicityService.UpsertEthnicity(ethnicity);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _ethnicityService.DeleteEthnicity(id);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}