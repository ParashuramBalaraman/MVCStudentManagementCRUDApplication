using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCStudentManagementCRUD.Models.Genders
{
    public class GenderBO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Name cannot be longer than 20 characters.")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Number of Students")]
        public int StudentCount { get; set; }

        [Required]
        [Display(Name = "Number of Teachers")]
        public int TeacherCount { get; set; }
    }
}