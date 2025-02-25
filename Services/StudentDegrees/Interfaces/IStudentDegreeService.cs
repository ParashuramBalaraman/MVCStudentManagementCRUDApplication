using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Models.StudentDegrees;
using MVCStudentManagementCRUD.Models.Students;

namespace MVCStudentManagementCRUD.Services.StudentDegrees
{
    public interface IStudentDegreeService
    {
        IEnumerable<DegreeBO> GetDegreesByStudent(int studentId);
        IEnumerable<StudentBO> GetStudentsByDegree(int degreeId);
        void CreateStudentDegree(int studentId, int degreeId);
        void DeleteStudentDegreesByStudent(int studentId);
    }
}
