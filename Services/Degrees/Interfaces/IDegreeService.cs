using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services.Degrees
{
    public interface IDegreeService
    {
        IEnumerable<TeacherBO> GetTeachersByDegree(int degreeId);
        IEnumerable<SelectListItem> GetDegreeOptions();
        IEnumerable<DegreeBO> GetAllDegrees();
        DegreeBO GetDegreeById(int id);
        ActionsResult UpsertDegree(DegreeBO degreeBO);
        ActionsResult DeleteDegree(int id);
        int GetStudentCountByDegree(int degreeId);
        int GetTeacherCountByDegree(int degreeId);
    }
}