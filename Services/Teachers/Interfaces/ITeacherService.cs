using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCStudentManagementCRUD.Models.Teachers;
using MVCStudentManagementCRUD.Models.Students;

namespace MVCStudentManagementCRUD.Services.Teachers 
{ 
    public interface ITeacherService
    {
        IEnumerable<TeacherBO> GetAllTeachers();
        TeacherBO GetTeacherById(int id);
        ActionsResult UpsertTeacher(TeacherBO teacherBO);
        ActionsResult DeleteTeacher(int id);
        IEnumerable<StudentBO> GetStudentsByTeacher(int id);
    }
}
