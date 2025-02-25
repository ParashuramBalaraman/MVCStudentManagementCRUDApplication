using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Services.Degrees;
using MVCStudentManagementCRUD.Services.Departments;
using MVCStudentManagementCRUD.ViewModels.Degrees;
using MVCStudentManagementCRUD.Services.StudentDegrees;
using MVCStudentManagementCRUD.Services.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Degrees.Controllers
{
    public class DegreeController : Controller
    {
        private readonly IDegreeService _degreeService;
        private readonly IDepartmentService _departmentService;
        private readonly IStudentDegreeService _studentDegreeService;

        public DegreeController(IDegreeService degreeService, IDepartmentService departmentService, IStudentDegreeService studentDegreeService)
        {
            _degreeService = degreeService;
            _departmentService = departmentService;
            _studentDegreeService = studentDegreeService;
        }

        // GET: Degrees/Degree
        public ActionResult Index()
        {
            var departmentDictionary = _departmentService.GetAllDepartments().ToDictionary(d => d.Id, d => d.Name);
            ViewBag.DepartmentDictionary = departmentDictionary;
            return View();
        }
        

        public ActionResult GetDegrees()
        {
            var degrees = _degreeService.GetAllDegrees();
            var degreeVM = new DegreeViewModel
            {
                Degrees = degrees
            };
            return Json(new { data = degrees }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DegreeList()
        {
            var degrees = _degreeService.GetAllDegrees();
            return PartialView("_DegreeList", degrees);
        }

        [HttpGet]
        //Default value of int is 0
        public ActionResult Upsert(int id = 0)
        {
            if (id == 0)
            {
                ViewBag.DepartmentOptions = _departmentService.GetDepartmentOptions();
                return PartialView("_Upsert", new DegreeBO());
            }
            else
            {
                var existingDegree = _degreeService.GetDegreeById(id);
                if (existingDegree == null)
                {
                    return Json(new { success = "false", message = "Degree not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.DepartmentOptions = _departmentService.GetDepartmentOptions();
                return PartialView("_Upsert", existingDegree);
            }
        }

        [HttpPost]
        public ActionResult Upsert(DegreeBO degree)
        {
            var result = _degreeService.UpsertDegree(degree);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _degreeService.DeleteDegree(id);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDegreeDetails(int id)
        {
            var students = _studentDegreeService.GetStudentsByDegree(id);
            var teachers = _degreeService.GetTeachersByDegree(id);
            List<string> studentNames = new List<string>();
            List<string> teacherNames = new List<string>();
            foreach (var student in students)
            {
                studentNames.Add(student.FirstName + " " + student.LastName);
            }
            foreach (var teacher in teachers)
            {
                teacherNames.Add(teacher.FirstName + " " + teacher.LastName);
            }

            var degree = _degreeService.GetDegreeById(id);

            var viewModel = new DegreeDetailsViewModel
            {
                Name = degree.Name,
                StudentNames = studentNames,
                TeacherNames = teacherNames
            };

            return PartialView("_DegreeDetails", viewModel);
        }
    }
}