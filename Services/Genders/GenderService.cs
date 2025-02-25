/// <summary>
/// Service layer for gender table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-10</created>
/// <modified>
///     <date>2025-02-25</date>
///     <description>Ensure names are capitalised</description>
/// </modified>

using MVCStudentManagementCRUD.DAL;
using MVCStudentManagementCRUD.Models.Genders;
using MVCStudentManagementCRUD.Models.Students;
using MVCStudentManagementCRUD.Models.Teachers;
using MVCStudentManagementCRUD.Services.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services.Genders
{
    public class GenderService : IGenderService
    {
        // DBmodel to be used for each method
        private DBModel db;

        public GenderService()
        {
            db = new DBModel();
        }

        /// <summary>
        /// Retrieves gender options for dropdowns.
        /// </summary>
        /// <returns>List of SelectListItem representing gender options.</returns>
        public IEnumerable<SelectListItem> GetGenderOptions()
        {
            return db.Genders.Select(g => new SelectListItem
            {
                Text = g.Name,
                Value = g.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Retrieves all genders.
        /// </summary>
        /// <returns>List of GenderBO representing all genders.</returns>
        public IEnumerable<GenderBO> GetAllGenders()
        {
            var genders = db.Genders.ToList();
            List<GenderBO> genderBOs = new List<GenderBO>();
            foreach (var gender in genders)
            {
                genderBOs.Add(new GenderBO
                {
                    Id = gender.Id,
                    Name = gender.Name,
                    StudentCount = GetStudentsByGender(gender.Id),
                    TeacherCount = GetTeachersByGender(gender.Id)
                });
            }
            return genderBOs;
        }

        /// <summary>
        /// Retrieves a gender by its ID.
        /// </summary>
        /// <param name="id">The ID of the gender.</param>
        /// <returns>GenderBO representing the gender.</returns>
        public GenderBO GetGenderById(int id)
        {
            var gender = db.Genders.Where(x => x.Id == id).FirstOrDefault();
            //Check if gender exists
            if (gender == null)
            {
                return null;
            }
            return new GenderBO
            {
                Id = gender.Id,
                Name = gender.Name
            };
        }

        /// <summary>
        /// Adds or updates a gender.
        /// </summary>
        /// <param name="genderBO">The gender business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult UpsertGender(GenderBO genderBO)
        {
            // Validate the gender is unique
            var result = validateGender(genderBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            genderBO.Name = stringService.CapitalizeString(genderBO.Name);
            //If gender is new create a new one else update the existing one
            if (genderBO.Id == 0)
            {
                var gender = new Gender
                {
                    Name = genderBO.Name
                };
                db.Genders.Add(gender);
            }
            else
            {
                var gender = db.Genders.Where(x => x.Id == genderBO.Id).FirstOrDefault();
                if (gender != null)
                {
                    gender.Name = genderBO.Name;
                }
                else
                {
                    return new ActionsResult { Success = false, Message = "Gender not found." };
                }
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes a gender by its ID.
        /// </summary>
        /// <param name="id">The ID of the gender.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult DeleteGender(int id)
        {
            // Check if gender exists.
            var gender = db.Genders.Where(x => x.Id == id).FirstOrDefault();
            if (gender == null)
            {
                return new ActionsResult { Success = false, Message = "Gender not found! Please refresh the page." };
            }
            try
            {
                // Check if gender is being used before deleting.
                if (gender.Students.Count > 0)
                {
                    return new ActionsResult
                    {
                        Success = false,
                        Message = "This gender is being used and cannot be deleted."
                    };
                }
                db.Genders.Remove(gender);
                db.SaveChanges();
                return new ActionsResult { Success = true, Message = "Gender deleted successfully!" };
            }
            catch (Exception ex)
            {
                return new ActionsResult { Success = false, Message = "This gender is being used and cannot be deleted." };
            }
        }

        /// <summary>
        /// Validates a gender.
        /// </summary>
        /// <param name="genderBO">The gender business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult validateGender(GenderBO genderBO)
        {
            //Check if there is a gender that exists with the same name and different ID
            var existingGender = db.Genders.Where(x => x.Name == genderBO.Name && x.Id != genderBO.Id).FirstOrDefault();
            if (existingGender != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "Gender with this name already exists."
                };
            }
            return new ActionsResult { Success = true, Message = "Gender saved successfully!" };
        }

        /// <summary>
        /// Retrieves the number of teachers by gender ID.
        /// </summary>
        /// <param name="genderId">The ID of the gender.</param>
        /// <returns>The number of teachers with the specified gender.</returns>
        public int GetTeachersByGender(int genderId)
        {
            var teachers = db.Teachers.Where(x => x.GenderId == genderId).ToList();
            return teachers.Count;
        }

        /// <summary>
        /// Retrieves the number of students by gender ID.
        /// </summary>
        /// <param name="genderId">The ID of the gender.</param>
        /// <returns>The number of students with the specified gender.</returns>
        public int GetStudentsByGender(int genderId)
        {
            var students = db.Students.Where(x => x.GenderId == genderId).ToList();
            return students.Count;
        }
    }
}