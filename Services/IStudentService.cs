using MVCStudentManagementCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services
{
    public interface IStudentService
    {
        IEnumerable<SelectListItem> GetGenderOptions();
        //Change to type Degree when getting from the database.
        IEnumerable<SelectListItem> GetDegreeOptions();
        IEnumerable<SelectListItem> GetEthnicityOptions();
        IEnumerable<Student> GetAllStudents();
        Student GetStudentById(int id);
        void CreateOrUpdateStudent(Student student);
        void DeleteStudent(int id);
    }
}
