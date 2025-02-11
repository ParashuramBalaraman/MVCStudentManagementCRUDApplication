//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCStudentManagementCRUD.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Student
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "This field is required")]
        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string LastName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string MiddleName { get; set; }

        [Display(Name = "Preferred Name")]
        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string PreferredName { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public System.DateTime DateOfBirth { get; set; }

        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [StringLength(50, ErrorMessage = "Cannot be longer than 50 characters")]
        public string Address { get; set; }

        public string Gender { get; set; }

        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters")]
        public string Ethnicity { get; set; }
    }
}
