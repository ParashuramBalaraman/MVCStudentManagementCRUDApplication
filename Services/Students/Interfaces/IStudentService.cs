using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using MVCStudentManagementCRUD.Models.Students;
using MVCStudentManagementCRUD.Models.Teachers;

namespace MVCStudentManagementCRUD.Services
{
    // Polymorphism as it has methods that are able to be implemented differently by classes that inherit from it.
    // Abstraction as it hides the implementation details from the user of the interface.
    // Open/Closed Principle as it is open for extension and closed for modification.
    public interface IStudentService
    {
        IEnumerable<StudentBO> GetAllStudents();
        StudentBO GetStudentById(int id);
        ActionsResult UpsertStudent(StudentBO studentBO);
        ActionsResult DeleteStudent(int id);
        IEnumerable<TeacherBO> GetTeachersByStudent(int id);
    }
}
