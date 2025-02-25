using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVCStudentManagementCRUD.Models.Departments;

namespace MVCStudentManagementCRUD.Services.Departments
{
    public interface IDepartmentService
    {
        IEnumerable<SelectListItem> GetDepartmentOptions();
        IEnumerable<DepartmentBO> GetAllDepartments();
        DepartmentBO GetDepartmentById(int id);
        ActionsResult UpsertDepartment(DepartmentBO departmentBO);
        ActionsResult DeleteDepartment(int id);
    }
}
