using MVCStudentManagementCRUD.Models.Genders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.ViewModels.Genders
{
    public class GenderViewModel
    {
        public IEnumerable<GenderBO> Genders { get; set; }
    }
}