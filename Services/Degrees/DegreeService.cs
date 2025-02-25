/// <summary>
/// Service layer for degree table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
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
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.DAL;
using MVCStudentManagementCRUD.Models.Teachers;
using System.Web.Mvc;
using MVCStudentManagementCRUD.Services.String;
using MVCStudentManagementCRUD.Services.StudentDegrees;

namespace MVCStudentManagementCRUD.Services.Degrees
{
    public class DegreeService : IDegreeService
    {
        // DBmodel to be used for each method
        private DBModel db;

        // Dependency Injection
        private readonly IStudentDegreeService _studentDegreeService;
        public DegreeService(IStudentDegreeService studentDegreeService)
        {
            db = new DBModel();
            _studentDegreeService = studentDegreeService;
        }

        /// <summary>
        /// Retrieves teachers by degree ID.
        /// </summary>
        /// <param name="degreeId">The ID of the degree.</param>
        /// <returns>List of TeacherBO representing teachers in the degree.</returns>
        public IEnumerable<TeacherBO> GetTeachersByDegree(int degreeId)
        {
            var teachers = db.Teachers.Where(x => x.DegreeId == degreeId).ToList();
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
        /// Retrieves degree options for dropdowns.
        /// </summary>
        /// <returns>List of SelectListItem representing degree options.</returns>
        public IEnumerable<SelectListItem> GetDegreeOptions()
        {
            // Retrieves the list of degrees then turns them into a list of SelectListItem with name as the text and value field.
            return db.Degrees.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Retrieves all degrees.
        /// </summary>
        /// <returns>List of DegreeBO representing all degrees.</returns>
        public IEnumerable<DegreeBO> GetAllDegrees()
        {
            var degrees = db.Degrees.ToList();
            List<DegreeBO> degreeBOs = new List<DegreeBO>();
            foreach (var degree in degrees)
            {
                degreeBOs.Add(new DegreeBO
                {
                    Id = degree.Id,
                    Name = degree.Name,
                    DepartmentId = degree.DepartmentId
                });
            }
            return degreeBOs;
        }

        /// <summary>
        /// Retrieves a degree by its ID.
        /// </summary>
        /// <param name="id">The ID of the degree.</param>
        /// <returns>DegreeBO representing the degree.</returns>
        public DegreeBO GetDegreeById(int id)
        {
            var degree = db.Degrees.Where(x => x.Id == id).FirstOrDefault();
            return new DegreeBO
            {
                Id = degree.Id,
                Name = degree.Name,
                DepartmentId = degree.DepartmentId
            };
        }

        /// <summary>
        /// Adds or updates a degree.
        /// </summary>
        /// <param name="degreeBO">The degree business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult UpsertDegree(DegreeBO degreeBO)
        {
            // Validate the degree is unique
            var result = validateDegree(degreeBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            degreeBO.Name = stringService.CapitalizeString(degreeBO.Name);

            //If degree is new create a new one else update the existing one
            if (degreeBO.Id == 0)
            {
                var degree = new Degree
                {
                    Name = degreeBO.Name,
                    DepartmentId = degreeBO.DepartmentId
                };
                db.Degrees.Add(degree);
            }
            else
            {
                var degree = db.Degrees.Where(x => x.Id == degreeBO.Id).FirstOrDefault();
                degree.Name = degreeBO.Name;
                degree.DepartmentId = degreeBO.DepartmentId;
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a degree by its ID.
        /// </summary>
        /// <param name="id">The ID of the degree.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult DeleteDegree(int id)
        {
            // Check if degree exists.
            var degree = db.Degrees.Where(x => x.Id == id).FirstOrDefault();
            if (degree == null)
            {
                return new ActionsResult { Success = false, Message = "Degree not found." };
            }
            try
            {
                db.Degrees.Remove(degree);
                db.SaveChanges();
                return new ActionsResult { Success = true, Message = "Degree deleted successfully!" };
            }
            catch (Exception ex)
            {
                return new ActionsResult { Success = false, Message = "This degree is being used and cannot be deleted." };
            }
        }

        /// <summary>
        /// Validates a degree.
        /// </summary>
        /// <param name="degreeBO">The degree business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult validateDegree(DegreeBO degreeBO)
        {
            //Check if degree with that name already exists.
            var existingDegree = db.Degrees.Where(x => x.Name == degreeBO.Name && x.Id != degreeBO.Id).FirstOrDefault();
            if (existingDegree != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "Degree with this name already exists."
                };
            }
            return new ActionsResult { Success = true, Message = "Degree saved successfully!" };
        }

        /// <summary>
        /// Retrieves the number of teachers by degree ID.
        /// </summary>
        /// <param name="degreeId">The ID of the degree.</param>
        /// <returns>The number of teachers in the degree.</returns>
        public int GetTeacherCountByDegree(int degreeId)
        {
            var teachers = db.Teachers.Where(x => x.DegreeId == degreeId).ToList();
            return teachers.Count;
        }

        /// <summary>
        /// Retrieves the number of students by degree ID.
        /// </summary>
        /// <param name="degreeId">The ID of the degree.</param>
        /// <returns>The number of students in the degree.</returns>
        public int GetStudentCountByDegree(int degreeId)
        {
            var students = _studentDegreeService.GetStudentsByDegree(degreeId); 
            return students.Count();
        }
    }
}