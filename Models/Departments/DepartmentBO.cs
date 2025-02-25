using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.Models.Departments
{
    public class DepartmentBO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Department Name cannot be longer than 30 characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Number of Students")]
        public int StudentCount { get; set; }

        [Required]
        [Display(Name = "Number of Teachers")]
        public int TeacherCount { get; set; }
    }
}