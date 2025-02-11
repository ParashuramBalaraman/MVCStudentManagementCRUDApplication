using MVCStudentManagementCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCStudentManagementCRUD.ViewModels;
using MVCStudentManagementCRUD.Services;

namespace MVCStudentManagementCRUD.Areas.Students.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService) 
        {
            _studentService = studentService;
        }
        // GET: Students/Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetStudents()
        {
            var students = _studentService.GetAllStudents();
            var studentVM = new StudentViewModel
            {
                Students = students
            };
            return Json(new { data = students }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //Default value of int is 0
        public ActionResult CreateOrUpdate(int id = 0)
        {
            ViewBag.GenderOptions = _studentService.GetGenderOptions();
            ViewBag.DegreeOptions = _studentService.GetDegreeOptions();
            ViewBag.EthnicityOptions = _studentService.GetEthnicityOptions();
            if (id == 0)
            {
                Student student = new Student();
                student.DateOfBirth = new DateTime(2000, 01, 01);
                return PartialView("_CreateOrUpdate", student);
            }
            else
            {
                return PartialView("_CreateOrUpdate", _studentService.GetStudentById(id));
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(Student student)
        {
            _studentService.CreateOrUpdateStudent(student);
            return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }
    }
}