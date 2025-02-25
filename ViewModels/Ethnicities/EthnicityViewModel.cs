using MVCStudentManagementCRUD.Models.Ethnicities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.ViewModels.Ethnicities
{
    public class EthnicityViewModel
    {
        public IEnumerable<EthnicityBO> Ethnicities{ get; set; }
    }
}