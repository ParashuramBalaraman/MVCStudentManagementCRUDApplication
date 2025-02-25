/// <summary>
/// Service layer for student table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-10</created>
/// <modified>
///     <date>2025-02-25</date>
///     <description>Ensure names are capitalised</description>
/// </modified>

using MVCStudentManagementCRUD.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCStudentManagementCRUD.Models.Students;
using MVCStudentManagementCRUD.Models.Genders;
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Services.StudentDegrees;
using MVCStudentManagementCRUD.Models.Teachers;
using MVCStudentManagementCRUD.Services.Degrees;
using MVCStudentManagementCRUD.Services.String;

namespace MVCStudentManagementCRUD.Services.Students
{
    // Inheritance
    public class StudentService : IStudentService
    {
        // Dependency Injection
        private IStudentDegreeService _studentDegreeService;
        private IDegreeService _degreeService;

        // DBmodel to be used for each method
        private DBModel db;

        public StudentService(IStudentDegreeService studentDegreeService, IDegreeService degreeService) 
        {
            db = new DBModel();
            _studentDegreeService = studentDegreeService;
            _degreeService = degreeService;
        }

        /// <summary>
        /// Gets all students from the database.
        /// </summary>
        /// <param>None</param>
        /// <returns>IEnumerable of StudentBO</returns>
        public IEnumerable<StudentBO> GetAllStudents()
        {
            var students = db.Students.ToList();
            List<StudentBO> studentBOs = new List<StudentBO>();
            foreach (var student in students)
            {
                var degrees = _studentDegreeService.GetDegreesByStudent(student.Id);
                List<int> degreeIds = new List<int>();
                foreach (DegreeBO degree in degrees)
                {
                    degreeIds.Add(degree.Id);
                }
                studentBOs.Add(new StudentBO
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName,
                    PreferredName = student.PreferredName,
                    DateOfBirth = student.DateOfBirth,
                    Email = student.Email,
                    Phone = student.Phone,
                    Address = student.Address,
                    GenderId = student.GenderId,
                    EthnicityId = student.EthnicityId,
                    DegreeIds = degreeIds
                });
            }
            return studentBOs;
        }

