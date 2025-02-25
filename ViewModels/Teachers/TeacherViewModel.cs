using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models.Teachers;

namespace MVCStudentManagementCRUD.ViewModels.Teachers
{
    public class TeacherViewModel
    {
        public IEnumerable<TeacherBO> Teachers { get; set; }
    }
}