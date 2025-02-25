using MVCStudentManagementCRUD.Models.Teachers;
using MVCStudentManagementCRUD.Services.Degrees;
using MVCStudentManagementCRUD.Services.Ethnicities;
using MVCStudentManagementCRUD.Services.Genders;
using MVCStudentManagementCRUD.Services.Teachers;
using MVCStudentManagementCRUD.ViewModels.Degrees;
using MVCStudentManagementCRUD.ViewModels.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Teachers.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IDegreeService _degreeService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IGenderService _genderService;

        public TeacherController(ITeacherService teacherService, IDegreeService degreeService, IEthnicityService ethnicityService, IGenderService genderService)
        {
            _teacherService = teacherService;
            _degreeService = degreeService;
            _ethnicityService = ethnicityService;
            _genderService = genderService;
        }
        // GET: Teachers/Teacher
        public ActionResult Index()
        {
            //Create a dictionary for degree, ethnicity and gender. This will be used to display the name rather than Id.
            var degreeDictionary = _degreeService.GetAllDegrees().ToDictionary(d => d.Id, d => d.Name);
            ViewBag.DegreeDictionary = degreeDictionary;
            var ethnicityDictionary = _ethnicityService.GetAllEthnicities().ToDictionary(e => e.Id, e => e.Name);
            ViewBag.EthnicityDictionary = ethnicityDictionary;
            var genderDictionary = _genderService.GetAllGenders().ToDictionary(g => g.Id, g => g.Name);
            ViewBag.GenderDictionary = genderDictionary;

            return View();
        }

        public ActionResult GetTeachers()
        {
            var teachers = _teacherService.GetAllTeachers();
            var teacherVM = new TeacherViewModel
            {
                Teachers = teachers
            };
            return Json(new { data = teachers }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TeacherList()
        {
            var teachers = _teacherService.GetAllTeachers();
            return PartialView("_TeacherList", teachers);
        }

        [HttpGet]
        //Default value of int is 0
        public ActionResult Upsert(int id = 0)
        {
            ViewBag.GenderOptions = _genderService.GetGenderOptions();
            ViewBag.DegreeOptions = _degreeService.GetDegreeOptions();
            ViewBag.EthnicityOptions = _ethnicityService.GetEthnicityOptions();
            var degreeDictionary = _degreeService.GetAllDegrees().ToDictionary(d => d.Id, d => d.Name);
            ViewBag.DegreeDictionary = degreeDictionary;

            if (id == 0)
            {
                TeacherBO teacher = new TeacherBO
                {
                    DateOfBirth = new DateTime(2000, 01, 01)
                };
                return PartialView("_Upsert", teacher);
            }
            else
            {
                var existingTeacher = _teacherService.GetTeacherById(id);
                if (existingTeacher == null)
                {
                    return Json(new { success = "false", message = "Teacher not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                return PartialView("_Upsert", existingTeacher);
            }
        }

        [HttpPost]
        public ActionResult Upsert(TeacherBO teacher)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GenderOptions = _genderService.GetGenderOptions();
                ViewBag.DegreeOptions = _degreeService.GetDegreeOptions();
                ViewBag.EthnicityOptions = _ethnicityService.GetEthnicityOptions();
                return PartialView("_Upsert", teacher);
            }
            var result = _teacherService.UpsertTeacher(teacher);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _teacherService.DeleteTeacher(id);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeacherDetails(int id)
        {
            var students = _teacherService.GetStudentsByTeacher(id);
            List<string> studentNames = new List<string>();
            foreach (var student in students)
            {
                studentNames.Add(student.FirstName + " " + student.LastName);
            }

            var teacher = _teacherService.GetTeacherById(id);
            var name = teacher.FirstName + " " + teacher.LastName;

            var viewModel = new TeacherDetailsViewModel
            {
                Name = name,
                StudentNames = studentNames
            };

            return PartialView("_TeacherDetails", viewModel);
        }

    }
}