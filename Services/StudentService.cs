using MVCStudentManagementCRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MVCStudentManagementCRUD.Services
{
    public class StudentService : IStudentService
    {
        public IEnumerable<SelectListItem> GetGenderOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "Male", Value = "M" },
                new SelectListItem { Text = "Female", Value = "F" },
                new SelectListItem { Text = "Prefer Not To Say", Value = "P" }
            };
        }

        public IEnumerable<SelectListItem> GetDegreeOptions()
        {
            using (DBModel db = new DBModel())
            {
                //Retrieves the list of degrees then turns them into a list of SelectListItem with name as the text and value field.
                return db.Degrees.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name 
                }).ToList();
            }
        }

        public IEnumerable<SelectListItem> GetEthnicityOptions()
        {
            using (DBModel db = new DBModel())
            {
                //Retrieves the list of ethnicities then turns them into a list of SelectListItem with name as the text and value field.
                return db.Ethnicities.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Name 
                }).ToList();
            }
        }

        //Better to use a new instance of DBModel for each method, to avoif conflicts.
        public IEnumerable<Student> GetAllStudents()
        {
            using (DBModel db = new DBModel())
            {
                return db.Students.ToList();
            }
        }

        public Student GetStudentById(int id)
        {
            using (DBModel db = new DBModel())
            {
                return db.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
            }
        }

        public void CreateOrUpdateStudent(Student student)
        {
            using (DBModel db = new DBModel())
            {
                if (student.ID == 0)
                {
                    db.Students.Add(student);
                }
                else
                {
                    db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            using (DBModel db = new DBModel())
            {
                Student student = db.Students.Where(x => x.ID == id).FirstOrDefault<Student>();
                db.Students.Remove(student);
                db.SaveChanges();
            }
        }
    }
}