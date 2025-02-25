using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCStudentManagementCRUD.ViewModels;
using MVCStudentManagementCRUD.Services;
using MVCStudentManagementCRUD.Models.Students;
using MVCStudentManagementCRUD.Services.Degrees;
using MVCStudentManagementCRUD.Services.Ethnicities;
using MVCStudentManagementCRUD.Services.Genders;
using MVCStudentManagementCRUD.ViewModels.Students;

namespace MVCStudentManagementCRUD.Areas.Students.Controllers
{
    // Inheritance

    // SRP: This controller is only responsible for handling student related requests

    // Liskov Substitution Principle: This controller can be replaced by any other controller that implements the same interface.

    // Interface Segregation Principle: This controller only implements the methods that are needed for handling student related requests.
    // Ensuring that clients depend only on the methods they use. 

    // Dependency Inversion Principle: This controller depends on the IStudentService interface (abstraction), not on concrete implementations of the service.
    // Dependency Injection is used to inject the concrete implementation of the service into the controller.
    // Promotes loose coupling between the controller and the service. As changes can be made to the service without needing to change the controller.
    public class StudentController : Controller
    {
        // Define the services that will be used in the controller
        private readonly IStudentService _studentService;
        private readonly IDegreeService _degreeService;
        private readonly IEthnicityService _ethnicityService;
        private readonly IGenderService _genderService;

        // Initiate the services in the constructor
        public StudentController(IStudentService studentService, IDegreeService degreeService, IEthnicityService ethnicityService, IGenderService genderService)
        {
            _studentService = studentService;
            _degreeService = degreeService;
            _ethnicityService = ethnicityService;
            _genderService = genderService;
        }

        /// <summary>
        /// Displays the list of students.
        /// </summary>
        /// <returns>View with the list of students.</returns>
        public ActionResult Index()
        {
            // Create a dictionary for degree, ethnicity and gender. This will be used to display the name rather than Id.
            var degreeDictionary = _degreeService.GetAllDegrees().ToDictionary(d => d.Id, d => d.Name);
            ViewBag.DegreeDictionary = degreeDictionary;
            var ethnicityDictionary = _ethnicityService.GetAllEthnicities().ToDictionary(e => e.Id, e => e.Name);
            ViewBag.EthnicityDictionary = ethnicityDictionary;
            var genderDictionary = _genderService.GetAllGenders().ToDictionary(g => g.Id, g => g.Name);
            ViewBag.GenderDictionary = genderDictionary;

            return View();
        }

        /// <summary>
        /// Retrieves all students in the database.
        /// </summary>
        /// <returns>JSON result with the list of students.</returns>
        public ActionResult GetStudents()
        {
            var students = _studentService.GetAllStudents();
            var studentVM = new StudentViewModel
            {
                Students = students
            };
            return Json(new { data = students }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Displays the list of students as a partial view.
        /// </summary>
        /// <returns>Partial view with the list of students.</returns>
        [HttpGet]
        public ActionResult StudentList()
        {
            var students = _studentService.GetAllStudents();
            return PartialView("_StudentList", students);
        }

        /// <summary>
        /// Displays the form to create or edit a student.
        /// </summary>
        /// <param name="id">The ID of the student. Default is 0 for creating a new student.</param>
        /// <returns>Partial view with the form to create or edit a student.</returns>
        [HttpGet]
        public ActionResult Upsert(int id = 0)
        {
            ViewBag.GenderOptions = _genderService.GetGenderOptions();
            ViewBag.DegreeOptions = _degreeService.GetDegreeOptions();
            ViewBag.EthnicityOptions = _ethnicityService.GetEthnicityOptions();
            var degreeDictionary = _degreeService.GetAllDegrees().ToDictionary(d => d.Id, d => d.Name);
            ViewBag.DegreeDictionary = degreeDictionary;

            if (id == 0)
            {
                StudentBO student = new StudentBO
                {
                    DateOfBirth = new DateTime(2000, 01, 01)
                };
                return PartialView("_Upsert", student);
            }
            else
            {
                var existingStudent = _studentService.GetStudentById(id);
                if (existingStudent == null)
                {
                    return Json(new { success = "false", message = "Student not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                // Get the current student with this Id, then create a studentBO with this Id.
                return PartialView("_Upsert", existingStudent);
            }
        }

        /// <summary>
        /// Creates or updates a student in the database.
        /// </summary>
        /// <param name="student">The student business object.</param>
        /// <returns>JSON result indicating success or failure.</returns>
        [HttpPost]
        public ActionResult Upsert(StudentBO student)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GenderOptions = _genderService.GetGenderOptions();
                ViewBag.DegreeOptions = _degreeService.GetDegreeOptions();
                ViewBag.EthnicityOptions = _ethnicityService.GetEthnicityOptions();
                return PartialView("_Upsert", student);
            }
            var result = _studentService.UpsertStudent(student);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes a student from the database.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>JSON result indicating success or failure.</returns>
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _studentService.DeleteStudent(id);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieves the details of a specific student by ID.
        /// </summary>
        /// <param name="id">The ID of the student.</param>
        /// <returns>Partial view with the student details.</returns>
        public ActionResult GetStudentDetails(int id)
        {
            var teachers = _studentService.GetTeachersByStudent(id);
            List<string> teacherNames = new List<string>();
            foreach (var teacher in teachers)
            {
                teacherNames.Add(teacher.FirstName + " " + teacher.LastName);
            }

            StudentBO student = _studentService.GetStudentById(id);
            string studentName = student.FirstName + " " + student.LastName;

            var viewModel = new StudentDetailsViewModel
            {
                Name = studentName,
                TeacherNames = teacherNames
            };

            return PartialView("_StudentDetails", viewModel);
        }
    }
}