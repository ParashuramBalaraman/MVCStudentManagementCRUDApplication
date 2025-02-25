/// <summary>
/// Service layer for teacher table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-10</created>
/// <modified>
///     <date>2025-02-25</date>
///     <description>Ensure names are capitalised</description>
/// </modified>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.DAL;
using MVCStudentManagementCRUD.Models.Teachers;
using MVCStudentManagementCRUD.Models.Students;
using System.Runtime.InteropServices;
using MVCStudentManagementCRUD.Services.StudentDegrees;
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Services.String;

namespace MVCStudentManagementCRUD.Services.Teachers
{
    public class TeacherService : ITeacherService
    {
        // Dependency Injection
        private readonly IStudentDegreeService _studentDegreeService;
        // DBmodel to be used for each method
        private readonly DBModel db;

        public TeacherService(IStudentDegreeService studentDegreeService)
        {
            _studentDegreeService = studentDegreeService;
            db = new DBModel();
        }

        /// <summary>
        /// Retrieves all teachers.
        /// </summary>
        /// <returns>List of TeacherBO representing all teachers.</returns>
        public IEnumerable<TeacherBO> GetAllTeachers()
        {
            var teachers = db.Teachers.ToList();
            List<TeacherBO> teacherBOs = new List<TeacherBO>();
            foreach (var teacher in teachers)
            {
                teacherBOs.Add(new TeacherBO
                {
                    Id = teacher.Id,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    MiddleName = teacher.MiddleName,
                    PreferredName = teacher.PreferredName,
                    DegreeId = teacher.DegreeId,
                    Email = teacher.Email,
                    Phone = teacher.Phone,
                    Address = teacher.Address,
                    GenderId = teacher.GenderId,
                    EthnicityId = teacher.EthnicityId,
                    DateOfBirth = teacher.DateOfBirth
                });
            }
            return teacherBOs;
        }

        /// <summary>
        /// Retrieves a teacher by its ID.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <returns>TeacherBO representing the teacher.</returns>
        public TeacherBO GetTeacherById(int id)
        {
            Teacher teacher = db.Teachers.Where(x => x.Id == id).FirstOrDefault<Teacher>();
            return (new TeacherBO
            {
                Id = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                PreferredName = teacher.PreferredName,
                DegreeId = teacher.DegreeId,
                Email = teacher.Email,
                Phone = teacher.Phone.Trim(),
                Address = teacher.Address,
                GenderId = teacher.GenderId,
                EthnicityId = teacher.EthnicityId,
                DateOfBirth = teacher.DateOfBirth
            });
        }

        /// <summary>
        /// Adds or updates a teacher.
        /// </summary>
        /// <param name="teacherBO">The teacher business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult UpsertTeacher(TeacherBO teacherBO)
        {
            // Validate the teacher is unique
            ActionsResult result = validateTeacher(teacherBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            teacherBO.FirstName = stringService.CapitalizeString(teacherBO.FirstName);
            teacherBO.LastName = stringService.CapitalizeString(teacherBO.LastName);
            teacherBO.MiddleName = stringService.CapitalizeString(teacherBO.MiddleName);
            teacherBO.PreferredName = stringService.CapitalizeString(teacherBO.PreferredName);
            teacherBO.Phone = teacherBO.Phone.Trim();

            //If teacher is new create a new one else update the existing one
            if (teacherBO.Id == 0)
            {
                Teacher teacher = new Teacher
                {
                    Id = teacherBO.Id,
                    FirstName = teacherBO.FirstName,
                    LastName = teacherBO.LastName,
                    MiddleName = teacherBO.MiddleName,
                    PreferredName = teacherBO.PreferredName,
                    DegreeId = teacherBO.DegreeId,
                    DateOfBirth = teacherBO.DateOfBirth,
                    Email = teacherBO.Email,
                    Phone = teacherBO.Phone,
                    Address = teacherBO.Address,
                    GenderId = teacherBO.GenderId,
                    EthnicityId = teacherBO.EthnicityId
                };
                db.Teachers.Add(teacher);
            }
            else
            {
                Teacher teacher = db.Teachers.Where(x => x.Id == teacherBO.Id).FirstOrDefault<Teacher>();
                if (teacher != null)
                {
                    teacher.FirstName = teacherBO.FirstName;
                    teacher.LastName = teacherBO.LastName;
                    teacher.MiddleName = teacherBO.MiddleName;
                    teacher.PreferredName = teacherBO.PreferredName;
                    teacher.DegreeId = teacherBO.DegreeId;
                    teacher.DateOfBirth = teacherBO.DateOfBirth;
                    teacher.Email = teacherBO.Email;
                    teacher.Phone = teacherBO.Phone;
                    teacher.Address = teacherBO.Address;
                    teacher.GenderId = teacherBO.GenderId;
                    teacher.EthnicityId = teacherBO.EthnicityId;

                    db.Entry(teacher).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a teacher by its ID.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult DeleteTeacher(int id)
        {
            // Check if teacher exists.
            var teacher = db.Teachers.Where(x => x.Id == id).FirstOrDefault<Teacher>();
            if (teacher == null)
            {
                return new ActionsResult { Success = false, Message = "Teacher not found! Please refresh the page." };
            }
            db.Teachers.Remove(teacher);
            db.SaveChanges();
            return new ActionsResult { Success = true, Message = "Teacher deleted successfully!" };
        }

        /// <summary>
        /// Validates a teacher.
        /// </summary>
        /// <param name="teacherBO">The teacher business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult validateTeacher(TeacherBO teacherBO)
        {
            //Check if teacher with the same name already exists
            var existingTeacher = db.Teachers
                .Where(x => x.FirstName == teacherBO.FirstName && x.LastName == teacherBO.LastName && x.Id != teacherBO.Id)
                .FirstOrDefault();
            if (existingTeacher != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "Teacher with the name " + teacherBO.FirstName + " " + teacherBO.LastName + " already exists."
                };
            }

            //Check if teacher with the same email already exists
            existingTeacher = db.Teachers
                .Where(x => x.Email == teacherBO.Email && x.Id != teacherBO.Id)
                .FirstOrDefault();
            if (existingTeacher != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "The email " + teacherBO.Email + " is already being used."
                };
            }

            //Check if teacher with the same phone number already exists
            existingTeacher = db.Teachers
                .Where(x => x.Phone == teacherBO.Phone && x.Id != teacherBO.Id)
                .FirstOrDefault();
            if (existingTeacher != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "The phone number " + teacherBO.Phone + " is already being used."
                };
            }

            return new ActionsResult { Success = true, Message = "Teacher saved successfully!" };
        }

        /// <summary>
        /// Retrieves students by teacher ID.
        /// </summary>
        /// <param name="id">The ID of the teacher.</param>
        /// <returns>List of StudentBO representing students taught by the teacher.</returns>
        public IEnumerable<StudentBO> GetStudentsByTeacher(int id)
        {
            int degreeId = GetTeacherById(id).DegreeId;
            return _studentDegreeService.GetStudentsByDegree(degreeId);
        }
    }
}