        /// <summary>
        /// Gets a student from the database from its id.
        /// </summary>
        /// <param name="id">Student's ID</param>
        /// <returns>StudentBO</returns>
        public StudentBO GetStudentById(int id)
        {
            Student student =  db.Students.Where(x => x.Id == id).FirstOrDefault<Student>();
            var degrees = _studentDegreeService.GetDegreesByStudent(student.Id);
            List<int> degreeIds = new List<int>();
            foreach (DegreeBO degree in degrees)
            {
                degreeIds.Add(degree.Id);
            }
            return (new StudentBO
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                MiddleName = student.MiddleName,
                PreferredName = student.PreferredName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                Phone = student.Phone.Trim(),
                Address = student.Address,
                GenderId = student.GenderId,
                EthnicityId = student.EthnicityId,
                DegreeIds = degreeIds
            });
        }

        /// <summary>
        /// Upserts a student to the database.
        /// </summary>
        /// <param name="studentBO">StudentBO</param>
        /// <returns>ActionsResult</returns>
        public ActionsResult UpsertStudent(StudentBO studentBO)
        {
            //Check student is valid
            ActionsResult result = validateStudent(studentBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            studentBO.FirstName = stringService.CapitalizeString(studentBO.FirstName);
            studentBO.LastName = stringService.CapitalizeString(studentBO.LastName);
            studentBO.MiddleName = stringService.CapitalizeString(studentBO.MiddleName);
            studentBO.PreferredName = stringService.CapitalizeString(studentBO.PreferredName);

            //Gets rid of whitespaces before or after the phone number.
            studentBO.Phone = studentBO.Phone.Trim();

            //Takes the studentBO object and creates a student object from it.
            if (studentBO.Id == 0)
            {
                Student student = new Student
                {
                    FirstName = studentBO.FirstName,
                    LastName = studentBO.LastName,
                    MiddleName = studentBO.MiddleName,
                    PreferredName = studentBO.PreferredName,
                    DateOfBirth = studentBO.DateOfBirth,
                    Email = studentBO.Email,
                    Phone = studentBO.Phone,
                    Address = studentBO.Address,
                    GenderId = studentBO.GenderId,
                    EthnicityId = studentBO.EthnicityId
                };
                db.Students.Add(student);
                db.SaveChanges();

                //Creates student degrees for the student.
                foreach (int degreeId in studentBO.DegreeIds)
                {
                    _studentDegreeService.CreateStudentDegree(student.Id, degreeId);
                }
            }
            else
            {
                //Updates the current student object from the studentBO.
                Student student = db.Students.Where(x => x.Id == studentBO.Id).FirstOrDefault<Student>();
                if (student != null)
                {
                    student.FirstName = studentBO.FirstName;
                    student.LastName = studentBO.LastName;
                    student.MiddleName = studentBO.MiddleName;
                    student.PreferredName = studentBO.PreferredName;
                    student.DateOfBirth = studentBO.DateOfBirth;
                    student.Email = studentBO.Email;
                    student.Phone = studentBO.Phone;
                    student.Address = studentBO.Address;
                    student.GenderId = studentBO.GenderId;
                    student.EthnicityId = studentBO.EthnicityId;

                    db.Entry(student).State = System.Data.Entity.EntityState.Modified;

                    //Updates the student degrees for the student.
                    _studentDegreeService.DeleteStudentDegreesByStudent(student.Id);
                    foreach (int degreeId in studentBO.DegreeIds)
                    {
                        _studentDegreeService.CreateStudentDegree(student.Id, degreeId);
                    }

                }
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a student from the database.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>ActionsResult</returns>
        public ActionsResult DeleteStudent(int id)
        {
            var student = db.Students.Where(x => x.Id == id).FirstOrDefault<Student>();
            //Check if student exists.
            if (student == null)
            {
                return new ActionsResult { Success = false, Message = "Student not found! Please refresh the page." };
            }
            //Calling the studentDegreeService to delete all student degrees associated with the student.
            _studentDegreeService.DeleteStudentDegreesByStudent(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return new ActionsResult { Success = true, Message = "Student deleted successfully!" };
        }

        /// <summary>
        /// Validates the student is unique.
        /// </summary>
        /// <param name="studentBO">StudentBO</param>
        /// <returns>ActionsResult</returns>
        public ActionsResult validateStudent(StudentBO studentBO)
        {
            //Check if student with that name already exists.
            var existingStudent = db.Students
                .Where(x => x.FirstName == studentBO.FirstName && x.LastName == studentBO.LastName && x.Id != studentBO.Id)
                .FirstOrDefault();
            if (existingStudent != null)
            {

                return new ActionsResult
                {
                    Success = false,
                    Message = "Student with the name " + studentBO.FirstName + " " + studentBO.LastName + " already exists."
                };
            }

            //Check if student with that email already exists.
            existingStudent = db.Students
                .Where(x => x.Email == studentBO.Email && x.Id != studentBO.Id)
                .FirstOrDefault();
            if (existingStudent != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "The email " + studentBO.Email + " is already being used."
                };
            }

            //Check if student with that phone number already exists.
            existingStudent = db.Students
                .Where(x => x.Phone == studentBO.Phone && x.Id != studentBO.Id)
                .FirstOrDefault();
            if (existingStudent != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "The phone number " + studentBO.Phone + " is already being used."
                };
            }

            return new ActionsResult { Success = true, Message = "Student saved successfully!" };
        }

        /// <summary>
        /// Gets all teachers that teach student.
        /// </summary>
        /// <param name="id">Student ID</param>
        /// <returns>IEnumerable<TeacherBO>></returns>
        public IEnumerable<TeacherBO> GetTeachersByStudent(int id)
        {
            // Gets all degrees for the student.
            var degrees = _studentDegreeService.GetDegreesByStudent(id);
            List<TeacherBO> teacherBOs = new List<TeacherBO>();
            // Gets all teachers for each degree and adds them to the list.
            foreach (DegreeBO degree in degrees)
            {
                var teachers = _degreeService.GetTeachersByDegree(degree.Id);
                foreach (TeacherBO teacher in teachers)
                {
                    teacherBOs.Add(teacher);
                }
            }
            return teacherBOs;
        }
    }
}