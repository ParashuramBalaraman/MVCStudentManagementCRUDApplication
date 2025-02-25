//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVCStudentManagementCRUD.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Teacher
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string PreferredName { get; set; }
        public int DegreeId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int GenderId { get; set; }
        public int EthnicityId { get; set; }
        public System.DateTime DateOfBirth { get; set; }
    
        public virtual Degree Degree { get; set; }
        public virtual Ethnicity Ethnicity { get; set; }
        public virtual Gender Gender { get; set; }
    }
}
