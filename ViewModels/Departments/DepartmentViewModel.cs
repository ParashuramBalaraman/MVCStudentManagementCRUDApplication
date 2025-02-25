using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models.Departments;

namespace MVCStudentManagementCRUD.ViewModels.Departments
{
    public class DepartmentViewModel
    {
        public IEnumerable<DepartmentBO> Departments { get; set; }
    }
}