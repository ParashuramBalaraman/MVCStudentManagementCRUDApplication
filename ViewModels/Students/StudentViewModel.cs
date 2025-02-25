using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models.Students;

namespace MVCStudentManagementCRUD.ViewModels
{
    public class StudentViewModel
    {
        public IEnumerable<StudentBO> Students { get; set; }


    }
}