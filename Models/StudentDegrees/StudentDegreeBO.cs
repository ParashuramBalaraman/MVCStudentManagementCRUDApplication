using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVCStudentManagementCRUD.Models.StudentDegrees
{
    public class StudentDegreeBO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int DegreeId { get; set; }
    }
}