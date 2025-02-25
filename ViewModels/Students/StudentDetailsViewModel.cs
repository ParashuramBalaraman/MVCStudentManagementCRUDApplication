using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.ViewModels.Students
{
    public class StudentDetailsViewModel
    {
        public string Name { get; set; }
        public List<string> TeacherNames { get; set; }
    }
}