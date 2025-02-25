using MVCStudentManagementCRUD.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.Models.Teachers
{
    public class TeacherBO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(30, ErrorMessage = "First Name cannot be longer than 30 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(30, ErrorMessage = "Last Name cannot be longer than 30 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Middle Name cannot be longer than 30 characters.")]
        public string MiddleName { get; set; }

        [Display(Name = "Preferred Name")]
        [StringLength(30, ErrorMessage = "Preferred Name cannot be longer than 30 characters.")]
        public string PreferredName { get; set; }

        [Required]
        [Display(Name = "Degree")]
        public int DegreeId { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^\s*\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})\s*$", ErrorMessage = "Entered phone format is not valid.")]
        [StringLength(20, ErrorMessage = "Phone Number cannot be longer than 20 characters.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "Email cannot be longer than 50 characters.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Address cannot be longer than 50 characters.")]
        //[DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        [Required]
        [Display(Name = "Ethnicity")]
        public int EthnicityId { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1970-01-01", "2009-01-01", ErrorMessage = "Date of Birth must be between {1} and {2}.")]
        public DateTime DateOfBirth { get; set; }
    }
}