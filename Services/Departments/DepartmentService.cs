/// <summary>
/// Service layer for department table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
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
using MVCStudentManagementCRUD.Models.Departments;
using MVCStudentManagementCRUD.DAL;
using System.Web.Mvc;
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Services.Degrees;
using MVCStudentManagementCRUD.Models.Genders;
using MVCStudentManagementCRUD.Services.String;

namespace MVCStudentManagementCRUD.Services.Departments
{
    // Implement other business logic methods related to departments. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
    // Inheritance
    public class DepartmentService : IDepartmentService
    {
        private DBModel db;

        // Dependency Injection
        private readonly IDegreeService _degreeService;
        public DepartmentService(IDegreeService degreeService)
        {
            db = new DBModel();
            _degreeService = degreeService;
        }

        /// <summary>
        /// Retrieves department options for dropdowns.
        /// </summary>
        /// <returns>List of SelectListItem representing department options.</returns>
        public IEnumerable<SelectListItem> GetDepartmentOptions()
        {
            return db.Departments.Select(d => new SelectListItem
            {
                Text = d.Name,
                Value = d.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Retrieves all departments.
        /// </summary>
        /// <returns>List of DepartmentBO representing all departments.</returns>
        public IEnumerable<DepartmentBO> GetAllDepartments()
        {
            var departments = db.Departments.ToList();
            List<DepartmentBO> departmentBOs = new List<DepartmentBO>();
            foreach (var department in departments)
            {
                departmentBOs.Add(new DepartmentBO
                {
                    Id = department.Id,
                    Name = department.Name,
                    TeacherCount = GetTeachersByDepartment(department.Id),
                    StudentCount = GetStudentsByDepartment(department.Id)
                });
            }
            return departmentBOs;
        }

        /// <summary>
        /// Retrieves a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department.</param>
        /// <returns>DepartmentBO representing the department.</returns>
        public DepartmentBO GetDepartmentById(int id)
        {
            var department = db.Departments.Where(x => x.Id == id).FirstOrDefault();
            return new DepartmentBO
            {
                Id = department.Id,
                Name = department.Name
            };
        }

        /// <summary>
        /// Adds or updates a department.
        /// </summary>
        /// <param name="departmentBO">The department business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult UpsertDepartment(DepartmentBO departmentBO)
        {
            // Validate the department is unique.
            var result = validateDepartment(departmentBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            departmentBO.Name = stringService.CapitalizeString(departmentBO.Name);

            //If it doesn't exist, create a new department. Otherwise, update the existing department.
            if (departmentBO.Id == 0)
            {
                var department = new Department
                {
                    Name = departmentBO.Name
                };
                db.Departments.Add(department);
            }
            else
            {
                var department = db.Departments.Where(x => x.Id == departmentBO.Id).FirstOrDefault();
                department.Name = departmentBO.Name;
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a department by its ID.
        /// </summary>
        /// <param name="id">The ID of the department.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult DeleteDepartment(int id)
        {
            // Check if department exists.
            var department = db.Departments.Where(x => x.Id == id).FirstOrDefault();
            if (department == null)
            {
                return new ActionsResult { Success = false, Message = "Department not found." };
            }
            try
            {
                db.Departments.Remove(department);
                db.SaveChanges();
                return new ActionsResult { Success = true, Message = "Department deleted successfully!" };
            }
            catch (Exception ex)
            {
                return new ActionsResult { Success = false, Message = "This department is being used and cannot be deleted." };
            }
        }

        /// <summary>
        /// Validates a department.
        /// </summary>
        /// <param name="departmentBO">The department business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult validateDepartment(DepartmentBO departmentBO)
        {
            // Check if department with this name already exists.
            var existingDepartment = db.Departments.Where(x => x.Name == departmentBO.Name && x.Id != departmentBO.Id).FirstOrDefault();
            if (existingDepartment != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "Department with this name already exists."
                };
            }
            return new ActionsResult { Success = true, Message = "Department saved successfully!" };
        }

        /// <summary>
        /// Retrieves degrees by department ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>List of DegreeBO representing degrees in the department.</returns>
        public IEnumerable<DegreeBO> GetDegreesByDepartment(int departmentId)
        {
            // Get degrees by department ID.
            var degrees = db.Degrees.Where(x => x.DepartmentId == departmentId).ToList();
            List<DegreeBO> degreeBOs = new List<DegreeBO>();
            // Goes through each degree and adds it to the list.
            foreach (var degree in degrees)
            {
                degreeBOs.Add(new DegreeBO
                {
                    Id = degree.Id,
                    Name = degree.Name
                });
            }
            return degreeBOs;
        }

        /// <summary>
        /// Retrieves the number of teachers by department ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>The number of teachers in the department.</returns>
        public int GetTeachersByDepartment(int departmentId)
        {
            // Get degrees by department ID.
            var degrees = GetDegreesByDepartment(departmentId);
            int count = 0;
            // Goes through each degree and adds the number of teachers to the count.
            foreach (var degree in degrees)
            {
                int teachersCount = _degreeService.GetTeacherCountByDegree(degree.Id);
                count += teachersCount;
            }
            return count;
        }

        /// <summary>
        /// Retrieves the number of students by department ID.
        /// </summary>
        /// <param name="departmentId">The ID of the department.</param>
        /// <returns>The number of students in the department.</returns>
        public int GetStudentsByDepartment(int departmentId)
        {
            // Get degrees by department ID.
            var degrees = GetDegreesByDepartment(departmentId);
            int count = 0;
            // Goes through each degree and adds the number of students to the count.
            foreach (var degree in degrees)
            {
                int studentsCount = _degreeService.GetStudentCountByDegree(degree.Id);
                count += studentsCount;
            }
            return count;
        }
    }
}