using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models;

namespace MVCStudentManagementCRUD.ViewModels
{
    public class StudentViewModel
    {
        public IEnumerable<Student> Students { get; set; }

        public IEnumerable<Degree> Degree { get; set; }


    }
}