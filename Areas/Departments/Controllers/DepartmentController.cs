using MVCStudentManagementCRUD.Models.Departments;
using MVCStudentManagementCRUD.Services.Departments;
using MVCStudentManagementCRUD.ViewModels.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Areas.Departments.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Departments/Department
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDepartments()
        {
            var departments = _departmentService.GetAllDepartments();
            var departmentVM = new DepartmentViewModel
            {
                Departments = departments
            };
            return Json(new { data = departments }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DepartmentList()
        {
            var departments = _departmentService.GetAllDepartments();
            return PartialView("_DepartmentList", departments);
        }


        [HttpGet]
        //Default value of int is 0
        public ActionResult Upsert(int id = 0)
        {
            if (id == 0)
            {
                return PartialView("_Upsert", new DepartmentBO());
            }
            else
            {
                var existingDepartment = _departmentService.GetDepartmentById(id);
                if (existingDepartment == null)
                {
                    return Json(new { success = "false", message = "Department not found! Please refresh the page." }, JsonRequestBehavior.AllowGet);
                }
                return PartialView("_Upsert", existingDepartment);
            }
        }

        [HttpPost]
        public ActionResult Upsert(DepartmentBO department)
        {
            var result = _departmentService.UpsertDepartment(department);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = _departmentService.DeleteDepartment(id);
            return Json(new { success = result.Success, message = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}
