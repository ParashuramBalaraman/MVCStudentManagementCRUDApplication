using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.ViewModels.Teachers
{
    public class TeacherDetailsViewModel
    {
        public string Name { get; set; }
        public List<string> StudentNames { get; set; }
    }
}