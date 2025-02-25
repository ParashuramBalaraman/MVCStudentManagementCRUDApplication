/// <summary>
/// Service layer for studentdegree table. This class is used to interact with the database as the controllers that use it are a part of presentation layer and this is the service layer.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-10</created>
/// <modified>
/// </modified>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCStudentManagementCRUD.Models.Degrees;
using MVCStudentManagementCRUD.Models.Students;
using MVCStudentManagementCRUD.Models.StudentDegrees;
using MVCStudentManagementCRUD.DAL;

namespace MVCStudentManagementCRUD.Services.StudentDegrees
{
    public class StudentDegreeService : IStudentDegreeService
    {
        // DBmodel to be used for each method
        private DBModel db;

        public StudentDegreeService()
        {
            db = new DBModel();
        }

        /// <summary>
        /// Retrieves degrees by student ID.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <returns>List of DegreeBO representing degrees associated with the student.</returns>
        public IEnumerable<DegreeBO> GetDegreesByStudent(int studentId)
        {
            // Gets Student Degrees by Student
            IEnumerable<StudentDegree> studentDegrees = db.StudentDegrees.Where(x => x.StudentId == studentId).ToList();
            List<DegreeBO> degreeBOs = new List<DegreeBO>();
            // Parse through each student degree and get the degree
            foreach (StudentDegree studentDegree in studentDegrees)
            {
                Degree degree = db.Degrees.Where(x => x.Id == studentDegree.DegreeId).FirstOrDefault();
                // Add the degree to the list
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
        /// Retrieves students by degree ID.
        /// </summary>
        /// <param name="degreeId">The ID of the degree.</param>
        /// <returns>List of StudentBO representing students associated with the degree.</returns>
        public IEnumerable<StudentBO> GetStudentsByDegree(int degreeId)
        {
            // Gets Student Degrees by Degree
            IEnumerable<StudentDegree> studentDegrees = db.StudentDegrees.Where(x => x.DegreeId == degreeId).ToList();
            List<StudentBO> studentBOs = new List<StudentBO>();
            // Parse through each student degree and get the student
            foreach (StudentDegree studentDegree in studentDegrees)
            {
                Student student = db.Students.Where(x => x.Id == studentDegree.StudentId).FirstOrDefault();
                // Add the student to the list
                studentBOs.Add(new StudentBO
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    MiddleName = student.MiddleName,
                    PreferredName = student.PreferredName,
                    Email = student.Email,
                    Phone = student.Phone,
                    Address = student.Address,
                    GenderId = student.GenderId,
                    EthnicityId = student.EthnicityId,
                    DateOfBirth = student.DateOfBirth
                });
            }
            return studentBOs;
        }

        /// <summary>
        /// Creates a student-degree association.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        /// <param name="degreeId">The ID of the degree.</param>
        public void CreateStudentDegree(int studentId, int degreeId)
        {
            var existingStudentDegree = db.StudentDegrees.Where(x => x.StudentId == studentId && x.DegreeId == degreeId).FirstOrDefault();
            // If the student-degree association already exists, do nothing
            if (existingStudentDegree != null)
            {
                return;
            }
            // Otherwise, create the student-degree association
            db.StudentDegrees.Add(new StudentDegree
            {
                StudentId = studentId,
                DegreeId = degreeId
            });
            db.SaveChanges();
        }

        /// <summary>
        /// Deletes all student-degree associations for a given student.
        /// </summary>
        /// <param name="studentId">The ID of the student.</param>
        public void DeleteStudentDegreesByStudent(int studentId)
        {
            // Get all student degrees for the student
            IEnumerable<StudentDegree> studentDegrees = db.StudentDegrees.Where(x => x.StudentId == studentId).ToList();
            foreach (StudentDegree studentDegree in studentDegrees)
            {
                // Remove each student degree
                db.StudentDegrees.Remove(studentDegree);
            }
            db.SaveChanges();
        }
    }
}