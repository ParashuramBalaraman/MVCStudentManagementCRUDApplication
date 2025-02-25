/// <summary>
/// Service layer for ethnicity table. This class is used to interact with the database as Controller is part of presentation layer and this is the service layer.
/// </summary>
/// <author>Parashuram Balaraman</author>
/// <created>2025-02-10</created>
/// <modified>
///     <date>2025-02-25</date>
///     <description>Ensure names are capitalised</description>
/// </modified>

using MVCStudentManagementCRUD.DAL;
using MVCStudentManagementCRUD.Models.Ethnicities;
using MVCStudentManagementCRUD.Models.Genders;
using MVCStudentManagementCRUD.Services.String;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCStudentManagementCRUD.Services.Ethnicities
{
    // Inheritance
    public class EthnicityService : IEthnicityService
    {
        private readonly DBModel db;

        public EthnicityService()
        {
            db = new DBModel();
        }

        /// <summary>
        /// Retrieves ethnicity options for dropdowns.
        /// </summary>
        /// <returns>List of SelectListItem representing ethnicity options.</returns>
        public IEnumerable<SelectListItem> GetEthnicityOptions()
        {
            // Retrieves the list of ethnicities then turns them into a list of SelectListItem with name as the text and value field.
            return db.Ethnicities.Select(e => new SelectListItem
            {
                Text = e.Name,
                Value = e.Id.ToString()
            }).ToList();
        }

        /// <summary>
        /// Retrieves all ethnicities.
        /// </summary>
        /// <returns>List of EthnicityBO representing all ethnicities.</returns>
        public IEnumerable<EthnicityBO> GetAllEthnicities()
        {
            var ethnicities = db.Ethnicities.ToList();
            List<EthnicityBO> ethnicityBOs = new List<EthnicityBO>();
            foreach (var ethnicity in ethnicities)
            {
                ethnicityBOs.Add(new EthnicityBO
                {
                    Id = ethnicity.Id,
                    Name = ethnicity.Name,
                    StudentCount = GetStudentsByEthnicity(ethnicity.Id),
                    TeacherCount = GetTeachersByEthnicity(ethnicity.Id),
                });
            }
            return ethnicityBOs;
        }

        /// <summary>
        /// Retrieves an ethnicity by its ID.
        /// </summary>
        /// <param name="id">The ID of the ethnicity.</param>
        /// <returns>EthnicityBO representing the ethnicity.</returns>
        public EthnicityBO GetEthnicityById(int id)
        {
            var ethnicity = db.Ethnicities.Where(x => x.Id == id).FirstOrDefault();
            return new EthnicityBO
            {
                Id = id,
                Name = ethnicity.Name
            };
        }

        /// <summary>
        /// Adds or updates an ethnicity.
        /// </summary>
        /// <param name="ethnicityBO">The ethnicity business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult UpsertEthnicity(EthnicityBO ethnicityBO)
        {
            // Validate the ethnicity is unique.
            var result = validateEthnicity(ethnicityBO);
            if (!result.Success)
            {
                return result;
            }

            // Create an instance of StringService to call the CapitalizeString method
            StringService stringService = new StringService();

            // Capitalize the first letter of each relevant property
            ethnicityBO.Name = stringService.CapitalizeString(ethnicityBO.Name);

            // If it doesn't exist create a new ethnicity. Otherwise, update the existing one.
            if (ethnicityBO.Id == 0)
            {
                var ethnicity = new Ethnicity
                {
                    Name = ethnicityBO.Name
                };
                db.Ethnicities.Add(ethnicity);
            }
            else
            {
                var ethnicity = db.Ethnicities.Where(x => x.Id == ethnicityBO.Id).FirstOrDefault();
                ethnicity.Name = ethnicityBO.Name;
            }
            db.SaveChanges();
            return result;
        }

        /// <summary>
        /// Deletes an ethnicity from the database.
        /// </summary>
        /// <param name="id">The ID of the ethnicity.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        public ActionsResult DeleteEthnicity(int id)
        {
            // Check if ethnicity exists.
            var ethnicity = db.Ethnicities.Where(x => x.Id == id).FirstOrDefault();
            if (ethnicity == null)
            {
                return new ActionsResult { Success = false, Message = "Ethnicity not found." };
            }
            try
            {
                db.Ethnicities.Remove(ethnicity);
                db.SaveChanges();
                return new ActionsResult { Success = true, Message = "Ethnicity deleted successfully!" };
            }
            catch (Exception ex)
            {
                return new ActionsResult { Success = false, Message = "This ethnicity is being used and cannot be deleted." };
            }
        }

        /// <summary>
        /// Validates the ethnicity to ensure it is unique.
        /// </summary>
        /// <param name="ethnicityBO">The ethnicity business object.</param>
        /// <returns>ActionsResult indicating success or failure.</returns>
        private ActionsResult validateEthnicity(EthnicityBO ethnicityBO)
        {
            //Check if ethnicity with this name already exists.
            var existingEthnicity = db.Ethnicities.Where(x => x.Name == ethnicityBO.Name && x.Id != ethnicityBO.Id).FirstOrDefault();
            if (existingEthnicity != null)
            {
                return new ActionsResult
                {
                    Success = false,
                    Message = "Ethnicity with this name already exists."
                };
            }
            return new ActionsResult { Success = true, Message = "Ethnicity saved successfully!" };
        }

        /// <summary>
        /// Retrieves the number of teachers by ethnicity ID.
        /// </summary>
        /// <param name="ethnicityId">The ID of the ethnicity.</param>
        /// <returns>The number of teachers with the specified ethnicity.</returns>
        public int GetTeachersByEthnicity(int ethnicityId)
        {
            var teachers = db.Teachers.Where(x => x.EthnicityId == ethnicityId).ToList();
            return teachers.Count;
        }

        /// <summary>
        /// Retrieves the number of students by ethnicity ID.
        /// </summary>
        /// <param name="ethnicityId">The ID of the ethnicity.</param>
        /// <returns>The number of students with the specified ethnicity.</returns>
        public int GetStudentsByEthnicity(int ethnicityId)
        {
            var students = db.Students.Where(x => x.EthnicityId == ethnicityId).ToList();
            return students.Count;
        }
    }
}