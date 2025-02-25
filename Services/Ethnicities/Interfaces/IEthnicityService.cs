using MVCStudentManagementCRUD.Models.Ethnicities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services.Ethnicities
{
    public interface IEthnicityService
    {
        IEnumerable<SelectListItem> GetEthnicityOptions();
        IEnumerable<EthnicityBO> GetAllEthnicities();
        EthnicityBO GetEthnicityById(int id);
        ActionsResult UpsertEthnicity(EthnicityBO ethnicityBO);
        ActionsResult DeleteEthnicity(int id);
    }
}
