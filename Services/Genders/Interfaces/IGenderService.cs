using MVCStudentManagementCRUD.Models.Genders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services.Genders
{
    public interface IGenderService
    {
        IEnumerable<SelectListItem> GetGenderOptions();
        IEnumerable<GenderBO> GetAllGenders();
        GenderBO GetGenderById(int id);
        ActionsResult UpsertGender(GenderBO genderBO);
        ActionsResult DeleteGender(int id);
    }
}
