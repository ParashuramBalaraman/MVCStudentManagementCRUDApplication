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

    public partial class Degree
    {
        [Key]
        public int ID { get; set; }

        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters")]
        public string Name { get; set; }

        [StringLength(20, ErrorMessage = "Cannot be longer than 20 characters")]
        public string Department { get; set; }

        [StringLength(30, ErrorMessage = "Cannot be longer than 30 characters")]
        public string Coordinator { get; set; }
    }
}
