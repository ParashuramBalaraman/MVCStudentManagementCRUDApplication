using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.ViewModels.Degrees
{
    public class DegreeDetailsViewModel
    {
        public string Name { get; set; }
        public List<string> StudentNames { get; set; }
        public List<string> TeacherNames { get; set; }
    }
}