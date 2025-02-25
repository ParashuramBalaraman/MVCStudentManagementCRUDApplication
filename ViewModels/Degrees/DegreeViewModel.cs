using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models.Degrees;

namespace MVCStudentManagementCRUD.ViewModels.Degrees
{
    public class DegreeViewModel
    {
        public IEnumerable<DegreeBO> Degrees { get; set; }
    }
}